using EnglishVocabApp.Data;
using EnglishVocabApp.Models;
using EnglishVocabApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EnglishVocabApp.Controllers
{
    public class MultipleChoiceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MultipleChoiceController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> SelectFolder()
        {
            var folders = await _context.Folders.ToListAsync();
            return View(folders);
        }

        public async Task<IActionResult> Quiz(int folderId)
        {
            var wordIds = await _context.WordsFolders
                .Where(wf => wf.FolderId == folderId)
                .Select(wf => wf.WordId)
                .ToListAsync();

            var words = await _context.Words
                .Where(w => wordIds.Contains(w.Id))
                .ToListAsync();

            if (!words.Any())
                return RedirectToAction("SelectFolder");

            var random = new Random();
            var currentWord = words[random.Next(words.Count)];

            var options = words
                .Where(w => w.Id != currentWord.Id)
                .OrderBy(x => random.Next())
                .Take(3)
                .Select(w => w.Meaning) // зміна тут
                .ToList();

            options.Add(currentWord.Meaning); // зміна тут
            options = options.OrderBy(x => random.Next()).ToList();

            var model = new MultipleChoiceViewModel
            {
                FolderId = folderId,
                WordId = currentWord.Id,
                WordText = currentWord.Name,
                Options = options,
                CorrectAnswer = currentWord.Meaning // зміна тут
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult CheckAnswer(int folderId, int wordId, string selectedAnswer, string correctAnswer)
        {
            ViewBag.IsCorrect = selectedAnswer == correctAnswer;
            ViewBag.CorrectAnswer = correctAnswer;
            ViewBag.FolderId = folderId;

            return View("AnswerResult");
        }

        public IActionResult NextQuestion(int folderId)
        {
            return RedirectToAction("Quiz", new { folderId });
        }
    }
}