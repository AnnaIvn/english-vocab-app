using EnglishVocabApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

public class FlashcardsController : Controller
{
    private readonly ApplicationDbContext _context;

    public FlashcardsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var words = await _context.Words
            .Include(w => w.Type) // завантажити тип слова
            .OrderBy(r => Guid.NewGuid())
            .Take(20)
            .ToListAsync();

        // Заповнюємо поля NotMapped (Examples)
        foreach (var word in words)
        {
            word.Examples = string.IsNullOrWhiteSpace(word.ExamplesString)
                ? new List<string>()
                : word.ExamplesString.Split('.', StringSplitOptions.RemoveEmptyEntries)
                                     .Select(e => e.Trim())
                                     .ToList();
        }

        return View(words);
    }
}
