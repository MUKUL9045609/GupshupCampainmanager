using Gupshupcampainmanager;
using Gupshupcampainmanager.Models;
using Gupshupcampainmanager.Persistence;
using Gupshupcampainmanager.Repository.Interface;
using Gupshupcampainmanager.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<GupshupApiService>();
builder.Services.AddTransient<IGupshupApiService, GupshupApiService>();
builder.Services.AddTransient<ICampaignRepository, CampaignRepository>();
builder.Services.AddSingleton<DbContext>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyAllowSpecificOrigins",
    builder =>
    {
        builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddControllers();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseStaticFiles();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Gupshup}/{action=SavecampaignTemplate}")
    .WithStaticAssets();


app.Run();
