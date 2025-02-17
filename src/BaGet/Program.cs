using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using BaGet;
using BaGet.Core;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IConfigureOptions<CorsOptions>, ConfigureBaGetOptions>();
builder.Services.AddTransient<IConfigureOptions<FormOptions>, ConfigureBaGetOptions>();
builder.Services.AddTransient<IConfigureOptions<ForwardedHeadersOptions>, ConfigureBaGetOptions>();
builder.Services.AddTransient<IValidateOptions<BaGetOptions>, ConfigureBaGetOptions>();

// You can swap between implementations of subsystems like storage and search using BaGet's configuration.
// Each subsystem's implementation has a provider that reads the configuration to determine if it should be
// activated. BaGet will run through all its providers until it finds one that is active.
builder.Services.AddScoped(DependencyInjectionExtensions.GetServiceFromProviders<IContext>);
builder.Services.AddTransient(DependencyInjectionExtensions.GetServiceFromProviders<IStorageService>);
builder.Services.AddTransient(DependencyInjectionExtensions.GetServiceFromProviders<IPackageDatabase>);
builder.Services.AddTransient(DependencyInjectionExtensions.GetServiceFromProviders<ISearchService>);
builder.Services.AddTransient(DependencyInjectionExtensions.GetServiceFromProviders<ISearchIndexer>);
builder.Services.AddSingleton<IConfigureOptions<MvcRazorRuntimeCompilationOptions>, ConfigureRazorRuntimeCompilation>();
builder.Services.AddTransient<IUrlGenerator, BaGetUrlGenerator>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddBaGetApplication(f => f.AddMySqlDatabase().AddFileStorage());

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers()
    .AddApplicationPart(typeof(PackageContentController).Assembly)
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddRazorPages();
builder.Services.AddCors();

var webHost = builder.Build();
if (!webHost.ValidateStartupOptions())
    return;

// Configure the HTTP request pipeline.
if (!webHost.Environment.IsDevelopment())
{
    webHost.UseDeveloperExceptionPage();
    webHost.UseStatusCodePages();
}
webHost.UseForwardedHeaders();
var options = webHost.Configuration.Get<BaGetOptions>();
webHost.UsePathBase(options.PathBase);

webHost.UseStaticFiles();
webHost.UseRouting();

webHost.UseCors(ConfigureBaGetOptions.CorsPolicy);
webHost.UseMiddleware<OperationCancelledMiddleware>();

webHost.UseEndpoints(endpoints =>
{
    var baget = new BaGetEndpointBuilder();
    baget.MapEndpoints(endpoints);
});


var app = new CommandLineApplication
{
    Name = "baget",
    Description = "A light-weight NuGet service",
};

app.HelpOption(inherited: true);

app.Command("import", import =>
{
    import.Command("downloads", downloads =>
    {
        downloads.OnExecuteAsync(async cancellationToken =>
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var importer = scope.ServiceProvider.GetRequiredService<DownloadsImporter>();

                await importer.ImportAsync(cancellationToken);
            }
        });
    });
});

app.Option("--urls", "The URLs that BaGet should bind to.", CommandOptionType.SingleValue);

app.OnExecuteAsync(async cancellationToken =>
{
    await webHost.RunMigrationsAsync(cancellationToken);
    await webHost.RunAsync(cancellationToken);
});

await app.ExecuteAsync(args);
