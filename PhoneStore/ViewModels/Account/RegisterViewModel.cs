using System.ComponentModel.DataAnnotations;

namespace PhoneStore.ViewModels.Account;

public class RegisterViewModel
{
    [Required]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email")]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Дата рождения")]
    public DateTime DateOfBirth { get; set; }
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; }
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Подтвердить пароль")]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    public string PasswordConfirm { get; set; }
}