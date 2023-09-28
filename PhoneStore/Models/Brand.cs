using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneStore.Models;

public class Brand
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Address { get; set; }
    
    [NotMapped]
    public List<User> Users { get; set; }

    public Brand()
    {
        Users = new List<User>();
    }
}