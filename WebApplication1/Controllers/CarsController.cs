using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    public class CarsController : Controller
    {
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        public IList<Cars> Cars { get; set; }
        private readonly CarInspectionContext _context;
        IWebHostEnvironment _appEnvironment;
        public CarsController(CarInspectionContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        // GET: Cars
        public async Task<IActionResult> Index(string searchString)
        {
            var carInspectionContext = from m in _context.Cars.Include(m => m.ColorNavigation).Include(m => m.IdDriverNavigation)
                                       select m;
            if (!string.IsNullOrEmpty(searchString))
            {
                carInspectionContext = carInspectionContext.Where(s => s.StateNumber.Contains(searchString));
            }
            return View(await carInspectionContext.ToListAsync());
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Include(c => c.ColorNavigation)
                .Include(c => c.IdDriverNavigation)
                .FirstOrDefaultAsync(m => m.CarId == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }


        // GET: Cars/Create
        [Authorize]
        public  IActionResult Create()
        {
            ViewData["Color"] = new SelectList(_context.CarColor, "ColorNum", "ColorName");
            ViewData["IdDriver"] = new SelectList(_context.Drivers, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile uploadedFile, [Bind("CarId,IdDriver,Vin,Manufacturer,Model,Year,Weight,Color,EngineType,TypeOfDrive,StateNumber,Dtp,DateDtp,PhotoDtp")] Cars car)
        {
            if (uploadedFile != null)
            {
                // путь к папке Files
                string path = "/photo/DTP/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.OpenOrCreate))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                car.PhotoDtp = uploadedFile.FileName;
            }

            if (ModelState.IsValid)
            {
                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Color"] = new SelectList(_context.CarColor, "ColorNum", "ColorName", car.Color);
            ViewData["IdDriver"] = new SelectList(_context.Drivers, "Id", "Name", car.IdDriver);
            return View(car);
        }

        // GET: Cars/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            ViewData["Color"] = new SelectList(_context.CarColor, "ColorNum", "ColorName", car.Color);
            ViewData["IdDriver"] = new SelectList(_context.Drivers, "Id", "Name", car.IdDriver);
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IFormFile uploadedFile, int id, [Bind("CarId,IdDriver,Vin,Manufacturer,Model,Year,Weight,Color,EngineType,TypeOfDrive,StateNumber,Dtp,DateDtp,PhotoDtp")] Cars car)
        {
            if (uploadedFile != null)
            {
                // путь к папке Files
                string path = "/photo/DTP/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.OpenOrCreate))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                car.PhotoDtp = uploadedFile.FileName;
            }

            if (id != car.CarId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(car);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.CarId))
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
            ViewData["Color"] = new SelectList(_context.CarColor, "ColorNum", "ColorName", car.Color);
            ViewData["IdDriver"] = new SelectList(_context.Drivers, "Id", "Name", car.IdDriver);
            return View(car);
        }

        // GET: Cars/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Include(c => c.ColorNavigation)
                .Include(c => c.IdDriverNavigation)
                .FirstOrDefaultAsync(m => m.CarId == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.CarId == id);
        }
    }
}
