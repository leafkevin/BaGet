using System;
using BaGet.Core;
using BaGet.Core.Configuration;
using BaGet.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BaGet.Database.MySql;

public static class MySqlExtensions
{
    public static BaGetApplication AddMySqlDatabase(this BaGetApplication app, string mySqlVersion = "8.4.4")
    {
        app.Services.AddBaGetDbContextProvider<MySqlContext>("MySql", (provider, options) =>
        {
            var databaseOptions = provider.GetRequiredService<IOptionsSnapshot<DatabaseOptions>>();
            options.UseMySql(databaseOptions.Value.ConnectionString, new MySqlServerVersion(mySqlVersion));
        });
        return app;
    }
    public static BaGetApplication AddMySqlDatabase(this BaGetApplication app, Action<DatabaseOptions> configure, string mySqlVersion = "8.4.4")
    {
        app.AddMySqlDatabase(mySqlVersion);
        app.Services.Configure(configure);
        return app;
    }
}
