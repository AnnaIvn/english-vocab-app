using System.Text.RegularExpressions;
using EnglishVocabApp.Data;
using Microsoft.AspNetCore.Mvc;

namespace EnglishVocabApp.Controllers
{
    public class ModelValidationController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ModelValidationController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult CheckNewTypeName(string name)
        {
            var result = _context.Types.Any(t => t.Name == name);   
            return Json(!result);
        }

        public IActionResult CheckNewMeaning(string meaning)
        {
            var result = _context.Words.Any(w => w.Meaning == meaning);
            return Json(!result);
        }

        public IActionResult CheckNewDescription(string description)
        {
            var result = _context.Folders.Any(f => f.Description == description);
            return Json(!result);
        }

        public IActionResult Validate(List<string> examples)
        {
            if (examples == null || examples.Count == 0) return Json(true);

            // Check for duplicates
            bool hasDuplicates = examples.GroupBy(e => e).Any(g => g.Count() > 1);

            // Check for valid characters (letters, numbers, punctuation)
            string pattern = @"^[a-zA-Z0-9\s.,!?'"":;_-]+$";
            bool hasInvalidCharacters = examples.Any(e => !Regex.IsMatch(e, pattern));

            if (hasDuplicates)
                return Json("lblExamplesMustBeUnique");

            if (hasInvalidCharacters)
                return Json("Only English letters, numbers, and common punctuation are allowed.");

            return Json(true);
        }

    }
}
