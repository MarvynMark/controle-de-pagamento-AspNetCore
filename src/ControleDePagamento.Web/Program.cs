using ControleDePagamento.Aplication.Interfaces;
using ControleDePagamento.Aplication.Services;
using ControleDePagamento.Domain.Interfaces;
using ControleDePagamento.Domain.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IImportadorDeDadosServices, ImportadorDeDadosServices> ();
builder.Services.AddScoped<IFechamentoDePontoDepartamento, FechamentoDePontoDepartamento>();
builder.Services.AddScoped<IFechamentoDePontoFuncionario, FechamentoDePontoFuncionario>();
builder.Services.AddScoped<IExportadorDeDadosServices, ExportadorDeDadosServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ImportacaoArquivos}/{action=Index}/{id?}");

app.Run();
