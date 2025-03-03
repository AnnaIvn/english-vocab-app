using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EnglishVocabApp.Data;
using EnglishVocabApp.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace EnglishVocabApp.Controllers
{
    public class FoldersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FoldersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Folders
        public async Task<IActionResult> Index()
        {
            var userId = (User.Identity != null && User.Identity.IsAuthenticated)
                ? User.FindFirstValue(ClaimTypes.NameIdentifier)
                : null;

            var foldersQuery = _context.Folders
                .Include(f => f.User)
                .Include(f => f.FoldersUsers) // Include saved folder-user relations
                .Where(f => !f.IsPrivate || (userId != null && f.UserId == userId));

            return View(await foldersQuery.ToListAsync());
        }

        public async Task<IActionResult> MyFolders()
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty; ;
            if (string.IsNullOrEmpty(currentUserId)) return Unauthorized();

            var userFolders = await _context.Folders
                .Where(f => f.UserId == currentUserId)  // Folders the user created
                .Union( // Include folders the user saved
                    _context.Folders
                        .Where(f => f.FoldersUsers.Any(fu => fu.UserId == currentUserId))
                )
                .Include(f => f.User)
                .ToListAsync();

            return View(userFolders);
        }

        // GET: Folders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var folder = await _context.Folders
                .Include(f => f.User)
                .Include(f => f.FoldersUsers) // Include saved folder-user relations
                .FirstOrDefaultAsync(m => m.Id == id);

            if (folder == null) return NotFound();

            return View(folder);
        }
                // GET: Folders/UserFolders
        //public async Task<IActionResult> UserFolders()
        //{
        //    string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    if (string.IsNullOrEmpty(currentUserId)) return Unauthorized();

        //    var userFolders = await _context.Folders
        //        .Where(f => f.UserId == currentUserId)  // Власні папки
        //       .Union( // Папки, які користувач зберіг
        //            _context.Folders
        //                .Where(f => f.FoldersUsers.Any(fu => fu.UserId == currentUserId))
        //        )
        //        .Include(f => f.User)
        //        .ToListAsync();

        //    return View(userFolders);
        //}
        // GET: Folders/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");        // changed Id to UserName
            return View();
        }

        // POST: Folders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,UserId,CreatedAt,UpdatedAt,IsPrivate")] Folder folder)
        {
            //if (ModelState.IsValid)              // commented for now
            {
                _context.Add(folder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", folder.UserId);
            return View(folder);
        }

        // GET: Folders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var folder = await _context.Folders.FindAsync(id);
            if (folder == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", folder.UserId);        // changed Id to UserName
            return View(folder);
        }

        // POST: Folders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,UserId,CreatedAt,UpdatedAt,IsPrivate")] Folder folder)
        {
            if (id != folder.Id)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)           // commented for now
            {
                try
                {
                    _context.Update(folder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FolderExists(folder.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", folder.UserId);
            return View(folder);
        }

        // GET: Folders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var folder = await _context.Folders
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (folder == null)
            {
                return NotFound();
            }

            return View(folder);
        }

        // POST: Folders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var folder = await _context.Folders.FindAsync(id);
            if (folder != null)
            {
                _context.Folders.Remove(folder);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FolderExists(int id)
        {
            return _context.Folders.Any(e => e.Id == id);
        }

        [Authorize]
        public async Task<IActionResult> SaveFolder(int folderId)
        {
            string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId)) return Unauthorized();

            var folder = await _context.Folders.FindAsync(folderId);
            if (folder == null || folder.IsPrivate) return NotFound(); // Only public folders

            if (folder.UserId == currentUserId) return BadRequest("You already own this folder."); // Prevent self-saving

            bool alreadySaved = await _context.FoldersUsers
                .AnyAsync(fu => fu.UserId == currentUserId && fu.FolderId == folderId);

            if (!alreadySaved)
            {
                var folderUser = new FoldersUsers
                {
                    UserId = currentUserId,
                    FolderId = folderId
                };
                _context.FoldersUsers.Add(folderUser);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(MyFolders));
        }

        [Authorize]
        public async Task<IActionResult> RemoveFolder(int folderId)
        {
            string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId)) return Unauthorized();

            var folder = await _context.Folders.FindAsync(folderId);
            if (folder == null) return NotFound();

            if (folder.UserId == currentUserId)
            {
                // If user owns the folder, delete it instead
                _context.Folders.Remove(folder);
            }
            else
            {
                // If user does not own the folder, remove it from the list 
                var folderUser = await _context.FoldersUsers
                    .FirstOrDefaultAsync(fu => fu.UserId == currentUserId && fu.FolderId == folderId);

                if (folderUser != null)
                {
                    _context.FoldersUsers.Remove(folderUser);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MyFolders)); 
        }
    }
}
