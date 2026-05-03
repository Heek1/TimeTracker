namespace TimeTracker.Models;

public class StatisticsViewModel
{
    public int TodayTotalMinutes { get; set; }
    public int WeekTotalMinutes { get; set; }
    public int AllTimeTotalMinutes { get; set; }
    public Dictionary<string, int> TodayByCategory { get; set; } = new();
    public Dictionary<string, int> WeekByCategory { get; set; } = new();
    public Dictionary<string, int> AllTimeByCategory { get; set; } = new();
    public List<TimeEntry> RecentEntries { get; set; } = new();
}
