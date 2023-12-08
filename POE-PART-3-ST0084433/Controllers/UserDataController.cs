using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using POE_PART_3_ST0084433.Data;
using TestMVC.Models;

namespace POE_PART_3_ST0084433.Controllers
{
    [Authorize]
    public class UserDataController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotyfService _notyf;

        public UserDataController(ApplicationDbContext context,INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }

        // GET: UserData
        public async Task<IActionResult> Index()
        {
            var username = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(await _context.UserData.Where(x => x.userName == username).ToListAsync());
        }

        // GET: UserData/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var username = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userData = await _context.UserData
                .FirstOrDefaultAsync(m => m.userName == username);
            if (userData == null)
            {
                return NotFound();
            }

            return View(userData);
        }

        // GET: UserData/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserData/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,userName,moduleCode,moduleName,numberOfCredits,classHours,semesterName,semesterDuration,semesterDate,dateToStudy")] UserData userData)
        {
            // assigns current logged in user info to username in userData DB table
            userData.userName = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ModelState.IsValid)
            {
                _notyf.Success("Module added", 3);

                _context.Add(userData);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userData);
        }

        // GET: UserData/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userData = await _context.UserData.FindAsync(id);
            if (userData == null)
            {
                return NotFound();
            }
            return View(userData);
        }

        // POST: UserData/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,userName,moduleCode,moduleName,numberOfCredits,classHours,semesterName,semesterDuration,semesterDate,dateToStudy")] UserData userData)
        {
            if (id != userData.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserDataExists(userData.ID))
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
            return View(userData);
        }

        // GET: UserData/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userData = await _context.UserData
                .FirstOrDefaultAsync(m => m.ID == id);
            if (userData == null)
            {
                return NotFound();
            }

            return View(userData);
        }

        // POST: UserData/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userData = await _context.UserData.FindAsync(id);
            _context.UserData.Remove(userData);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserDataExists(int id)
        {
            return _context.UserData.Any(e => e.ID == id);
        }
    }
}
