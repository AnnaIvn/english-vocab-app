using EnglishVocabApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
public class WordOfTheDayController : Controller
{
    private readonly ApplicationDbContext _context;

    public WordOfTheDayController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var today = DateTime.UtcNow.Date;
        var hash = today.GetHashCode();
        var word = await _context.Words.OrderBy(w => w.Id).Skip(Math.Abs(hash % _context.Words.Count())).FirstOrDefaultAsync();
        return View(word);
    }
}