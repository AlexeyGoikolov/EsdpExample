using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneStore.Context;
using PhoneStore.Models;

namespace PhoneStore.Controllers;

public class OrdersController : Controller
{
    private readonly MobileContext _db;

    public OrdersController(MobileContext context)
    {
        _db = context;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        var orders = _db.Orders
            .Include(o => o.Phone)
            .ToList();
        return View(orders);
    }
    
    [HttpGet]
    public IActionResult Create(int phoneId)
    {
        if (phoneId == 0)
            return BadRequest();
        var phone = _db.Phones.FirstOrDefault(p => p.Id == phoneId);
        if (phone is null)
            return NotFound();
        return View(new Order{Phone = phone});
    }

    [HttpPost]
    public IActionResult Create(Order? order)
    {
        if (order != null)
        {
            _db.Orders.Add(order);
            _db.SaveChanges();
            return RedirectToAction("Index", "Phones");
        }
        
        return BadRequest();
    }
}