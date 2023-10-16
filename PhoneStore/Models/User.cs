using Microsoft.AspNetCore.Identity;

namespace PhoneStore.Models;

public class User : IdentityUser
{
    public DateTime DateOfBirth { get; set; }
}



