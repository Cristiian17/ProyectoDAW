using Blazored.LocalStorage;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Blazorise.RichTextEdit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Proyecto_Front_MuresanCristian.Services;
using Proyecto_Front_MuresanCristian.Services.Interfaces;
using Syncfusion.Blazor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IRestService, RestService>();
builder.Services.AddSingleton<HttpClientHandler>();
builder.Services.AddScoped<DialogService>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddSyncfusionBlazor();
builder.Services.AddBlazoriseRichTextEdit();
builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrapProviders()
    .AddFontAwesomeIcons();


var app = builder.Build();

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("@32312e312e3335lTIYqGAzjr4p9q4p/uSpFlV/c0OdQuY6Ny2cJvmcOfs=\r\n");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
