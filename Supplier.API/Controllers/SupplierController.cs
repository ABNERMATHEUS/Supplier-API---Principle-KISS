using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Supplier.Domain.Dtos;
using Supplier.Domain.Requests;

namespace Supplier.API.Controllers;

[ApiController]
[Route("[controller]")]
public class SupplierController : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> CreateSupplier(
        [FromBody] SupplierCreateRequest request,
        [FromServices] IMongoCollection<Domain.Entities.Supplier> collection)
    {
        var supplier = new Domain.Entities.Supplier(request.Name, request.Category, request.Image, request.Phone, request.Email, request.Website);
        await collection.InsertOneAsync(supplier);
        return Ok(supplier);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteSupplier(
               [FromQuery] Guid id,
               [FromServices] IMongoCollection<Domain.Entities.Supplier> collection)
    {
        var result = await collection.DeleteOneAsync(x => x.Id == id);
        if (result.DeletedCount == 0)
            return NotFound();
        
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateSupplier(
                      [FromBody] SupplierUpdateRequest request,
                      [FromServices] IMongoCollection<Domain.Entities.Supplier> collection)
    {
        var supplier = await collection.Find(x => x.Id == request.Id).FirstOrDefaultAsync();
        if (supplier is null)
            return NotFound();

        supplier.Update(request.Name, request.Category, request.Image, request.Phone, request.Email, request.Website);
        await collection.ReplaceOneAsync(x => x.Id == request.Id, supplier);
        return Ok(supplier);
    }

    [HttpGet]
    public async Task<IActionResult> GetSupplier(
                      [FromQuery] Guid id,
                                    [FromServices] IMongoCollection<Domain.Entities.Supplier> collection)
    {
        var supplier = await collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        if (supplier is null)
            return NotFound();
        
        var supplierDto = (SupplierDto)supplier;
        return Ok(supplier);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllSuppliers(
               [FromQuery] int page,
               [FromQuery] int pageSize,
               [FromServices] IMongoCollection<Domain.Entities.Supplier> collection)
    {
        var suppliers = await collection.Find(x => true).Skip(page * pageSize).Limit(pageSize).ToListAsync();
        var suppliersDto = suppliers.Select(x => (SupplierDto)x).ToList();
        return Ok(suppliersDto);
    }
}
