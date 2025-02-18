using System;
using BaGet.Core;
using BaGet.Core.Configuration;
using BaGet.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BaGet.Database.SqlServer;

public static class SqlServerExtensions
{
    public static BaGetApplication AddSqlServerDatabase(this BaGetApplication app)
    {
        app.Services.AddBaGetDbContextProvider<SqlServerContext>("SqlServer", (provider, options) =>
        {
            var databaseOptions = provider.GetRequiredService<IOptionsSnapshot<DatabaseOptions>>();
            options.UseSqlServer(databaseOptions.Value.ConnectionString);
        });
        return app;
    }

    public static BaGetApplication AddSqlServerDatabase(this BaGetApplication app, Action<DatabaseOptions> configure)
    {
        app.AddSqlServerDatabase();
        app.Services.Configure(configure);
        return app;
    }
}
