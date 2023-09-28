using System.Globalization;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using PhoneStore.Context;
using PhoneStore.Services;
using PhoneStore.Validations;
using PhoneStore.ViewModels;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);


string connection = builder.Configuration.GetConnectionString("PostgresConnection");
builder.Services.AddDbContext<MobileContext>(options => options.UseNpgsql(connection));
builder.Services.AddTransient<FileUploadService>();
builder.Services.AddValidatorsFromAssemblyContaining<AddPhoneValidator>();
ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("ru");

QuestPDF.Settings.License = LicenseType.Community;
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithRedirects("/Error/{0}");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();