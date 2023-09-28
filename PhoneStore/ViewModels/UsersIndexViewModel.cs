using PhoneStore.Models;

namespace PhoneStore.ViewModels;

public class UsersIndexViewModel
{
    public IEnumerable<User> Users { get; set; }
    public PageViewModel PageViewModel { get; set; }
    
}