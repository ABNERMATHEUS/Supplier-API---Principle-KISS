using FluentValidation;

namespace Supplier.Domain.Requests;

public record SupplierCreateRequest(string Name, string Description, string Category, string Image, string Phone, string Email, string Website);

public class SupplierCreateRequestValidator : AbstractValidator<SupplierCreateRequest>
{
    public SupplierCreateRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(500);
        RuleFor(x => x.Category).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Image).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Phone).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Email).NotEmpty().MaximumLength(100).EmailAddress();
        RuleFor(x => x.Website).MaximumLength(100);
    }
}
