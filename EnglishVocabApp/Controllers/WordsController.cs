using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EnglishVocabApp.Data;
using EnglishVocabApp.Models;
using EnglishVocabApp.ViewModels;  // Importing the ViewModel namespace

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
        public async Task<IActionResult> Index(string sortOrder)
        {
            // Set sort order parameters
            ViewBag.NameSortParam = sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewBag.TypeSortParam = sortOrder == "type_asc" ? "type_desc" : "type_asc";

            // Select only necessary fields and project into WordViewModel
            var wordsQuery = _context.Words
                .Select(w => new WordViewModel
                {
                    Id = w.Id,
                    Name = w.Name,
                    Transcript = w.Transcript,
                    Meaning = w.Meaning,
                    ExamplesString = w.ExamplesString,
                    SynonymsString = w.SynonymsString,
                    AntonymsString = w.AntonymsString,
                    TypeName = w.Type.Name
                });

            // Apply sorting
            wordsQuery = sortOrder switch
            {
                "name_desc" => wordsQuery.OrderByDescending(w => w.Name),
                "name_asc" => wordsQuery.OrderBy(w => w.Name),
                "type_desc" => wordsQuery.OrderByDescending(w => w.TypeName),
                "type_asc" => wordsQuery.OrderBy(w => w.TypeName),
                _ => wordsQuery.OrderBy(w => w.Name) // Default sort by Name
            };

            // Execute the query and return results
            return View(await wordsQuery.ToListAsync());
        }

        // Simpler Index that works, let it be for now, just in case
        //public async Task<IActionResult> Index()
        //{
        //    var words = await _context.Words
        //        .Select(w => new WordViewModel
        //        {
        //            Id = w.Id,
        //            Name = w.Name,
        //            Transcript = w.Transcript,
        //            Meaning = w.Meaning,
        //            ExamplesString = w.ExamplesString,
        //            SynonymsString = w.SynonymsString,
        //            AntonymsString = w.AntonymsString,
        //            TypeName = w.Type.Name
        //        })
        //        .ToListAsync();

        //    return View(words);
        //}


        // GET: Words/Search
        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return View(new List<Word>());
            }

            var words = await _context.Words
                .Where(w => w.Name.StartsWith(query)) // filter by word starting with query
                .ToListAsync();

            return View(words);
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

            var wordVm = new WordViewModel
            {
                Id = word.Id,
                Name = word.Name,
                Transcript = word.Transcript,
                Meaning = word.Meaning,
                Examples = word.ExamplesString?.Split(';').Select(s => s.Trim()).ToList() ?? new List<string>(),
                Synonyms = word.SynonymsString?.Split(',').Select(s => s.Trim()).ToList() ?? new List<string>(),
                Antonyms = word.AntonymsString?.Split(',').Select(a => a.Trim()).ToList() ?? new List<string>(),
                TypeName = word.Type?.Name
            };

            return View(wordVm);
        }

        // GET: Words/Create
        public IActionResult Create()
        {
            ViewBag.WordTypes = _context.Types.Select(t => t.Name).ToList();
            return View(new WordViewModel());
        }

        // POST: Words/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WordViewModel wordVm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.WordTypes = await _context.Types.Select(t => t.Name).ToListAsync();
                return View(wordVm);
            }

            var wordType = await _context.Types.FirstOrDefaultAsync(t => t.Name == wordVm.TypeName);
            if (wordType == null)
            {
                ModelState.AddModelError("TypeName", "Invalid word type.");
                ViewBag.WordTypes = await _context.Types.Select(t => t.Name).ToListAsync();
                return View(wordVm);
            }

            var wordEntity = new Word
            {
                Name = wordVm.Name,
                Transcript = wordVm.Transcript,
                Meaning = wordVm.Meaning,
                ExamplesString = wordVm.ExamplesString,
                SynonymsString = wordVm.SynonymsString,
                AntonymsString = wordVm.AntonymsString,
                TypeId = wordType.Id
            };

            _context.Words.Add(wordEntity);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        // GET: Words/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wordEntity = await _context.Words
                .Include(w => w.Type) 
                .FirstOrDefaultAsync(w => w.Id == id);

            if (wordEntity == null)
            {
                return NotFound();
            }

            var wordVm = new WordViewModel(wordEntity);       // uses constructor written in WordViewModel.cs file

            ViewBag.WordTypes = await _context.Types.Select(t => t.Name).ToListAsync();
            return View(wordVm);
        }

        // POST: Words/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WordViewModel wordVm)
        {
            if (id != wordVm.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.WordTypes = await _context.Types.Select(t => t.Name).ToListAsync();
                return View(wordVm);
            }

            var wordType = await _context.Types.FirstOrDefaultAsync(t => t.Name == wordVm.TypeName);
            if (wordType == null)
            {
                ModelState.AddModelError("TypeName", "Invalid word type.");
                ViewBag.WordTypes = await _context.Types.Select(t => t.Name).ToListAsync();
                return View(wordVm);
            }

            var wordEntity = await _context.Words.FirstOrDefaultAsync(w => w.Id == id);
            if (wordEntity == null)
            {
                return NotFound();
            }

            // Update properties
            wordEntity.Name = wordVm.Name;
            wordEntity.Transcript = wordVm.Transcript;
            wordEntity.Meaning = wordVm.Meaning;
            wordEntity.ExamplesString = wordVm.ExamplesString;
            wordEntity.SynonymsString = wordVm.SynonymsString;
            wordEntity.AntonymsString = wordVm.AntonymsString;
            wordEntity.TypeId = wordType.Id;

            // No need for _context.Words.Update(wordEntity);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }






        // GET: Words/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var word= await _context.Words
                .FirstOrDefaultAsync(m => m.Id == id);

            if (word == null)
            {
                return NotFound();
            }

            var wordVm = new WordViewModel
            {
                Id = word.Id,
                Name = word.Name,
                Transcript = word.Transcript,
                Meaning = word.Meaning,
                Examples = word.ExamplesString?.Split(';').Select(s => s.Trim()).ToList() ?? new List<string>(),
                Synonyms = word.SynonymsString?.Split(',').Select(s => s.Trim()).ToList() ?? new List<string>(),
                Antonyms = word.AntonymsString?.Split(',').Select(a => a.Trim()).ToList() ?? new List<string>(),
                TypeName = word.Type?.Name
            };

            return View(wordVm);
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

        // For search by word name (ByName action)
        [HttpGet]
        public async Task<IActionResult> ByName(string name)
        {
            var words = await _context.Words
                .Where(w => w.Name.Contains(name)) // Adjusted for partial match
                .Include(w => w.Type)
                .Select(w => new
                {
                    id = w.Id,
                    name = w.Name,
                    type = w.Type != null ? w.Type.Name : "",
                    transcript = w.Transcript,
                    meaning = w.Meaning,
                    examplesString = w.ExamplesString ?? "",
                    synonymsString = w.SynonymsString ?? "",
                    antonymsString = w.AntonymsString ?? ""
                })
                .ToListAsync();

            return Json(new { words });
        }
    }
}
