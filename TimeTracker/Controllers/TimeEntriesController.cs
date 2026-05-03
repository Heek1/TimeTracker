using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTracker.Data;
using TimeTracker.Models;

namespace TimeTracker.Controllers;

public class TimeEntriesController : Controller
{
    private readonly TimeTrackerContext _context;

    public TimeEntriesController(TimeTrackerContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? category, string period = "today")
    {
        var today = DateTime.Today;
        var weekStart = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);

        var query = _context.TimeEntries.AsQueryable();

        query = period switch
        {
            "week" => query.Where(e => e.StartTime >= weekStart),
            "all"  => query,
            _      => query.Where(e => e.StartTime.Date == today)
        };

        if (!string.IsNullOrEmpty(category))
            query = query.Where(e => e.Category == category);

        var entries = await query.OrderByDescending(e => e.StartTime).ToListAsync();

        ViewBag.SelectedCategory = category;
        ViewBag.SelectedPeriod = period;
        ViewBag.Categories = TimeEntry.Categories;
        ViewBag.TodayTotal = await _context.TimeEntries
            .Where(e => e.StartTime.Date == today)
            .SumAsync(e => e.DurationMinutes);

        return View(entries);
    }

    public IActionResult Create()
    {
        ViewBag.Categories = TimeEntry.Categories;
        return View(new TimeEntry { StartTime = DateTime.Now });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TimeEntry entry)
    {
        if (ModelState.IsValid)
        {
            _context.TimeEntries.Add(entry);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Запис «{entry.TaskName}» додано!";
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Categories = TimeEntry.Categories;
        return View(entry);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var entry = await _context.TimeEntries.FindAsync(id);
        if (entry == null) return NotFound();
        ViewBag.Categories = TimeEntry.Categories;
        return View(entry);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TimeEntry entry)
    {
        if (id != entry.Id) return NotFound();
        if (ModelState.IsValid)
        {
            _context.Update(entry);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Запис оновлено!";
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Categories = TimeEntry.Categories;
        return View(entry);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var entry = await _context.TimeEntries.FindAsync(id);
        if (entry != null)
        {
            _context.TimeEntries.Remove(entry);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Запис видалено!";
        }
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Statistics()
    {
        var today = DateTime.Today;
        var weekStart = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);

        var allEntries = await _context.TimeEntries.ToListAsync();

        var todayEntries = allEntries.Where(e => e.StartTime.Date == today).ToList();
        var weekEntries  = allEntries.Where(e => e.StartTime >= weekStart).ToList();

        var vm = new StatisticsViewModel
        {
            TodayTotalMinutes   = todayEntries.Sum(e => e.DurationMinutes),
            WeekTotalMinutes    = weekEntries.Sum(e => e.DurationMinutes),
            AllTimeTotalMinutes = allEntries.Sum(e => e.DurationMinutes),

            TodayByCategory = TimeEntry.Categories
                .ToDictionary(c => c, c => todayEntries.Where(e => e.Category == c).Sum(e => e.DurationMinutes)),

            WeekByCategory = TimeEntry.Categories
                .ToDictionary(c => c, c => weekEntries.Where(e => e.Category == c).Sum(e => e.DurationMinutes)),

            AllTimeByCategory = TimeEntry.Categories
                .ToDictionary(c => c, c => allEntries.Where(e => e.Category == c).Sum(e => e.DurationMinutes)),

            RecentEntries = allEntries.OrderByDescending(e => e.StartTime).Take(5).ToList()
        };

        return View(vm);
    }
}
