using System.Text.Json.Serialization;
using BaGet.Core;
using BaGet.Core.Configuration;
using BaGet.Core.Entities;
using BaGet.Core.Extensions;
using BaGet.Core.Search;
using BaGet.Core.Storage;
using BaGet.Database.MySql;
using BaGet.Web.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace BaGet.Web;

public static class Startup
{
    public static void AddDomainServivces(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddTransient<IConfigureOptions<CorsOptions>, ConfigureBaGetOptions>();
        services.AddTransient<IConfigureOptions<FormOptions>, ConfigureBaGetOptions>();
        services.AddTransient<IConfigureOptions<ForwardedHeadersOptions>, ConfigureBaGetOptions>();
        services.AddTransient<IValidateOptions<BaGetOptions>, ConfigureBaGetOptions>();

        services
            .AddRouting(options => options.LowercaseUrls = true)
            .AddControllers()
            .AddApplicationPart(typeof(PackageContentController).Assembly)
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

        services.AddRazorPages();
        services.AddHttpContextAccessor();
        services.AddTransient<IUrlGenerator, BaGetUrlGenerator>();
        services.AddBaGetApplication(f =>  f.AddMySqlDatabase().AddFileStorage()); 

        services.AddScoped(DependencyInjectionExtensions.GetServiceFromProviders<IContext>);
        services.AddTransient(DependencyInjectionExtensions.GetServiceFromProviders<IStorageService>);
        services.AddTransient(DependencyInjectionExtensions.GetServiceFromProviders<IPackageDatabase>);
        services.AddTransient(DependencyInjectionExtensions.GetServiceFromProviders<ISearchService>);
        services.AddTransient(DependencyInjectionExtensions.GetServiceFromProviders<ISearchIndexer>);

        services.AddSingleton<IConfigureOptions<MvcRazorRuntimeCompilationOptions>, ConfigureRazorRuntimeCompilation>();
        services.AddCors();
    }

    public static void Configure(WebApplication app)
    {
        var options = app.Configuration.Get<BaGetOptions>();
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
        }
        app.UsePathBase(options.PathBase);
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors(ConfigureBaGetOptions.CorsPolicy);
        app.UseOperationCancelledMiddleware();
        new BaGetEndpointBuilder().MapEndpoints(app);
    }
}
