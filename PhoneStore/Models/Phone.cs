using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using PhoneStore.Enums;

namespace PhoneStore.Models;

public class Phone
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public decimal Price { get; set; }
    
    public int BrandId { get; set; }
    
    public Brand? Brand { get; set; }
    
    public virtual ICollection<Order> Orders { get; set; }

    public Phone()
    {
    }

    public Phone(string title, decimal price, int brandId)
    {
        Title = title;
        BrandId = brandId;
        Price = price;
    }
}