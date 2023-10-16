using System.ComponentModel.DataAnnotations;

namespace PhoneStore.ViewModels.Account;

public class LoginViewModel
{
    [Required]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Электронный адрес")]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; }

    [Display(Name = "Запомнить")]
    public bool RememberMe { get; set; }

    public string ReturnUrl { get; set; }
}