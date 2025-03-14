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
    }
}
