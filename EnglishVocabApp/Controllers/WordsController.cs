using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EnglishVocabApp.Data;
using EnglishVocabApp.Models;

namespace EnglishVocabApp.Controllers
{
    public class WordsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WordsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Words
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Words.Include(w => w.Type);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Words/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var word = await _context.Words
                .Include(w => w.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (word == null)
            {
                return NotFound();
            }

            return View(word);
        }

        // GET: Words/Create         for form creation
        public IActionResult Create()
        {
            ViewData["TypeId"] = new SelectList(_context.Types, "Id", "Name");
            return View();
        }

        // POST: Words/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,TypeId,Transcript,Meaning,Examples,Synonyms,Antonyms,Id")] Word word)
        {
            //if (ModelState.IsValid)    // commented for now
            {
                word.ExamplesString = string.Join(". ", word.Examples.Where(s => !string.IsNullOrWhiteSpace(s)));  // convert Examples list to comma-separated ExamplesString before saving
                word.SynonymsString = string.Join(", ", word.Synonyms.Where(s => !string.IsNullOrWhiteSpace(s)));  // convert Synonyms list to comma-separated SynonymsString before saving
                word.AntonymsString = string.Join(", ", word.Antonyms.Where(a => !string.IsNullOrWhiteSpace(a)));  // convert Antonyms list to comma-separated AntonymsString before saving

                _context.Add(word);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TypeId = new SelectList(_context.Types, "Id", "Name", word.TypeId);
            return View(word);
        }

        // GET: Words/Edit/5           for filled form 
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var word = await _context.Words.FindAsync(id);
            if (word == null)
            {
                return NotFound();
            }
            ViewData["TypeId"] = new SelectList(_context.Types, "Id", "Name", word.TypeId);

            word.Examples = word.ExamplesString?.Split('.').Select(s => s.Trim()).ToList() ?? new List<string>();   // convert comma-separated values in SynonymsString into Synonym list
            word.Synonyms = word.SynonymsString?.Split(',').Select(s => s.Trim()).ToList() ?? new List<string>();   // convert comma-separated values in SynonymsString into Synonym list
            word.Antonyms = word.AntonymsString?.Split(',').Select(a => a.Trim()).ToList() ?? new List<string>();   // convert comma-separated values in AntonymsString into Antonym list

            return View(word);
        }

        // POST: Words/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,TypeId,Transcript,Meaning,Examples,Synonyms,Antonyms,Id")] Word word)
        {
            if (id != word.Id)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)               // commented for now
            {
                try
                {
                    word.ExamplesString = string.Join(". ", word.Examples.Where(s => !string.IsNullOrWhiteSpace(s)));  // convert Examples list to comma-separated ExamplesString before saving
                    word.SynonymsString = string.Join(", ", word.Synonyms.Where(s => !string.IsNullOrWhiteSpace(s)));  // convert Synonyms list to comma-separated SynonymsString before saving
                    word.AntonymsString = string.Join(", ", word.Antonyms.Where(a => !string.IsNullOrWhiteSpace(a)));  // convert Antonyms list to comma-separated AntonymsString before saving

                    _context.Update(word);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WordExists(word.Id))
                    //if (!_context.Words.Any(e => e.Id == word.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TypeId"] = new SelectList(_context.Types, "Id", "Id", word.TypeId);
            return View(word);
        }

        // GET: Words/Delete/5       for window creation
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var word = await _context.Words
                .Include(w => w.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (word == null)
            {
                return NotFound();
            }

            return View(word);
        }

        // POST: Words/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var word = await _context.Words.FindAsync(id);
            if (word != null)
            {
                _context.Words.Remove(word);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WordExists(int id)
        {
            return _context.Words.Any(e => e.Id == id);
        }
    }
}
