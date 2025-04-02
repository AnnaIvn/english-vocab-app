using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EnglishVocabApp.Data;
using EnglishVocabApp.Models;
using EnglishVocabApp.ViewModels;
using System.Security.Claims;  // Importing the ViewModel namespace

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
        public async Task<IActionResult> Index(string sortOrder, int? pageIndex, int? pageSize)
        {
            // Set sort order parameters
            ViewBag.NameSortParam = sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewBag.TypeSortParam = sortOrder == "type_asc" ? "type_desc" : "type_asc";

            // Select only necessary fields and project into WordViewModel
            var wordsQuery = _context.Words.AsQueryable();

            // Apply sorting
            wordsQuery = sortOrder switch
            {
                "name_desc" => wordsQuery.OrderByDescending(w => w.Name),
                "name_asc" => wordsQuery.OrderBy(w => w.Name),
                "type_desc" => wordsQuery.OrderByDescending(w => w.Type.Name),
                "type_asc" => wordsQuery.OrderBy(w => w.Type.Name),
                _ => wordsQuery.OrderBy(w => w.Name) // Default sort by Name
            };

            var paginatedWordList = await PaginateList<Word, WordViewModel>.CreateAsync(
                wordsQuery,
                w => new WordViewModel
                {
                    Id = w.Id,
                    Name = w.Name,
                    Transcript = w.Transcript,
                    Meaning = w.Meaning,
                    ExamplesString = w.ExamplesString,
                    SynonymsString = w.SynonymsString,
                    AntonymsString = w.AntonymsString,
                    TypeName = w.Type.Name
                },
                pageIndex ?? 1,
                pageSize ?? 5
                );

            // Execute the query and return results
            return View(paginatedWordList);
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

            //return View(wordVm);
            return PartialView("~/Views/Words/_Details.cshtml", wordVm);
        }

        public IActionResult GetWordTypes()
        {
            var wordTypes = _context.Types.Select(t => t.Name).ToList();
            return Json(wordTypes);
        }


        // GET: Words/Create
        public IActionResult Create()
        {
            //ViewBag.WordTypes = _context.Types.Select(t => t.Name).ToList();
            //return View(new WordViewModel());
            return PartialView("~/Views/Words/_Create.cshtml", new WordViewModel());
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
            //return View(wordVm);
            return PartialView("~/Views/Words/_Edit.cshtml", wordVm);
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

            // Validate ModelState before querying database
            if (!ModelState.IsValid)
            {
                ViewBag.WordTypes = await _context.Types.Select(t => t.Name).ToListAsync();
                return View(wordVm);
            }

            // Check uniqueness of Meaning, excluding the current word
            var existingMeaning = await _context.Words
                .AnyAsync(w => w.Meaning == wordVm.Meaning && w.Id != id);

            if (existingMeaning)
            {
                ModelState.AddModelError("Meaning", "This meaning already exists.");
                ViewBag.WordTypes = await _context.Types.Select(t => t.Name).ToListAsync();
                return View(wordVm);
            }

            // Validate TypeName exists
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

            // Update word properties
            wordEntity.Name = wordVm.Name;
            wordEntity.Transcript = wordVm.Transcript;
            wordEntity.Meaning = wordVm.Meaning;
            wordEntity.ExamplesString = wordVm.ExamplesString;
            wordEntity.SynonymsString = wordVm.SynonymsString;
            wordEntity.AntonymsString = wordVm.AntonymsString;
            wordEntity.TypeId = wordType.Id;

            // Save changes
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

            //return View(wordVm);
            return PartialView("~/Views/Words/_Delete.cshtml", wordVm);
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

        //public async Task<IActionResult> Save(int wordId, string wordName)
        //{
        //    string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        //    if (string.IsNullOrEmpty(currentUserId)) return Unauthorized();

        //    var userFolders = await _context.Folders
        //        .Where(f => f.UserId == currentUserId)  // Folders the user created
        //        .Union( // Include folders the user saved
        //            _context.Folders
        //                .Where(f => f.FoldersUsers.Any(fu => fu.UserId == currentUserId))
        //        )
        //        .Include(f => f.User)
        //        .Select(f => new FolderViewModel
        //        {
        //            Id = f.Id,
        //            Name = f.Name,
        //            Description = f.Description,
        //            UserId = f.UserId,
        //            User = f.User,
        //            IsPrivate = f.IsPrivate
        //        })
        //        .ToListAsync();

        //    ViewBag.WordId = wordId;
        //    ViewBag.WordName = wordName;

        //    return View(userFolders);
        //}

        public async Task<IActionResult> Save(int wordId, string wordName)
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            if (string.IsNullOrEmpty(currentUserId)) return Unauthorized();

            var userFolders = await _context.Folders
                .Where(f => f.UserId == currentUserId)  // Folders the user created
                .Union( // Include folders the user saved
                    _context.Folders
                        .Where(f => f.FoldersUsers.Any(fu => fu.UserId == currentUserId))
                )
                .Include(f => f.User)
                .Select(f => new FolderViewModel
                {
                    Id = f.Id,
                    Name = f.Name,
                    Description = f.Description,
                    UserId = f.UserId,
                    User = f.User,
                    IsPrivate = f.IsPrivate
                })
                .ToListAsync();

            var savedFolderIds = await _context.WordsFolders
                .Where(wf => wf.WordId == wordId)
                .Select(wf => wf.FolderId)
                .ToListAsync();

            ViewBag.WordId = wordId;
            ViewBag.WordName = wordName;
            ViewBag.SavedFolderIds = savedFolderIds;

            return View(userFolders);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveWordToFolders(int wordId, List<int> folderIds)
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            if (string.IsNullOrEmpty(currentUserId)) return Unauthorized();

            var word = await _context.Words.FindAsync(wordId);
            if (word == null) return NotFound();

            var existingFolders = await _context.WordsFolders
                .Where(wf => wf.WordId == wordId)
                .ToListAsync();

            // Remove existing folder associations that are not in the new list
            _context.WordsFolders.RemoveRange(existingFolders.Where(ef => !folderIds.Contains(ef.FolderId)));

            // Add new folder associations
            foreach (var folderId in folderIds)
            {
                if (!existingFolders.Any(ef => ef.FolderId == folderId))
                {
                    _context.WordsFolders.Add(new WordsFolders
                    {
                        WordId = wordId,
                        FolderId = folderId
                    });
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }





    }
}
