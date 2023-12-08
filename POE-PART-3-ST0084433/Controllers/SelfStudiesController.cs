using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using POE_PART_3_ST0084433.Data;
using POE_PART_3_ST0084433.Models;
using TestMVC.Models;
using CalcRef;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace POE_PART_3_ST0084433.Controllers
{
    [Authorize]
    public class SelfStudiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly INotyfService _notyf;


        public SelfStudiesController(ApplicationDbContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }

        // GET: SelfStudies
        public async Task<IActionResult> Index()
        {

            var username = User.FindFirstValue(ClaimTypes.NameIdentifier);

            DateTime today = DateTime.Today;

                var dateList = from x in _context.UserData
                               where x.userName == username
                               select x.dateToStudy;

                var modList = from x in _context.UserData
                              where x.userName == username && x.dateToStudy == today
                              select x.moduleCode;


            if (dateList.Contains(today))
                {
                foreach (var item in modList)
                  {
                    _notyf.Information(item + " needs to be studied today", 3);
                  }

                }

                //if (selfStudy.studyDate.Equals(today)) // if the date assigned to study on is equal to the day the user logs in -> pop up will be displayed
                //{
                //    _notyf.Information(selfStudy.moduleCode + " needs to be studied today", 3);
                //}

            return View(await _context.SelfStudy.Where(x => x.userName == username).ToListAsync());
        }

        // GET: SelfStudies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var username = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var selfStudy = await _context.SelfStudy
                .FirstOrDefaultAsync(m => m.userName == username);
            if (selfStudy == null)
            {
                return NotFound();
            }

            return View(selfStudy);
        }

        // GET: SelfStudies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SelfStudies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,userName,moduleCode,selfStudyHours,studyHoursRemaining,studyDate,studyDuration")] SelfStudy selfStudy)
        {
            // used to assign username in selfStudy table = to current logged in user 
            selfStudy.userName = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // temp user variable to be used in calculations
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // assigning static module code in DB
            CalcRef.Class1.moduleCode = selfStudy.moduleCode;

            // methods used to search respective credits, hours and semester weeks as per user and module
            string creditsTemp = searchCredits(CalcRef.Class1.moduleCode, user);
            string hoursTemp = searchHours(CalcRef.Class1.moduleCode, user);
            string semesterTemp = searchSemester(CalcRef.Class1.moduleCode, user);


            // selects list of modules registered in DB respective to the user logged in
            var modList = from x in _context.UserData
                          where x.userName == user
                          select x.moduleCode;

            // module entered by user must exist in order for if statement to succeed
            if (modList.Contains(selfStudy.moduleCode))
            { 
                // assigning self study hours to result of calc in DLL
                selfStudy.selfStudyHours = CalcRef.Class1.Calculation(Convert.ToInt32(creditsTemp), Convert.ToInt32(semesterTemp), Convert.ToInt32(hoursTemp));

                CalcRef.Class1.selfStudyHours = selfStudy.selfStudyHours;

                // calculating remaining hours after removing user input
                selfStudy.studyHoursRemaining = CalcRef.Class1.selfStudyHours - Convert.ToInt32(selfStudy.studyDuration);


                if (ModelState.IsValid)
                {               
                    _notyf.Success("Study date recorded", 3);
                    _context.Add(selfStudy);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                _notyf.Warning("Module does not exist!", 2); // displayed if user inputted module code doesnt exist in DB
            }
            return View(selfStudy);
        }

        // GET: SelfStudies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var selfStudy = await _context.SelfStudy.FindAsync(id);
            if (selfStudy == null)
            {
                return NotFound();
            }
            return View(selfStudy);
        }

        // POST: SelfStudies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,userName,moduleCode,selfStudyHours,studyHoursRemaining,studyDate,studyDuration")] SelfStudy selfStudy)
        {
            if (id != selfStudy.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(selfStudy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SelfStudyExists(selfStudy.ID))
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
            return View(selfStudy);
        }

        // GET: SelfStudies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var selfStudy = await _context.SelfStudy
                .FirstOrDefaultAsync(m => m.ID == id);
            if (selfStudy == null)
            {
                return NotFound();
            }

            return View(selfStudy);
        }

        // POST: SelfStudies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var selfStudy = await _context.SelfStudy.FindAsync(id);
            _context.SelfStudy.Remove(selfStudy);
            await _context.SaveChangesAsync();
            _notyf.Warning("Study record deleted successfully",3);
            return RedirectToAction(nameof(Index));
        }

        private bool SelfStudyExists(int id)
        {
            return _context.SelfStudy.Any(e => e.ID == id);
        }



        #region Search for credits
        public string searchCredits(string moduleCode, string username)
        {
            // retrieves credits
            var users = from x in _context.UserData
                        where x.userName == username && x.moduleCode == CalcRef.Class1.moduleCode
                        select x.numberOfCredits;

            string data = string.Empty;

            foreach (var m in users.ToList())
            {
                data += (m); // assigns values from linq statement into variable 
            }

            return data;
        }
        #endregion


        #region Search for hours
        public string searchHours(string moduleCode, string username)
        {

            // retrieves hours
            var users = from x in _context.UserData
                        where x.userName == username && x.moduleCode == moduleCode
                        select x.classHours;

            string data = string.Empty;

            foreach (var m in users.ToList())
            {
                data += (m); // assigns values from linq statement into variable 
            }

            return data;
        }
        #endregion


        #region Search for semester
        public string searchSemester(string moduleCode, string username)
        {
            // retrieves semester duration
            var users = from x in _context.UserData
                        where x.userName == username && x.moduleCode == moduleCode
                        select x.semesterDuration;

            string data = string.Empty;

            foreach (var m in users.ToList())
            {
                data += (m); // assigns values from linq statement into variable 
            }

            return data;
        }
        #endregion
    }
}
