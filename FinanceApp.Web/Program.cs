using Microsoft.EntityFrameworkCore;
using FinanceApp.Infrastructure.Data;
using FinanceApp.Infrastructure.Services;
using FinanceApp.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Adding the DbContext to the service container with a connection string from appsettings.json
builder.Services.AddDbContext<FinanceDbContext>(
    options => options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection"),b => b.MigrationsAssembly("FinanceApp.Infrastructure")
        )
    );

// Adding the services
builder.Services.AddScoped<AccountService>(); //Accounts service
builder.Services.AddScoped<TransactionService>(); //Transactions service

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<FinanceDbContext>();
    dbContext.Database.Migrate(); // Ensures the creation of the db and also applies any pending migrations
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
