using DinkToPdf.Contracts;
using DinkToPdf;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Oblig1.DAL;
using Oblig1.Models;
using Oblig1.Services;
using Serilog;
using Serilog.Events;
using Microsoft.AspNetCore.Hosting;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ItemDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ItemDbContextConnection' was not found");



builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ItemDbContext>(options => {

    options.UseSqlite(
        builder.Configuration["ConnectionStrings:ItemDbContextConnection"]);
        
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddDefaultIdentity<Person>()
    
   .AddRoles<IdentityRole>()
   .AddEntityFrameworkStores<ItemDbContext>();

builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);


builder.Services.AddTransient<Oblig1.Services.Kvittering>();
builder.Services.AddScoped<PersonInterface, PersonRepo>();
builder.Services.AddScoped<eierInterface, EierRepo>();
builder.Services.AddScoped<HusInterface, HusRepo>();
builder.Services.AddScoped<KundeInterface, KundeRepo>();
builder.Services.AddScoped<OrdreInterface, OrdreRepo>();


builder.Services.AddRazorPages();
builder.Services.AddSession();

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyAllowSpecificOrigins", builder =>
    {
        builder.WithOrigins("http://localhost:3000") 
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});


builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));


var loggerConfiguration = new LoggerConfiguration()
    .MinimumLevel.Information() // levels: Trace< Information < Warning < Erorr < Fatal
    .WriteTo.File($"Logs/app_{DateTime.Now:yyyyMMdd_HHmmss}.log");

loggerConfiguration.Filter.ByExcluding(e => e.Properties.TryGetValue("SourceContext", out var value) &&
                            e.Level == LogEventLevel.Information &&
                            e.MessageTemplate.Text.Contains("Executed DbCommand"));

var logger = loggerConfiguration.CreateLogger();
builder.Logging.AddSerilog(logger);

var app = builder.Build();






if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
    DBInit.Seed(app).Wait();
}


app.UseRouting();


app.UseCors("MyAllowSpecificOrigins");



app.UseStaticFiles();

app.UseSession();


app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
    {

        endpoints.MapControllers();
        endpoints.MapRazorPages();

        endpoints.MapDefaultControllerRoute();
    });




app.Run();



