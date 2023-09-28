using FluentValidation;
using PhoneStore.ViewModels;

namespace PhoneStore.Validations;

public class AddPhoneValidator : AbstractValidator<CreatePhoneViewModel>
{
    public AddPhoneValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Название не может быть пустым")
            .Length(3,20).WithMessage("Название должно быть от 3-х до 20 символов");
        RuleFor(x => x.Price)
            .GreaterThan(100).WithName("Стоимость").WithMessage("{PropertyName} должна быть более 100")
            .LessThan(2000)
            .WithMessage("{PropertyName} должна быть не более 2000");
        RuleFor(x => x.BrandId).GreaterThan(0).WithMessage("Выберите бренд");
    }
}