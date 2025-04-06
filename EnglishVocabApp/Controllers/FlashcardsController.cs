using EnglishVocabApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
public class FlashcardsController : Controller
{
    private readonly ApplicationDbContext _context;

    public FlashcardsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var words = await _context.Words.OrderBy(r => Guid.NewGuid()).Take(20).ToListAsync();
        return View(words);
    }
}