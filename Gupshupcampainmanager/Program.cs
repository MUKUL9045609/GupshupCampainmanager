using Gupshupcampainmanager;
using Gupshupcampainmanager.Models;
using Gupshupcampainmanager.Persistence;
using Gupshupcampainmanager.Repository.Interface;
using Gupshupcampainmanager.Service;
using Microsoft.AspNetCore.Authorization;
using NLog.Extensions.Logging;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);
var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true; 
    options.Cookie.IsEssential = true; 
});

builder.Services.AddAuthentication("Cookie")
    .AddCookie("Cookie", options =>
    {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        //options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
        options.Cookie.IsEssential = true;
        options.Cookie.HttpOnly = true;
        options.SlidingExpiration = false;

    });

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddNLog();
});


builder.Services.AddAuthorization(options =>
{
    var userAuthPolicyBuilder = new AuthorizationPolicyBuilder("Cookie");
    options.DefaultPolicy = userAuthPolicyBuilder
                        .RequireAuthenticatedUser()
                        .Build();
});

// Add services to the container.
builder.Services.AddControllersWithViews();
//builder.Services.AddHttpClient<GupshupApiService>();
builder.Services.AddTransient<IGupshupApiService, GupshupApiService>();
builder.Services.AddTransient<ICampaignRepository, CampaignRepository>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddSingleton<DbContext>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
//builder.Services.AddLogging();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyAllowSpecificOrigins",
    builder =>
    {
        builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddSession();


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
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.UseSession();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}")
    .WithStaticAssets();


app.Run();
