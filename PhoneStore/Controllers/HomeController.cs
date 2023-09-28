using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PhoneStore.Models;
using PhoneStore.Services;
using PhoneStore.ViewModels;

namespace PhoneStore.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHostEnvironment _environment;
    private readonly FileUploadService _fileUploadService;

    public HomeController(ILogger<HomeController> logger, IHostEnvironment environment, FileUploadService fileUploadService)
    {
        _logger = logger;
        _environment = environment;
        _fileUploadService = fileUploadService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult UploadFile()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UploadFile(UploadFileViewModel model)
    {
        string filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/Files");
        string newName = "Картинка" + model.File.FileName.Substring(model.File.FileName.LastIndexOf('.'));
        string path = $"files/{newName}";
        
        await _fileUploadService.UploadFile(filePath, newName, model.File);
        return Json(new { message = "Файл загружен", path = path });
    }

    public IActionResult GetMessage()
    {
        return PartialView("PartialViews/_GetMessage");
    }

    public VirtualFileResult DownloadFile()
    {
        var filePath = Path.Combine("~/Files", "book.pdf");
        return File(filePath, "application/pdf", "book.pdf");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}