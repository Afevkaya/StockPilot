using System.Data;

namespace StockPilot.Application.Abstractions.Connections;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
