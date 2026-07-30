namespace StockPilot.Application.Features.Suppliers.Queries.GetAllSuppliers;

public record GetAllSuppliersQuery(
    int Page = 1,
    int PageSize = 10,
    string? Name = null,
    string? Email = null,
    string? Phone = null,
    string? SortBy = null,
    string? SortDirection = "asc",
    string? Search = null);
