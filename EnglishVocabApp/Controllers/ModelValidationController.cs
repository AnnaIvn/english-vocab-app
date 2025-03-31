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

        public IActionResult CheckDuplicateExamples(List<string> examples)
        {
            if (examples == null || examples.Count == 0) return Json(true);

            bool hasDuplicates = examples.GroupBy(e => e)
                                         .Any(g => g.Count() > 1);

            return Json(!hasDuplicates);
        }

        public IActionResult CheckDuplicateSynonyms(List<string> synonyms)
        {
            if (synonyms == null || synonyms.Count == 0) return Json(true);

            bool hasDuplicates = synonyms.GroupBy(e => e)
                                         .Any(g => g.Count() > 1);

            return Json(!hasDuplicates);
        }

        public IActionResult CheckDuplicateAntonyms(List<string> antonyms)
        {
            if (antonyms == null || antonyms.Count == 0) return Json(true);

            bool hasDuplicates = antonyms.GroupBy(e => e)
                                         .Any(g => g.Count() > 1);

            return Json(!hasDuplicates);
        }
    }
}
