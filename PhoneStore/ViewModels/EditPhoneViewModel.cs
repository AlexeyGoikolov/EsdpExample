using PhoneStore.Models;

namespace PhoneStore.ViewModels;

public class EditPhoneViewModel
{
    public Phone Phone { get; set; }
    public List<Brand> Brands { get; set; }
}