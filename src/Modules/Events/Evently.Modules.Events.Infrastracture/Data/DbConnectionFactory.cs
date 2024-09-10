using System.Data.Common;
using Evently.Modules.Events.Application.Abstractions.Data;
using Npgsql;

namespace Evently.Modules.Events.Infrastracture.Data;

internal sealed class DbConnectionFactory(NpgsqlDataSource dataSource):IDbConnectionFactory
{
    public async  ValueTask<DbConnection> OpenConnectionAsync()
    {
        return await dataSource.OpenConnectionAsync();
    }
}