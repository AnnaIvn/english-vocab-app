﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EnglishVocabApp.Data;
using EnglishVocabApp.ViewModels;
using EnglishVocabApp.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using static NuGet.Packaging.PackagingConstants;

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

            var foldersQuery = await _context.Folders
                .Include(f => f.User)
                .Include(f => f.FoldersUsers) // Include saved folder-user relations
                .Where(f => !f.IsPrivate || (userId != null && f.UserId == userId))
            .ToListAsync();

            var folderViewModels = foldersQuery.Select(f => new FolderViewModel
            {
                Id = f.Id,
                Name = f.Name,
                Description = f.Description,
                UserId = f.UserId,
                User = f.User,
                CreatedAt = f.CreatedAt,
                UpdatedAt = f.UpdatedAt,
                IsPrivate = f.IsPrivate,
                FoldersUsers = f.FoldersUsers
            });

            return View(folderViewModels);
        }

        public async Task<IActionResult> MyFolders()
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            if (string.IsNullOrEmpty(currentUserId)) return Unauthorized();

            var userFolders = await _context.Folders
                .Where(f => f.UserId == currentUserId)  // Folders the user created
                .Union( // Include folders the user saved
                    _context.Folders
                        .Where(f => f.FoldersUsers.Any(fu => fu.UserId == currentUserId))
                )
                .Include(f => f.User)
                .ToListAsync();

            var folderViewModels = userFolders.Select(f => new FolderViewModel
            {
                Id = f.Id,
                Name = f.Name,
                Description = f.Description,
                UserId = f.UserId,
                User = f.User,
                CreatedAt = f.CreatedAt,
                UpdatedAt = f.UpdatedAt,
                IsPrivate = f.IsPrivate
            }).ToList();

            return View(folderViewModels);
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

            var folderVm = new FolderViewModel
            {
                Id = folder.Id,
                Name = folder.Name,
                Description = folder.Description,
                UserId = folder.UserId,
                User = folder.User,
                CreatedAt = folder.CreatedAt,
                UpdatedAt = folder.UpdatedAt,
                IsPrivate = folder.IsPrivate,
                FoldersUsers = folder.FoldersUsers
            };

            return PartialView("~/Views/Folders/_Details.cshtml", folderVm);
            //return View(folderVm);
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
        [Authorize]
        public IActionResult Create()
        {
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");
            return PartialView("~/Views/Folders/_Create.cshtml", new FolderViewModel());
            //return View(new FolderViewModel());
        }

        // POST: Folders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,IsPrivate")]FolderViewModel folder)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            if (ModelState.IsValid)             
            {

                var folderEntity = new Folder
                {
                    Name = folder.Name,
                    Description = folder.Description,
                    UserId = userId,
                    User = folder.User,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsPrivate = folder.IsPrivate
                };

                _context.Folders.Add(folderEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", folder.UserId);
            return View(folder);
        }

        // GET: Folders/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var folder = await _context.Folders.Include(f => f.User).FirstOrDefaultAsync(f => f.Id == id);
            if (folder == null)
            {
                return NotFound();
            }
            if (folder.UserId != userId)
            {
                return Forbid();
            }
            var folderVm = new FolderViewModel
            {
                Id = folder.Id,
                Name = folder.Name,
                Description = folder.Description,
                //UserId = folder.UserId,
                //User = folder.User,
                //CreatedAt = folder.CreatedAt,
                //UpdatedAt = folder.UpdatedAt,
                IsPrivate = folder.IsPrivate
            };
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", folder.UserId);        // changed Id to UserName
            return PartialView("~/Views/Folders/_Edit.cshtml", folderVm);
            //return View(folderVm);
        }

        // POST: Folders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FolderViewModel folderVm)
        {
            if (id != folderVm.Id) return NotFound();

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            if (ModelState.IsValid)
            {
                var folder = await _context.Folders.Include(f => f.User).FirstOrDefaultAsync(f => f.Id == id);
                if (folder == null) return NotFound();

                if (folder.UserId != userId)
                {
                    return Forbid();
                }

                folder.Name = folderVm.Name;
                folder.Description = folderVm.Description;
                folder.UpdatedAt = DateTime.UtcNow;
                folder.IsPrivate = folderVm.IsPrivate;

                _context.Update(folder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", folderVm.UserId);
            return View(folderVm);
        }

        // GET: Folders/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var folder = await _context.Folders.Include(f => f.User).FirstOrDefaultAsync(m => m.Id == id);
            if (folder == null)
            {
                return NotFound();
            }
            if (folder.UserId != userId)
            {
                return Forbid();
            }

            var folderVm = new FolderViewModel
            {
                Id = folder.Id,
                Name = folder.Name,
                Description = folder.Description,
                UserId = folder.UserId,
                User = folder.User,
                CreatedAt = folder.CreatedAt,
                UpdatedAt = folder.UpdatedAt,
                IsPrivate = folder.IsPrivate,
                FoldersUsers = folder.FoldersUsers
            };

            //return View(folderVm);
            return PartialView("~/Views/Folders/_Delete.cshtml", folderVm);
        }

        // POST: Folders/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var folder = await _context.Folders.Include(f => f.FoldersUsers).FirstOrDefaultAsync(f => f.Id == id);

            if (folder == null)
            {
                return NotFound();
            }

            if (folder.UserId != userId)
            {
                return Forbid();
            }

            _context.Folders.Remove(folder);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FolderExists(int id)
        {
            return _context.Folders.Any(e => e.Id == id);
        }
        // GET: Folders/Available
        //public async Task<IActionResult> Available()
        //{
        //    IQueryable<Folder> folders;

         //   if (User.Identity.IsAuthenticated)
          //  {
           //     // Отримуємо ідентифікатор залогіненого користувача
           //     var userId = _context.Users
             //       .Where(u => u.UserName == User.Identity.Name)
              //      .Select(u => u.Id)
              //      .FirstOrDefault();

                // Повертаємо публічні папки + папки, створені цим користувачем
           //     folders = _context.Folders
            //        .Where(f => !f.IsPrivate || f.UserId == userId);
            //}
            //else
            //{
                // Якщо користувач не увійшов, повертаємо лише публічні папки
               // folders = _context.Folders
               //     .Where(f => !f.IsPrivate);
            //}

            //return View(await folders.ToListAsync());
        //}
        [Authorize]
        public async Task<IActionResult> SaveFolder(int folderId)
        {
            string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId)) return Unauthorized();

            var folder = await _context.Folders.Include(f => f.FoldersUsers).FirstOrDefaultAsync(f => f.Id == folderId);
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

            var folder = await _context.Folders.Include(f => f.FoldersUsers).FirstOrDefaultAsync(f => f.Id == folderId);
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
