using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Supplier.Domain.Dtos;
using Supplier.Domain.Requests;

namespace Supplier.Domain.Entities;

public class Supplier
{
    public Supplier(string name, string category, string image, string phone, string email, string website)
    {
        Id = Guid.NewGuid();
        Name = name;
        Category = category;
        Image = image;
        Phone = phone;
        Email = email;
        Website = website;
        CreatedAt = DateTime.Now;
    }

    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; set; }
    public string Category { get; private set; }
    public string Image { get; private set; }
    public string Phone { get; private set; }
    public string Email { get; private set; }
    public string Website { get; private set; }
    public DateTime CreatedAt { get; set; }
    
    public void Update(string name, string category, string image, string phone, string email, string website)
    {
        Name = name;
        Category = category;
        Image = image;
        Phone = phone;
        Email = email;
        Website = website;
    }
    
    public static explicit operator Supplier(SupplierCreateRequest request)
    {
        return new Supplier(request.Name, request.Category, request.Image, request.Phone, request.Email, request.Website);
    }
    
    public static explicit operator SupplierDto(Supplier supplier)
    {
        return new SupplierDto(supplier.Id, supplier.Name, supplier.Description, supplier.Category, supplier.Image, supplier.Phone, supplier.Email, supplier.Website, supplier.CreatedAt);
    }
}
