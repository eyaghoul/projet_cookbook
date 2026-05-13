using CookBook.Models;
using CookBook.Services.Implementations;
using CookBook.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ── Razor / Blazor ────────────────────────────────────────────────────────────
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// ── EF Core SQLite ────────────────────────────────────────────────────────────
builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? "Data Source=app.db"));

// ── Application services ──────────────────────────────────────────────────────
builder.Services.AddScoped<IRecipeService,     RecipeService>();
builder.Services.AddScoped<IIngredientService, IngredientService>();
builder.Services.AddScoped<ICategoryService,   CategoryService>();
builder.Services.AddScoped<ITypeCuisineService, TypeCuisineService>();
builder.Services.AddScoped<IUnitService,        UnitService>();

var app = builder.Build();

// ── Auto-apply migrations on startup ─────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// ── Middleware ────────────────────────────────────────────────────────────────
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<CookBook.Components.App>()
    .AddInteractiveServerRenderMode();

app.Run();