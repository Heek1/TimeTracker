using Microsoft.EntityFrameworkCore;
using TimeTracker.Models;

namespace TimeTracker.Data;

public class TimeTrackerContext : DbContext
{
    public TimeTrackerContext(DbContextOptions<TimeTrackerContext> options) : base(options) { }

    public DbSet<TimeEntry> TimeEntries => Set<TimeEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        var now = DateTime.Now;
        var entries = new List<TimeEntry>
        {
            new() { Id = 1, TaskName = "Читання документації ASP.NET", Category = "Навчання", StartTime = now.Date.AddHours(8), DurationMinutes = 90, Notes = "Вивчав middleware та routing" },
            new() { Id = 2, TaskName = "Ранкова пробіжка", Category = "Спорт", StartTime = now.Date.AddHours(7), DurationMinutes = 45, Notes = "5 км по парку" },
            new() { Id = 3, TaskName = "Написання звіту", Category = "Робота", StartTime = now.Date.AddHours(10), DurationMinutes = 120, Notes = "Квартальний звіт" },
            new() { Id = 4, TaskName = "Обід та відпочинок", Category = "Відпочинок", StartTime = now.Date.AddHours(13), DurationMinutes = 60, Notes = null },

            new() { Id = 5, TaskName = "Entity Framework Core", Category = "Навчання", StartTime = now.Date.AddDays(-1).AddHours(9), DurationMinutes = 150, Notes = "Міграції та LINQ запити" },
            new() { Id = 6, TaskName = "Код-рев'ю", Category = "Робота", StartTime = now.Date.AddDays(-1).AddHours(14), DurationMinutes = 75, Notes = "Pull request від команди" },
            new() { Id = 7, TaskName = "Йога", Category = "Спорт", StartTime = now.Date.AddDays(-1).AddHours(7).AddMinutes(30), DurationMinutes = 40, Notes = "Ранкова практика" },

            new() { Id = 8, TaskName = "Зустріч з командою", Category = "Робота", StartTime = now.Date.AddDays(-2).AddHours(10), DurationMinutes = 90, Notes = "Sprint planning" },
            new() { Id = 9, TaskName = "Читання книги", Category = "Відпочинок", StartTime = now.Date.AddDays(-2).AddHours(20), DurationMinutes = 60, Notes = "Clean Code" },
            new() { Id = 10, TaskName = "C# advanced patterns", Category = "Навчання", StartTime = now.Date.AddDays(-2).AddHours(16), DurationMinutes = 120, Notes = "SOLID принципи" },

            new() { Id = 11, TaskName = "Плавання", Category = "Спорт", StartTime = now.Date.AddDays(-3).AddHours(7), DurationMinutes = 60, Notes = "Басейн, 2 км" },
            new() { Id = 12, TaskName = "Розробка API", Category = "Робота", StartTime = now.Date.AddDays(-3).AddHours(11), DurationMinutes = 180, Notes = "REST endpoints для нового модуля" },

            new() { Id = 13, TaskName = "Docker та контейнери", Category = "Навчання", StartTime = now.Date.AddDays(-5).AddHours(10), DurationMinutes = 100, Notes = "Docker compose, networking" },
            new() { Id = 14, TaskName = "Прогулянка в парку", Category = "Відпочинок", StartTime = now.Date.AddDays(-5).AddHours(18), DurationMinutes = 50, Notes = "Вечірня прогулянка" },

            new() { Id = 15, TaskName = "Фіксація багів", Category = "Робота", StartTime = now.Date.AddDays(-6).AddHours(9), DurationMinutes = 150, Notes = "Критичні баги з production" },
        };

        modelBuilder.Entity<TimeEntry>().HasData(entries);
    }
}
