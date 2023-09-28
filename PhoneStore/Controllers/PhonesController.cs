using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneStore.Context;
using PhoneStore.Models;
using PhoneStore.Validations;
using PhoneStore.ViewModels;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;

namespace PhoneStore.Controllers;


public class PhonesController : Controller
{
    private readonly MobileContext _db;
    private readonly IWebHostEnvironment _environment;
    private IValidator<CreatePhoneViewModel> _validator;

    public PhonesController(MobileContext db, IWebHostEnvironment environment, IValidator<CreatePhoneViewModel> validator)
    {
        _db = db;
        _environment = environment;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> Index(PhonesFilterViewModel? model)
    {
        List<Phone> phones = _db.Phones
            .Include(p => p.Brand)
            .OrderBy(p => p.Title)
            .ThenBy(p => p.Price)
            .ToList();
        
        var averagePrice = phones.Average(p => p.Price);
        var phone2 = phones.Where(p => p.Brand.Name == "Samsung").MaxBy(p => p.Price);
    
        
        var phones2 = await (from phone in _db.Phones.Include(p => p.Brand) 
            where phone.BrandId == 1
                select phone).ToListAsync();
        var intersected = phones.Intersect(phones2);
        if (model.Name != null || model.Brand != null)
        {
            if (!string.IsNullOrEmpty(model.Name))
            {
                phones = phones.Where(p => p.Title.Contains(model.Name, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            else if (!string.IsNullOrEmpty(model.Brand))
            {
                phones = phones.Where(p => p.Brand.Name.Contains(model.Brand, StringComparison.OrdinalIgnoreCase)).ToList();
            }
        }
        
        return View(phones);
    }
    
    [HttpGet]
    public IActionResult Add()
    {
        var brands = _db.Brands.ToList();
        CreatePhoneViewModel model = new CreatePhoneViewModel
        {
            Brands = brands
        };
        return View(model);
    }
    
    [HttpPost]
    public IActionResult Add(CreatePhoneViewModel model)
    {
        var result = _validator.Validate(model);
        if (result.IsValid)
        {
            Phone phone = new Phone(model.Title, model.Price, model.BrandId.Value);
            _db.Phones.Add(phone);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        result.AddToModelState(ModelState);
        model.Brands = _db.Brands.ToList();
        return View(model);
    }

    [HttpGet]
    public IActionResult Edit(int? phoneId)
    {
        if (phoneId is null)
            return BadRequest();
        var phone = _db.Phones.FirstOrDefault(p => p.Id == phoneId);
        if (phone is null)
            return NotFound();

        EditPhoneViewModel model = new EditPhoneViewModel
        {
            Phone = phone,
            Brands = _db.Brands.ToList()
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult Edit(EditPhoneViewModel model)
    {
        if (model.Phone.Price == 0 || string.IsNullOrEmpty(model.Phone.Title) || model.Phone.BrandId == 0)
        {
            ViewBag.Error = "Не все поля заполнены";
            return View(model);
        }

        _db.Phones.Update(model.Phone);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult ConfirmDelete(int? phoneId)
    {
        if (phoneId is null)
            return BadRequest();
        var phone = _db.Phones.FirstOrDefault(p => p.Id == phoneId);
        if (phone is null)
            return NotFound();

        return View(phone);
    }

    [HttpPost]
    [ActionName("Delete")]
    public async Task<IActionResult> Delete(int? phoneId)
    {
        if (phoneId is null)
            return BadRequest();
        var phone = await _db.Phones.FirstOrDefaultAsync(p => p.Id == phoneId);
        if (phone is null)
            return NotFound();
        
        _db.Phones.Remove(phone);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    [HttpGet]
    [ActionName("file")]
    public IActionResult GetPhoneDescriptionFile(int? id)
    {
        if (id.HasValue)
        {
            var phone = _db.Phones
                .Include(p => p.Brand)
                .FirstOrDefault(p => p.Id == id);
            if (phone is not null)
            {
                var file = GetDescriptionFile(phone);
                return File(file, "application/pdf", $"{phone.Brand?.Name} {phone.Title}.pdf");
            }

            return NotFound();
        }

        return BadRequest();
    }

    private byte[] GetDescriptionFile(Phone phone)
    {
        var document = Document.Create(container =>
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(1f, Unit.Centimetre);
                page.PageColor(Colors.Grey.Lighten5);
                page.DefaultTextStyle(x =>
                    x.FontSize(16)
                        .FontFamily("Arial")
                        .FontColor(Colors.Black));

                page.Header().AlignCenter()
                    .ShowOnce()
                    .Text($"Описание модели {phone.Title}")
                    .Bold()
                    .FontSize(26)
                    .FontColor(Colors.Green.Medium);
                
                page.Content()
                    .Column(column =>
                    {
                        column.Spacing(20);
                        
                        column.Item().Row(row =>
                        {
                            row.RelativeItem()
                                .PaddingTop(1f, Unit.Centimetre)
                                .MaxHeight(400f)
                                .MaxWidth(300f)
                                .Image(Path.Combine(_environment.WebRootPath, "Files\\galaxy-S23-Plus-.jpg"));
                            
                            row.RelativeItem()
                                .AlignLeft()
                                .PaddingTop(1f, Unit.Centimetre)
                                .PaddingLeft(1f, Unit.Centimetre)
                                .Column(column =>
                                {
                                    column.Item().Text(text =>
                                    {
                                        text.Span("Бренд: ").Bold();
                                        text.Span($"{phone.Brand?.Name}");
                                    });
                                    column.Item().PaddingTop(1f, Unit.Centimetre).Text(text =>
                                    {
                                        text.Span("Модель: ").Bold();
                                        text.Span($"{phone.Title}");
                                    });
                                    column.Item().PaddingTop(1f, Unit.Centimetre).Text(text =>
                                    {
                                        text.Span("Цена: ").Bold();
                                        text.Span($"{phone.Price}$");
                                    });
                                });
                        });
                        column.Item().AlignCenter().Text(text =>
                        {
                            text.Line($"Описание {phone.Brand.Name} {phone.Title}").FontSize(22f).Bold().FontColor(Colors.BlueGrey.Darken1);
                        });
                        column.Item().AlignLeft().Text(text =>
                        {
                            text.ParagraphSpacing(1f, Unit.Centimetre);
                            text.Line(Placeholders.LoremIpsum()).Italic();
                            text.Line(Placeholders.LoremIpsum());
                        });
                    });


            }));
        return document.GeneratePdf();
    }
}