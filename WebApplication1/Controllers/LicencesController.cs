using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    public class LicencesController : Controller
    {
        private readonly CarInspectionContext _context;

        public LicencesController(CarInspectionContext context)
        {
            _context = context;
        }

        // GET: Licences
        public async Task<IActionResult> Index(string searchString)
        {
            var carInspectionContext = from m in _context.Licences.Include(m => m.IdDriverNavigation)
            select m;
            if (!string.IsNullOrEmpty(searchString))
            {
                carInspectionContext = carInspectionContext.Where(s => s.LicenceNumber.ToString().Contains(searchString));
            }
            return View(await carInspectionContext.ToListAsync());
        }

        // GET: Licences/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var licence = await _context.Licences
                .Include(l => l.IdDriverNavigation)
                .FirstOrDefaultAsync(m => m.LicenceNumber == id);
            if (licence == null)
            {
                return NotFound();
            }

            return View(licence);
        }

        // GET: Licences/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["IdDriver"] = new SelectList(_context.Drivers, "Id", "Name");
            return View();
        }

        // POST: Licences/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LicenceNumber,IdDriver,LicenceDate,ExpireDate,Categories,LicenceSeries")] Licence licence)
        {
            if (ModelState.IsValid)
            {
                _context.Add(licence);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdDriver"] = new SelectList(_context.Drivers, "Id", "Name", licence.IdDriver);
            return View(licence);
        }

        // GET: Licences/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var licence = await _context.Licences.FindAsync(id);
            if (licence == null)
            {
                return NotFound();
            }
            ViewData["IdDriver"] = new SelectList(_context.Drivers, "Id", "Name", licence.IdDriver);
            return View(licence);
        }

        // POST: Licences/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LicenceNumber,IdDriver,LicenceDate,ExpireDate,Categories,LicenceSeries")] Licence licence)
        {
            if (id != licence.LicenceNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(licence);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LicenceExists(licence.LicenceNumber))
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
            ViewData["IdDriver"] = new SelectList(_context.Drivers, "Id", "Id", licence.IdDriver);
            return View(licence);
        }

        // GET: Licences/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var licence = await _context.Licences
                .Include(l => l.IdDriverNavigation)
                .FirstOrDefaultAsync(m => m.LicenceNumber == id);
            if (licence == null)
            {
                return NotFound();
            }

            return View(licence);
        }

        // POST: Licences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var licence = await _context.Licences.FindAsync(id);
            _context.Licences.Remove(licence);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LicenceExists(int id)
        {
            return _context.Licences.Any(e => e.LicenceNumber == id);
        }
    }
}
