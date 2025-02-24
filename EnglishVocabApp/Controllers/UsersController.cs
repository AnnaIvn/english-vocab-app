using EnglishVocabApp.Data;
using Microsoft.AspNetCore.Mvc;

namespace EnglishVocabApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UsersController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var users = _db.Users.ToList();

            return View(users);
        }
    }
}
