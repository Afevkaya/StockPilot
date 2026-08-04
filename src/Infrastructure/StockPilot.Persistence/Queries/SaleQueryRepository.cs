using System.Data;
using Dapper;
using StockPilot.Application.Abstractions.Connections;
using StockPilot.Application.Abstractions.Persistence.Queries;
using StockPilot.Application.Features.Sales.Queries.GetSaleById;
using StockPilot.Application.Features.Sales.Queries.GetSales;

namespace StockPilot.Persistence.Queries;

public class SaleQueryRepository(IDbConnectionFactory dbConnectionFactory) : ISaleQueryRepository
{
    public async Task<GetSalesResponse> GetSalesAsync(GetSalesQuery query, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            SELECT
                s.id AS SaleId,
                s.product_id as ProductId,
                p.name AS ProductName,
                s.quantity as Quantity,
                s.unit_price as UnitPrice,
                s.quantity * s.unit_price AS TotalPrice,
                s.sale_date as SaleDate,
                s.created_at as CreatedAt
            FROM Sales s
            INNER JOIN products p ON s.product_id = p.id
            ORDER BY s.sale_date desc, s.created_at DESC, s.id desc
            offset @Offset rows fetch next @PageSize rows only;

            select count(*) from sales";

        CommandDefinition command = new(sql,
            new { Offset = (query.Page - 1) * query.PageSize, PageSize = query.PageSize },
            cancellationToken: cancellationToken);

        using IDbConnection connection = dbConnectionFactory.CreateConnection();
        await using SqlMapper.GridReader multi = await connection.QueryMultipleAsync(command);
        IEnumerable<GetSaleResponse> sales = await multi.ReadAsync<GetSaleResponse>();
        int totalCount = await multi.ReadSingleAsync<int>();
        int totalPages = (int)Math.Ceiling((double)totalCount / query.PageSize);

        return new GetSalesResponse(query.Page, query.PageSize, totalCount, totalPages, sales);

    }

    public async Task<GetSaleByIdResponse?> GetSaleByIdAsync(Guid saleId, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            SELECT
                s.id AS SaleId,
                s.product_id as ProductId,
                p.name AS ProductName,
                s.quantity as Quantity,
                s.unit_price as UnitPrice,
                s.quantity * s.unit_price AS TotalPrice,
                s.sale_date as SaleDate,
                s.created_at as CreatedAt
            FROM sales s
            INNER JOIN products p ON s.product_id = p.id
            WHERE s.id = @SaleId";

        CommandDefinition command = new(sql, new { SaleId = saleId }, cancellationToken: cancellationToken);
        using IDbConnection connection = dbConnectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<GetSaleByIdResponse>(command);
    }
}
