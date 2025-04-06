using EnglishVocabApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using EnglishVocabApp.Data;
using EnglishVocabApp.Models; 

public class ListeningController : Controller
{
    private readonly ApplicationDbContext _context;

    public ListeningController(ApplicationDbContext context)
    {
        _context = context;
    }
    [HttpGet]
    public async Task<JsonResult> Next()
    {
        var word = await _context.Words.OrderBy(w => Guid.NewGuid()).FirstOrDefaultAsync();
        return Json(word);
    }
    public async Task<IActionResult> Index()
    {
        var word = await _context.Words.OrderBy(w => Guid.NewGuid()).FirstOrDefaultAsync();
        return View(word);
    }
}