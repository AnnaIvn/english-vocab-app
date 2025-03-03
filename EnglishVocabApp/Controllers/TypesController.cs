using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EnglishVocabApp.Data;
using EnglishVocabApp.ViewModels;
using EnglishVocabApp.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EnglishVocabApp.Controllers
{
    public class TypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Type
        public async Task<IActionResult> Index()
        {
            var types = await _context.Types
                //.Include(t => t.Name)              // it is just a string, so no need to use include
                .Select(t => new TypeViewModel(t))
                .ToListAsync();
            return View(types);
        }

        // GET: Type/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var types = await _context.Types
                .FirstOrDefaultAsync(m => m.Id == id);
            if (types == null)
            {
                return NotFound();
            }

            // Convert Type to TypeViewModel
            var typeVm = new TypeViewModel
            {
                Id = types.Id,
                Name = types.Name
            };

            return View(typeVm);
        }

        // GET: Type/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Type/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] TypeViewModel types)
        {
            //if (ModelState.IsValid)
            {
                var typeEntity = new Models.Type
                {
                    Name = types.Name // Convert ViewModel to Entity
                };

                _context.Add(typeEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(types);
        }

        // GET: Type/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var types = await _context.Types.FindAsync(id);
            if (types == null)
            {
                return NotFound();
            }

            var typeVm = new TypeViewModel
            {
                Id = types.Id,
                Name = types.Name
            };

            return View(typeVm);
        }

        // POST: Type/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] TypeViewModel types)
        {
            if (id != types.Id)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            {
                try
                {
                    var typeEntity = await _context.Types.FindAsync(id);
                    if (typeEntity == null)
                    {
                        return NotFound();
                    }

                    typeEntity.Name = types.Name; // Copy properties manually


                    _context.Update(typeEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeViewModelExists(types.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(types);
        }

        // GET: Type/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var types = await _context.Types
                .FirstOrDefaultAsync(m => m.Id == id);
            if (types == null)
            {
                return NotFound();
            }

            var typeVm = new TypeViewModel
            {
                Id = types.Id,
                Name = types.Name
            };

            return View(typeVm);
        }

        // POST: Type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var types = await _context.Types.FindAsync(id);
            if (types != null)
            {
                _context.Types.Remove(types);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TypeViewModelExists(int id)
        {
            return _context.TypeViewModel.Any(e => e.Id == id);
        }
    }
}
