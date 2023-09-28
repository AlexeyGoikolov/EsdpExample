using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using PhoneStore.Models;

namespace PhoneStore.ViewModels;

public class CreatePhoneViewModel
{
    public string? Title { get; set; }
    public decimal Price { get; set; }
    public int? BrandId { get; set; }

    public List<Brand>? Brands { get; set; }
}