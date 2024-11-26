using Microsoft.EntityFrameworkCore;
using ProyectoProgramacionAvanzada.Models;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddDbContext<ProyectoPrograDbContext>(options => options.UseSqlServer
(builder.Configuration.GetConnectionString("ProyectoConnection")));


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ProyectoPrograDbContext>(options => options.UseSqlServer
(builder.Configuration.GetConnectionString("ProyectoConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
