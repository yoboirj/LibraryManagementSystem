using LibraryManagementSystem.Components;
using LibraryManagementSystem.Features.Data;
using LibraryManagementSystem.Features.Helpers;
using LibraryManagementSystem.Features.Helpers.Auth;
using LibraryManagementSystem.Features.Repositories.Implementations;
using LibraryManagementSystem.Features.Repositories.Intefaces;
using LibraryManagementSystem.Features.Services.Implementations;
using LibraryManagementSystem.Features.Services.Intefaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
// Add services
builder.Services.AddControllers(); // or AddRazorPages(), etc.
builder.Services.AddAuthorization(); // Usually added by default, but ensure it's there
builder.Services.AddMudServices();

// Authentication   
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/login";
    });

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<AuthStateProvider>());

//Connection string for the database, you can change it to your own connection string in
builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("LibraryManagement"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("LibraryManagement"))
        ));

// Repository
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IAuthorServices, AuthorServices>();
builder.Services.AddScoped<IBorrowRepository, BorrowRepository>();
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();
// Service
builder.Services.AddScoped<IBookServices, BookServices>();
builder.Services.AddScoped<IBorrowServices, BorrowServices>();
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IMemberServices, MemberServices>();

builder.Services.AddScoped<ThemePalos>();

var app = builder.Build();

// Automatically apply migrations on startup
using (var scope = app.Services.CreateScope())
{
    var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
    using var dbContext = dbContextFactory.CreateDbContext();
    await dbContext.Database.MigrateAsync();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapControllers();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
