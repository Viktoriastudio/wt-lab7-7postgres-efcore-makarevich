using Microsoft.EntityFrameworkCore;
using wt_lab7_7postgres_efcore_makarevich.Data;
using wt_lab7_7postgres_efcore_makarevich.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();

// --- Настройка подключения к БД (пункт 26) ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var databaseUrl = builder.Configuration["DATABASE_URL"];

// Если мы на Render (передана переменная DATABASE_URL), парсим её
if (!string.IsNullOrWhiteSpace(databaseUrl))
{
    var uri = new Uri(databaseUrl);
    var userInfo = uri.UserInfo.Split(':', 2);
    
    // Если порт не указан в URL (как во внутренних ссылках Render), ставим стандартный 5432
    var dbPort = uri.Port == -1 ? 5432 : uri.Port; 
    var dbName = uri.AbsolutePath.TrimStart('/');

    connectionString = $"Host={uri.Host};Port={dbPort};Database={dbName};Username={Uri.UnescapeDataString(userInfo[0])};Password={Uri.UnescapeDataString(userInfo[1])};SSL Mode=Disable";
    
    // Выведем в логи (без пароля), чтобы видеть, что происходит
    Console.WriteLine($"Connecting to DB at {uri.Host} on port {dbPort}");
}

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// --- Автоматическое применение миграций (пункт 12) ---
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate(); 
}

app.MapGet("/", () => "Minimal API работает с PostgreSQL!");

app.MapGet("/api/info", () => new { Application = "Web Technologies", Version = "2.0 (EF Core)" });

// --- CRUD с EF Core (пункт 13-18) ---
app.MapGet("/api/tasks", async (AppDbContext db) => await db.Tasks.ToListAsync());

app.MapGet("/api/tasks/{id}", async (int id, AppDbContext db) =>
{
    var task = await db.Tasks.FirstOrDefaultAsync(x => x.Id == id);
    return task == null ? Results.NotFound() : Results.Ok(task);
});

app.MapGet("/api/tasks/search", async (string? search, AppDbContext db) =>
{
    var query = db.Tasks.AsQueryable();
    if (!string.IsNullOrWhiteSpace(search))
        query = query.Where(x => x.Title.ToLower().Contains(search.ToLower()));
    return Results.Ok(await query.ToListAsync());
});

// LINQ-запросы (пункт 19)
app.MapGet("/api/tasks/filter", async (AppDbContext db) =>
    Results.Ok(await db.Tasks.Where(x => x.EstimatedHours > 20).OrderBy(x => x.Title).ToListAsync()));

app.MapPost("/api/tasks", async (TaskItem task, AppDbContext db) =>
{
    db.Tasks.Add(task);
    await db.SaveChangesAsync();
    return Results.Created($"/api/tasks/{task.Id}", task);
});

app.MapPut("/api/tasks/{id}", async (int id, TaskItem updatedTask, AppDbContext db) =>
{
    var task = await db.Tasks.FirstOrDefaultAsync(x => x.Id == id);
    if (task == null) return Results.NotFound();

    task.Title = updatedTask.Title;
    task.Project = updatedTask.Project;
    task.Assignee = updatedTask.Assignee;
    task.EstimatedHours = updatedTask.EstimatedHours;
    task.IsCompleted = updatedTask.IsCompleted;
    
    await db.SaveChangesAsync();
    return Results.Ok(task);
});

app.MapDelete("/api/tasks/{id}", async (int id, AppDbContext db) =>
{
    var task = await db.Tasks.FirstOrDefaultAsync(x => x.Id == id);
    if (task == null) return Results.NotFound();

    db.Tasks.Remove(task);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Run($"http://0.0.0.0:{port}");