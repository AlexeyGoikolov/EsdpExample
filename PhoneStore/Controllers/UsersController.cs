using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneStore.Context;
using PhoneStore.Enums;
using PhoneStore.Models;
using PhoneStore.ViewModels;

namespace PhoneStore.Controllers;

public class UsersController : Controller
{
    private readonly MobileContext _context;

    public UsersController(MobileContext context)
    {
        _context = context;
    }
    
}