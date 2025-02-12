using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Super_Simple_Homebrew_Hoster.Data;
using Super_Simple_Homebrew_Hoster.Models;

namespace Super_Simple_Homebrew_Hoster.Controllers
{
    public class HomebrewItemsController : Controller
    {
        private readonly Super_Simple_Homebrew_HosterContext _context;

        public HomebrewItemsController(Super_Simple_Homebrew_HosterContext context)
        {
            _context = context;
        }

        // GET: HomebrewItems
        public async Task<IActionResult> Index(string itemSystem, string titleSearch)
        {
            if (_context.HomebrewItem == null)
            {
                return Problem("Entity set 'Super_Simple_Homebrew_Hoster.HomebrewItem'  is null.");
            }

            // Use LINQ to get a list of available systems.
            IQueryable<string> systemQuery = from i in _context.HomebrewItem
                                             orderby i.System
                                             select i.System;

            var homebrewItems = from m in _context.HomebrewItem
                         select m;

            if (!String.IsNullOrEmpty(titleSearch))
            {
                homebrewItems = homebrewItems.Where(s => s.Title!.ToUpper().Contains(titleSearch.ToUpper()));
            }
            if (!String.IsNullOrEmpty(itemSystem))
            {
                homebrewItems = homebrewItems.Where(x => x.System == itemSystem);
            }

            var itemSystemVM = new HomebrewItemSystemViewModel
            {
                Systems = new SelectList(await systemQuery.Distinct().ToListAsync()),
                HomebrewItems = await homebrewItems.ToListAsync()
            };

            return View(itemSystemVM);
        }

        // GET: HomebrewItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homebrewItem = await _context.HomebrewItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homebrewItem == null)
            {
                return NotFound();
            }

            return View(homebrewItem);
        }

        // GET: HomebrewItems/Create
        [Authorize(Roles = "Admin,User")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: HomebrewItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Create([Bind("Id,Title,Type,Version,Source,System,Author,ReleaseDate,Link,Content")] HomebrewItem homebrewItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(homebrewItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(homebrewItem);
        }

        // GET: HomebrewItems/Edit/5
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homebrewItem = await _context.HomebrewItem.FindAsync(id);
            if (homebrewItem == null)
            {
                return NotFound();
            }
            return View(homebrewItem);
        }

        // POST: HomebrewItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Type,Version,Source,System,Author,ReleaseDate,Link,Content")] HomebrewItem homebrewItem)
        {
            if (id != homebrewItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(homebrewItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomebrewItemExists(homebrewItem.Id))
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
            return View(homebrewItem);
        }

        // GET: HomebrewItems/Delete/5
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homebrewItem = await _context.HomebrewItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homebrewItem == null)
            {
                return NotFound();
            }

            return View(homebrewItem);
        }

        // POST: HomebrewItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var homebrewItem = await _context.HomebrewItem.FindAsync(id);
            if (homebrewItem != null)
            {
                _context.HomebrewItem.Remove(homebrewItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomebrewItemExists(int id)
        {
            return _context.HomebrewItem.Any(e => e.Id == id);
        }
    }
}
