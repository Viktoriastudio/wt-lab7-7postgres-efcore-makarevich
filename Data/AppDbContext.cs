using Microsoft.EntityFrameworkCore;
using wt_lab7_7postgres_efcore_makarevich.Models;

namespace wt_lab7_7postgres_efcore_makarevich.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Начальные данные (пункт 23)
        modelBuilder.Entity<TaskItem>().HasData(
            new TaskItem { Id = 1, Title = "Сбор требований", Project = "CRM для банка", Assignee = "Ольга Смирнова", EstimatedHours = 16, IsCompleted = true, CreatedAt = new DateTime(2026, 1, 1) },
            new TaskItem { Id = 2, Title = "Проектирование БД", Project = "CRM для банка", Assignee = "Иван Петров", EstimatedHours = 24, IsCompleted = true, CreatedAt = new DateTime(2026, 1, 2) },
            new TaskItem { Id = 3, Title = "Разработка API", Project = "CRM для банка", Assignee = "Иван Петров", EstimatedHours = 40, IsCompleted = false, CreatedAt = new DateTime(2026, 1, 3) }
        );
    }
}