using Microsoft.EntityFrameworkCore;
using Oblig1.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ItemDbContext>(options => {
    options.UseSqlite(
        builder.Configuration["ConnectionStrings:ItemDbContextConnection"]);
});


var app = builder.Build();


app.UseStaticFiles();
 

app.Run();


