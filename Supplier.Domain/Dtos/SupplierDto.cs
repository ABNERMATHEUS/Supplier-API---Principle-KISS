namespace Supplier.Domain.Dtos;
public record SupplierDto(Guid Id, string Name, string Description, string Category, string Image, string Phone, string Email, string Website, DateTime CreatedAt);

