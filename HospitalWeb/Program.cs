using HospitalWeb.Components;
using HospitalData.Models;
using Microsoft.EntityFrameworkCore;
using HospitalData.Services;
using HospitalData.DTOs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<HospitalDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HospitalDB"))
);

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddSingleton<UserSessionService>();

builder.Services.AddScoped<IStaffService, StaffService>();

builder.Services.AddScoped<IDoctorService, DoctorService>();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host"); 

app.Run();
