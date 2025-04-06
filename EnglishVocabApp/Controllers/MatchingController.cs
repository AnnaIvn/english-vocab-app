using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EnglishVocabApp.Controllers
{
    public class MatchingController : Controller
    {
        public IActionResult Index()
        {
            var wordPairs = new Dictionary<string, string>
            {
                { "Apple", "Яблуко" },
                { "Dog", "Собака" },
                { "Car", "Авто" },
                { "Book", "Книга" }
            };

            return View(wordPairs);
        }
    }
}