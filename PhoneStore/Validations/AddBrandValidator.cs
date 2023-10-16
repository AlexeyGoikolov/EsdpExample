using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PhoneStore.Context;
using PhoneStore.ViewModels;

namespace PhoneStore.Validations;

public class AddBrandValidator : AbstractValidator<CreateBrandViewModel>
{

    public AddBrandValidator()
    {
        RuleFor(x => x.Name)
            .NotNull().WithMessage("Имя бренда обязательно")
            .MinimumLength(3).WithMessage("Название должно быть не менее 3-х символов");
        RuleFor(x => x.Phone).NotNull().WithName("Номер телефона")
            .Matches(@"^\+7([0-9]{10}$)").WithMessage("Номер телефона не соответсвует формату +77777777777");
        RuleFor(x => x.Email).NotNull().WithName("Электронный адрес")
            .EmailAddress().WithMessage("{PropertyName} не соответсвует формату");
    }
}