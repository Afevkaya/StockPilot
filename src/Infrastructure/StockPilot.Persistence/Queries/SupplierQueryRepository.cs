using System.Data;
using Dapper;
using StockPilot.Application.Abstractions.Connections;
using StockPilot.Application.Abstractions.Persistence.Queries;
using StockPilot.Application.Features.Suppliers.Queries.GetAllSuppliers;
using StockPilot.Application.Features.Suppliers.Queries.GetSupplierById;

namespace StockPilot.Persistence.Queries;

public class SupplierQueryRepository(IDbConnectionFactory dbConnectionFactory) : ISupplierQueryRepository
{
    public async Task<GetAllSuppliersResponse?> GetAllAsync(GetAllSuppliersQuery query,
        CancellationToken cancellationToken = default)
    {
        var whereConditions = new List<string>();
        var parameters = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(query.Name))
        {
            whereConditions.Add("name ILIKE @Name");
            parameters.Add("Name", query.Name.Trim());
        }

        if (!string.IsNullOrWhiteSpace(query.Email))
        {
            whereConditions.Add("email ILIKE @Email");
            parameters.Add("Email", query.Email.Trim());
        }

        if (!string.IsNullOrWhiteSpace(query.Phone))
        {
            whereConditions.Add("phone ILIKE @Phone");
            parameters.Add("Phone", query.Phone.Trim());
        }

        if (!string.IsNullOrEmpty(query.Search))
        {
            whereConditions.Add("(name ILIKE @SearchTerm OR contact_name ILIKE @SearchTerm)");
            parameters.Add("SearchTerm", $"%{query.Search.Trim()}%");
        }

        string whereClause = whereConditions.Count > 0 ? $"WHERE {string.Join(" AND ", whereConditions)}" : string.Empty;

        string sortBy = query.SortBy?.ToLower() switch
        {
            "name" => "name",
            "email" => "email",
            "createdat" => "created_at",
            "updatedat" => "updated_at",
            _ => "name"
        };

        string sortDirection = query.SortDirection?.ToLower() == "desc" ? "DESC" : "ASC";

        string sql = $@"
            SELECT s.id as Id, s.name as Name, s.email as Email, s.phone as Phone
            FROM suppliers s
            {whereClause}
            ORDER BY s.{sortBy} {sortDirection}
            LIMIT @Limit OFFSET @Offset;

            SELECT COUNT(*) FROM suppliers {whereClause};
        ";
        parameters.Add("Limit", query.PageSize);
        parameters.Add("Offset", (query.Page - 1) * query.PageSize);

        using IDbConnection connection = dbConnectionFactory.CreateConnection();
        var command = new CommandDefinition(sql, parameters, cancellationToken: cancellationToken);

        await using SqlMapper.GridReader multi = await connection.QueryMultipleAsync(command);
        IEnumerable<GetAllSupplierResponse> items = await multi.ReadAsync<GetAllSupplierResponse>();
        int totalCount = await multi.ReadSingleAsync<int>();
        int totalPages = (int)Math.Ceiling((double)totalCount / query.PageSize);

        return new GetAllSuppliersResponse(items, query.Page, query.PageSize, totalCount, totalPages);
    }

    public async Task<GetSupplierByIdResponse?> GetByIdAsync(GetSupplierByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        string sql = @"
            SELECT s.id as Id,
                   s.name as Name,
                   s.contact_name as ContactName,
                   s.email as Email,
                   s.phone as Phone,
                   s.address as Address,
                   s.created_at as Createdat,
                   s.updated_at as Updatedat
            FROM suppliers s
            WHERE id = @Id
        ";

        CommandDefinition commandDefinition = new(sql, new { Id = query.Id }, cancellationToken: cancellationToken);
        using IDbConnection connection = dbConnectionFactory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<GetSupplierByIdResponse>(commandDefinition);
    }
}
