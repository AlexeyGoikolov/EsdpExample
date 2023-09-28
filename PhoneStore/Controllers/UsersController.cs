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

    [HttpGet]
    public async Task<IActionResult> Paging(int page = 1)
    {
        int pageSize = 2;
        IQueryable<User> users = _context.Users.Include(u => u.Brand);
        var count = await users.CountAsync();
        var items = await users.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        PageViewModel model = new PageViewModel(count, page, pageSize);
        UsersIndexViewModel viewModel = new UsersIndexViewModel
        {
            PageViewModel = model,
            Users = items
        };
        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Index(SortState state = SortState.NameAsc)
    {
        IQueryable<User> users = _context.Users.Include(x => x.Brand);
        ViewBag.NameSort = state == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
        ViewBag.AgeSort = state == SortState.AgeAsc ? SortState.AgeDesc : SortState.AgeAsc;
        ViewBag.BrandSort = state == SortState.BrandAsc ? SortState.BrandDesc : SortState.BrandAsc;

        
        return View(await GetSortedUsers(state, users));
    }

    private async Task<List<User>> GetSortedUsers(SortState state, IQueryable<User> users)
    {
        switch (state)
        {
            case SortState.NameDesc:
                users = users.OrderByDescending(u => u.Name);
                break;
            case SortState.AgeAsc:
                users = users.OrderBy(u => u.Age);
                break;
            case SortState.AgeDesc:
                users = users.OrderByDescending(u => u.Age);
                break;
            case SortState.BrandAsc:
                users = users.OrderBy(u => u.Brand);
                break;
            case SortState.BrandDesc:
                users = users.OrderByDescending(u => u.Brand);
                break;
            default:
                users = users.OrderBy(u => u.Name);
                break;
        }

        return await users.ToListAsync();
    }
}