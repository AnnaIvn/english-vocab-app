using EnglishVocabApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
public class QuizController : Controller
{
    private readonly ApplicationDbContext _context;

    public QuizController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var words = await _context.Words.OrderBy(r => Guid.NewGuid()).Take(5).ToListAsync();
        return View(words);
    }
}