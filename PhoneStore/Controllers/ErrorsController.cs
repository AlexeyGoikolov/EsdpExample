using Microsoft.AspNetCore.Mvc;

namespace PhoneStore.Controllers;

public class ErrorsController : Controller
{
    [HttpGet]
    [Route("Error/{statusCode}")]
    public IActionResult Index(int statusCode)
    {
        switch (statusCode)
        {
            case 404:
                return View("NotFound");
            case 400:
                return View("BadRequest");
            default:
                return View("ServerError");
        }
    }
}