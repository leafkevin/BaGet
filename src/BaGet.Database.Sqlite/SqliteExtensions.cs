using BaGet.Core;
using BaGet.Core.Configuration;
using BaGet.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BaGet.Database.Sqlite;

public static class SqliteExtensions
{
    public static BaGetApplication AddSqliteDatabase(this BaGetApplication app )
    {
        app.Services.AddBaGetDbContextProvider<SqliteContext>("Sqlite", (provider, options) =>
        {
            var databaseOptions = provider.GetRequiredService<IOptionsSnapshot<DatabaseOptions>>();
            options.UseSqlite(databaseOptions.Value.ConnectionString);
        });
        return app;
    }
}
