using System.Data;
using Dapper;
using StockPilot.Application.Abstractions.Connections;
using StockPilot.Application.Abstractions.Persistence.Queries;
using StockPilot.Application.Features.Products.Queries.GetProductById;
using StockPilot.Application.Features.Products.Queries.GetProducts;
using StockPilot.Application.Features.ProductSuppliers.Queries.GetProductSuppliers;

namespace StockPilot.Persistence.Queries;

public class ProductQueryRepository(IDbConnectionFactory dbConnectionFactory) : IProductQueryRepository
{
    public async Task<GetProductByIdResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        const string query = """
                             SELECT
                                 p.name AS Name,
                                 p.description AS Description,
                                 p.purchase_price AS PurchasePrice,
                                 p.sale_price AS SalePrice,
                                 p.category_id AS CategoryId,
                                 c.name AS CategoryName
                             FROM products p
                             INNER JOIN categories c
                                 ON c.id = p.category_id
                             WHERE p.id = @Id;

                             SELECT
                                 s.id AS Id,
                                 s.name AS Name,
                                 s.email AS Email,
                                 s.phone AS Phone
                             FROM suppliers s
                             INNER JOIN product_suppliers ps
                                 ON ps.supplier_id = s.id
                             WHERE ps.product_id = @Id;
                             """;

        using IDbConnection connection =
            dbConnectionFactory.CreateConnection();

        CommandDefinition command = new(
            commandText: query,
            parameters: new { Id = id },
            cancellationToken: cancellationToken);

        await using SqlMapper.GridReader multi =
            await connection.QueryMultipleAsync(command);

        GetProductByIdRow? product =
            await multi.ReadSingleOrDefaultAsync<GetProductByIdRow>();

        if (product is null)
        {
            return null;
        }

        IEnumerable<GetSuppliersByProduct> suppliers =
            await multi.ReadAsync<GetSuppliersByProduct>();

        return new GetProductByIdResponse(
            Name: product.Name,
            Description: product.Description,
            PurchasePrice: product.PurchasePrice,
            SalePrice: product.SalePrice,
            CategoryId: product.CategoryId,
            CategoryName: product.CategoryName,
            Suppliers: suppliers);
    }

    public async Task<GetProductsResponse> GetAllAsync(GetProductsQuery productsQuery,
        CancellationToken cancellationToken = default)
    {
        var builder = new SqlBuilder();

        if (!string.IsNullOrWhiteSpace(productsQuery.Name))
        {
            builder.Where("p.name LIKE @Name", new { Name = productsQuery.Name.Trim() });
        }

        if (productsQuery.MinSalePrice is not null)
        {
            builder.Where("p.sale_price >= @MinSalePrice", new { productsQuery.MinSalePrice });
        }

        if (productsQuery.MaxSalePrice is not null)
        {
            builder.Where("p.sale_price <= @MaxSalePrice", new { productsQuery.MaxSalePrice });
        }

        if (!string.IsNullOrWhiteSpace(productsQuery.Search))
        {
            builder.Where("(p.name ILIKE @Search OR p.description ILIKE @Search)", new { Search = $"%{productsQuery.Search.Trim()}%" });
        }

        string sortBy = productsQuery.SortBy?.ToLower() switch
        {
            "name" => "p.name",
            "saleprice" => "p.sale_price",
            "purchaseprice" => "p.purchase_price",
            "createdat" => "p.created_at",
            _ => "p.name"
        };

        string sortDirection = productsQuery.SortDirection?.ToLower() == "desc" ? "DESC" : "ASC";
        builder.OrderBy(sortBy, sortDirection);

        SqlBuilder.Template template = builder.AddTemplate(@"
        SELECT
            p.id AS Id,
            p.name AS Name,
            p.purchase_price AS PurchasePrice,
            p.sale_price AS SalePrice,
            p.category_id AS CategoryId,
            c.name AS CategoryName
        FROM products p
        join categories c
            on p.category_id = c.id
        /**where**/
        /**orderby**/
        OFFSET @Offset ROWS
        FETCH NEXT @PageSize ROWS ONLY;

        SELECT COUNT(*)
        FROM products p
        /**where**/;");

        var parameters = new DynamicParameters(template.Parameters);
        parameters.Add("Offset", (productsQuery.Page - 1) * productsQuery.PageSize);
        parameters.Add("PageSize", productsQuery.PageSize);

        using IDbConnection connection = dbConnectionFactory.CreateConnection();
        var command = new CommandDefinition(
            commandText: template.RawSql,
            parameters: parameters,
            cancellationToken: cancellationToken
        );

        await using SqlMapper.GridReader multi = await connection.QueryMultipleAsync(command);
        IEnumerable<GetProductResponse> items = await multi.ReadAsync<GetProductResponse>();
        int totalCount = await multi.ReadSingleAsync<int>();
        int totalPages = (int)Math.Ceiling((double)totalCount / productsQuery.PageSize);

        return new GetProductsResponse(items, productsQuery.Page, productsQuery.PageSize, totalCount, totalPages);
    }

    public async Task<IEnumerable<GetProductSuppliersResponse>> GetProductSuppliersAsync(Guid productId,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
                                select
                                    s.id as Id,
                                    s.name as Name,
                                    s.email as Email,
                                    s.phone as  Phone
                                from suppliers s
                                join product_suppliers ps
                                    on s.id = ps.supplier_id
                                where ps.product_id = @ProductId
                           """;

        CommandDefinition command = new(sql, new { ProductId = productId }, cancellationToken: cancellationToken);
        using IDbConnection connection = dbConnectionFactory.CreateConnection();
        return await connection.QueryAsync<GetProductSuppliersResponse>(command);
    }

    private sealed record GetProductByIdRow(
        string Name,
        string? Description,
        decimal PurchasePrice,
        decimal SalePrice,
        Guid CategoryId,
        string CategoryName);
}
