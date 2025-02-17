using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Super_Simple_Homebrew_Hoster.Data;
using Super_Simple_Homebrew_Hoster.Models;
using Super_Simple_Homebrew_Hoster.Areas.Identity.Data;

namespace Super_Simple_Homebrew_Hoster.Controllers
{
    public class HomebrewItemsController : Controller
    {
        private readonly Super_Simple_Homebrew_HosterContext _context;
        private readonly UserAccountsContext _accountsContext;
        private readonly UserManager<HomebrewUser> _userManager;

        public HomebrewItemsController(Super_Simple_Homebrew_HosterContext context, UserAccountsContext accountsContext, UserManager<HomebrewUser> userManager)
        {
            _context = context;
            _accountsContext = accountsContext;
            _userManager = userManager;
        }

        // GET: HomebrewItems
        public async Task<IActionResult> Index(string itemSystem, string titleSearch, string sourceSearch, string authorSearch)
        {
            if (_context.HomebrewItem == null)
            {
                return Problem("Entity set 'Super_Simple_Homebrew_Hoster.HomebrewItem' is null.");
            }

            // Use LINQ to get a list of available systems.
            IQueryable<string> systemQuery = from i in _context.HomebrewItem
                                             orderby i.System
                                             select i.System;

            var homebrewItems = from m in _context.HomebrewItem
                         select m;

            if (!String.IsNullOrEmpty(titleSearch))
            {
                homebrewItems = homebrewItems.Where(item => item.Title!.ToUpper().Contains(titleSearch.ToUpper()));
            }
            if (!String.IsNullOrEmpty(sourceSearch))
            {
                homebrewItems = homebrewItems.Where(item => item.Source!.ToUpper().Contains(sourceSearch.ToUpper()));
            }
            if (!String.IsNullOrEmpty(authorSearch))
            {
                homebrewItems = homebrewItems.Where(item => item.Author!.ToUpper().Contains(authorSearch.ToUpper()));
            }
            if (!String.IsNullOrEmpty(itemSystem))
            {
                homebrewItems = homebrewItems.Where(item => item.System == itemSystem);
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
        // Creates a new HomebrewItem with the user-supplied attributes.
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Create([Bind("Title,Type,Version,Source,System,ReleaseDate,Link,Content")] HomebrewItem homebrewItem)
        {
            if (ModelState.IsValid)
            {
                HomebrewUser? currentUser = _userManager.GetUserAsync(User).Result; // Get the currently logged-in user
                if (currentUser != null) {
                    homebrewItem.Author = currentUser.UserName;
                    _context.Add(homebrewItem); // Adds 'homebrewItem' object to the Super_Simple_Homebrew_Hoster context.                
                    _context.SaveChanges();  // Saves the above changes to the database

                    currentUser.BrewsCreated?.Add(homebrewItem.Id); // Add Id of the homebrewItem to the user's BrewsCreated
                    _accountsContext.Update(currentUser); // Update the UserAccountsContext AspNetUsers table with the modified data
                    await _accountsContext.SaveChangesAsync(); // Save the changes to the table
                }
                else {
                    throw new Exception("Error creating item: Current user is null and does not exist.");
                }
                
                return RedirectToAction(nameof(Index)); // Redirect user back to the index page, where their changes should now be visible
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

            // Don't allow incorrect users to access the edit page of an item they haven't made
            var currentUser = _userManager.GetUserAsync(User).Result;
            if (currentUser != null)
            {
                if (currentUser.UserName != homebrewItem.Author && !currentUser.BrewsCreated.Contains(homebrewItem.Id))
                {
                    return Forbid();
                }
            }
            return View(homebrewItem);
        }

        // POST: HomebrewItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Edit(int id, [Bind("Title,Type,Version,Source,System,ReleaseDate,Link,Content")] HomebrewItem homebrewItem)
        {
            if (id != homebrewItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Don't allow incorrect users to edit an item they haven't made
                var currentUser = _userManager.GetUserAsync(User).Result;
                if (currentUser != null)
                {
                    if (currentUser.UserName != homebrewItem.Author && !currentUser.BrewsCreated.Contains(homebrewItem.Id))
                    {
                        return Forbid();
                    }
                }
                else
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

            // Don't allow incorrect users to access the delete page of an item they haven't made
            var currentUser = _userManager.GetUserAsync(User).Result;
            if (currentUser != null)
            {
                if (currentUser.UserName != homebrewItem.Author && !currentUser.BrewsCreated.Contains(homebrewItem.Id))
                {
                    return Forbid();
                }
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
                HomebrewUser userToRemoveBrewFrom;
                // If an admin, find the user according to item author; otherwise get currently logged-in user
                if (User.IsInRole("Admin")) 
                {
                    userToRemoveBrewFrom = _userManager.FindByNameAsync(homebrewItem.Author).Result;
                } 
                else 
                {
                    userToRemoveBrewFrom = _userManager.GetUserAsync(User).Result;
                }

                if (userToRemoveBrewFrom != null) {
                    // Don't allow incorrect users to delete an item they haven't made
                    if (userToRemoveBrewFrom.UserName != homebrewItem.Author && !userToRemoveBrewFrom.BrewsCreated.Contains(homebrewItem.Id))
                    {
                        return Forbid();
                    }
                    _context.HomebrewItem.Remove(homebrewItem);

                    try 
                    {
                        userToRemoveBrewFrom.BrewsCreated?.Remove(homebrewItem.Id); // Remove Id of the homebrewItem from the user's BrewsCreated
                        _accountsContext.Update(userToRemoveBrewFrom); // Update the UserAccountsContext AspNetUsers table with the modified data
                        await _accountsContext.SaveChangesAsync(); // Save changes
                    } 
                    catch (DbUpdateConcurrencyException) 
                    {
                        throw new DbUpdateConcurrencyException("Error: more changes made to database than expected.");
                    }
                } 
                else 
                {
                    throw new Exception("Error creating item: Current user is null and does not exist.");
                }
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
