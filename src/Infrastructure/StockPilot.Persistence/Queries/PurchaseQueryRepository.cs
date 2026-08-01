using System.Data;
using Dapper;
using StockPilot.Application.Abstractions.Connections;
using StockPilot.Application.Abstractions.Persistence.Queries;
using StockPilot.Application.Features.Purchases.Queries.GetPurchaseById;
using StockPilot.Application.Features.Purchases.Queries.GetPurchases;

namespace StockPilot.Persistence.Queries;

public class PurchaseQueryRepository(IDbConnectionFactory dbConnectionFactory) : IPurchaseQueryRepository
{
    public async Task<IEnumerable<GetPurchasesResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        const string sql = @"
            select
                p.id as Id,
                p.product_id as ProductId,
                pr.name AS ProductName,
                p.supplier_id as SupplierId,
                s.name AS SupplierName,
                p.quantity as Quantity,
                p.unit_price as UnitPrice,
                p.quantity * p.unit_price as TotalPrice,
                p.purchase_date as PurchaseDate,
                p.created_at as CreatedAt
            from purchases p
            join products pr on p.product_id = pr.id
            join suppliers s on p.supplier_id = s.id
            order by p.purchase_date desc, p.created_at desc";

        CommandDefinition command = new(sql, cancellationToken: cancellationToken);
        using IDbConnection connection = dbConnectionFactory.CreateConnection();
        return await connection.QueryAsync<GetPurchasesResponse>(command);
    }

    public async Task<GetPurchaseByIdResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            select
                p.id as Id,
                p.product_id as ProductId,
                pr.name AS ProductName,
                pr.description AS ProductDescription,
                p.supplier_id as SupplierId,
                s.name AS SupplierName,
                s.contact_name as ContactName,
                s.email as Email,
                s.phone as Phone,
                s.address as Address,
                p.quantity as Quantity,
                p.unit_price as UnitPrice,
                p.quantity * p.unit_price as TotalPrice,
                p.purchase_date as PurchaseDate,
                p.created_at as CreatedAt
            from purchases p
            join products pr on p.product_id = pr.id
            join suppliers s on p.supplier_id = s.id
            where p.id = @Id";

        CommandDefinition command = new(sql, new { Id = id }, cancellationToken: cancellationToken);
        using IDbConnection connection = dbConnectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<GetPurchaseByIdResponse>(command);
    }
}
