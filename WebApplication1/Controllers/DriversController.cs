using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace WebApplication1.Controllers
{
    public class DriversController : Controller
    {
        private readonly CarInspectionContext _context;
        IWebHostEnvironment _appEnvironment;

        public DriversController(CarInspectionContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        // GET: Drivers
        public async Task<IActionResult> Index(string searchString)
        {
            var fines = from t in _context.Fines
                         select t.IdFineNavigation;
            ViewBag.Fines = fines;

            var carInspectionContext = from m in _context.Drivers.Include(m => m.Fines)
                                       select m;
            if (!string.IsNullOrEmpty(searchString))
            {
                carInspectionContext = carInspectionContext.Where(s => s.Name.Contains(searchString));
            }
            return View(await carInspectionContext.ToListAsync());
        }

        // GET: Drivers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sumfines = from s in _context.Fines
                          where s.IdDriver == id
                          select s.IdFineNavigation.Fine;
            ViewBag.SumFines = sumfines.Sum();

            var fine = from t in _context.Fines
                        where t.IdDriver == id
                        select t.IdFineNavigation;
            ViewBag.Fine = fine.Any();

            var fines = from t in _context.Fines
                        where t.IdDriver == id
                        select t.IdFineNavigation;
            ViewBag.Fines = fines;

            var driver = await _context.Drivers
                .Include(d => d.Fines)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (driver == null)
            {
                return NotFound();
            }

            return View(driver);
        }
        // GET: Drivers/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["Fine"] = new SelectList(_context.Fines, "Id", "Id");
            return View();
        }

        // POST: Drivers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile uploadedFile, [Bind("Id,Name,PassportSerial,PassportNumber,Address,Company,Jobname,Phone,Email,Photo,Fine")] Drivers driver)
        {
            if (uploadedFile != null)
            {
                // путь к папке Files
                string path = "/photo/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.OpenOrCreate))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                driver.Photo = uploadedFile.FileName;
            }

            if (ModelState.IsValid)
            {
                _context.Add(driver);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Fine"] = new SelectList(_context.Fines, "Id", "Id", driver.Fines);
            return View(driver);
        }

        // GET: Drivers/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _context.Drivers.FindAsync(id);
            if (driver == null)
            {
                return NotFound();
            }
            ViewData["Fine"] = new SelectList(_context.Fines, "Id", "Description", driver.Fines);
            return View(driver);
        }

        // POST: Drivers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IFormFile uploadedFile, int id, [Bind("Id,Name,PassportSerial,PassportNumber,Address,Company,Jobname,Phone,Email,Photo,Fine")] Drivers driver)
        {
            if (uploadedFile != null)
            {
                // путь к папке Files
                string path = "/photo/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.OpenOrCreate))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                driver.Photo = uploadedFile.FileName;
            }

            if (id != driver.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(driver);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DriverExists(driver.Id))
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
            ViewData["Fine"] = new SelectList(_context.Fines, "Id", "Id", driver.Fines);
            return View(driver);
        }

        // GET: Drivers/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _context.Drivers
                .Include(d => d.Fines)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (driver == null)
            {
                return NotFound();
            }

            return View(driver);
        }

        // POST: Drivers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var driver = await _context.Drivers.FindAsync(id);
            _context.Drivers.Remove(driver);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DriverExists(int id)
        {
            return _context.Drivers.Any(e => e.Id == id);
        }
    }
}
