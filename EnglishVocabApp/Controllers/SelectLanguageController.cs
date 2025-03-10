using Microsoft.AspNetCore.Mvc;

namespace EnglishVocabApp.Controllers
{
    public class SelectLanguageController : Controller
    {
        public IActionResult Index(string returnUrl = null)
        {
            return View();
        }
    }
}
