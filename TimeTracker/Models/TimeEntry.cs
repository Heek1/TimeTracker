using System.ComponentModel.DataAnnotations;

namespace TimeTracker.Models;

public class TimeEntry
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Назва завдання обов'язкова")]
    [StringLength(200)]
    [Display(Name = "Назва завдання")]
    public string TaskName { get; set; } = "";

    [Required(ErrorMessage = "Категорія обов'язкова")]
    [Display(Name = "Категорія")]
    public string Category { get; set; } = "Інше";

    [Display(Name = "Час початку")]
    public DateTime StartTime { get; set; } = DateTime.Now;

    [Required]
    [Range(1, 1440, ErrorMessage = "Тривалість від 1 до 1440 хвилин")]
    [Display(Name = "Тривалість (хв)")]
    public int DurationMinutes { get; set; }

    [Display(Name = "Нотатки")]
    [StringLength(1000)]
    public string? Notes { get; set; }

    public static readonly string[] Categories =
        ["Навчання", "Робота", "Відпочинок", "Спорт", "Інше"];
}
