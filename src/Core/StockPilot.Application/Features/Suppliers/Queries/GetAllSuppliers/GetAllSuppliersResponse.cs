namespace StockPilot.Application.Features.Suppliers.Queries.GetAllSuppliers;

public record GetAllSuppliersResponse(IEnumerable<GetAllSupplierResponse> Items, int Page, int PageSize, int TotalCount, int TotalPages);
public record GetAllSupplierResponse(Guid Id, string Name, string Email, string Phone);
