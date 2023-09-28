using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using PhoneStore.Context;
using PhoneStore.Models;
using PhoneStore.ViewModels;

namespace PhoneStore.Controllers;

public class BrandsController : Controller
{
    private readonly MobileContext _context;
    private IValidator<CreateBrandViewModel> _validator;

    public BrandsController(MobileContext context, IValidator<CreateBrandViewModel> validator)
    {
        _context = context;
        _validator = validator;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var brands = _context.Brands.ToList();
        return View(brands);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(CreateBrandViewModel model)
    {
        var result = await _validator.ValidateAsync(model);
        if (result.IsValid)
        {
            var brand = new Brand
            {
                Name = model.Name,
                Address = model.Address
            };
            _context.Brands.Add(brand);
            _context.SaveChanges();
            return RedirectToAction("Index", "Phones");
        }
        result.AddToModelState(ModelState);
        return View(model);
    }
}