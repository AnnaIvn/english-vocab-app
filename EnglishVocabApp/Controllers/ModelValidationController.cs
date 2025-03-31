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
    }
}
