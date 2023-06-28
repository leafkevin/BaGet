using BaGet.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace BaGet;

public static class IHostExtensions
{
    public static async Task RunMigrationsAsync(
        this IHost host,
        CancellationToken cancellationToken = default)
    {
        // Run migrations if necessary.
        var options = host.Services.GetRequiredService<IOptions<BaGetOptions>>();

        if (options.Value.RunMigrationsAtStartup)
        {
            using (var scope = host.Services.CreateScope())
            {
                var ctx = scope.ServiceProvider.GetService<IContext>();
                if (ctx != null)
                {
                    await ctx.RunMigrationsAsync(cancellationToken);
                }
            }
        }
    }

    public static bool ValidateStartupOptions(this IHost host)
    {
        return host
            .Services
            .GetRequiredService<ValidateStartupOptions>()
            .Validate();
    }
}
