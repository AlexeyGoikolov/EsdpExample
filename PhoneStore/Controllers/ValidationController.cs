using Microsoft.AspNetCore.Mvc;
using PhoneStore.Context;
using PhoneStore.ViewModels;

namespace PhoneStore.Controllers;

public class ValidationController : Controller
{
    private MobileContext _db;
    
    public ValidationController(MobileContext db)
    {
        _db = db;
    }
    
    // GET
    [AcceptVerbs("GET", "POST")]
    public bool CheckName(string title)
    {
        return !_db.Phones.Any(b => b.Title == title);
    }
}