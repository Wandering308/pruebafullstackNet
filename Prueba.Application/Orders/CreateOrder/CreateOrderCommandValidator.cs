using FluentValidation;

namespace Prueba.Application.Orders.CreateOrder;

public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Customer)
            .NotEmpty().WithMessage("Customer is required")
            .MaximumLength(200);

        RuleFor(x => x.Product)
            .NotEmpty().WithMessage("Product is required")
            .MaximumLength(200);

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be > 0");

        // Latitud: -90..90
        RuleFor(x => x.OriginLat).InclusiveBetween(-90, 90);
        RuleFor(x => x.DestinationLat).InclusiveBetween(-90, 90);

        // Longitud: -180..180
        RuleFor(x => x.OriginLon).InclusiveBetween(-180, 180);
        RuleFor(x => x.DestinationLon).InclusiveBetween(-180, 180);
    }
}
