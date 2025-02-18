using System;
using BaGet.Core;
using BaGet.Core.Configuration;
using BaGet.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BaGet.Database.PostgreSql;

public static class PostgreSqlExtensions
{
    public static BaGetApplication AddPostgreSqlDatabase(this BaGetApplication app)
    {
        app.Services.AddBaGetDbContextProvider<PostgreSqlContext>("PostgreSql", (provider, options) =>
        {
            var databaseOptions = provider.GetRequiredService<IOptionsSnapshot<DatabaseOptions>>();
            options.UseNpgsql(databaseOptions.Value.ConnectionString);
        });

        return app;
    }

    public static BaGetApplication AddPostgreSqlDatabase(this BaGetApplication app, Action<DatabaseOptions> configure)
    {
        app.AddPostgreSqlDatabase();
        app.Services.Configure(configure);
        return app;
    }
}
