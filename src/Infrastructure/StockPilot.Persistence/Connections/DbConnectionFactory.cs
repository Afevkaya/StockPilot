using System.Data;
using Npgsql;
using StockPilot.Application.Abstractions.Connections;

namespace StockPilot.Persistence.Connections;

public class DbConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public IDbConnection CreateConnection()
    {
        var connection = new NpgsqlConnection(connectionString);
        return connection;
    }
}
