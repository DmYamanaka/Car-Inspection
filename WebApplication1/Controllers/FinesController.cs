using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace WebApplication1.Controllers
{
    public class FinesController : Controller
    {
        private readonly CarInspectionContext _context;
        IWebHostEnvironment _appEnvironment;

        public FinesController(CarInspectionContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        // GET: Fines
        public async Task<IActionResult> Index()
        {
            var carInspectionContext = _context.Fines.Include(f => f.IdDriverNavigation).Include(f => f.IdFineNavigation);
            return View(await carInspectionContext.ToListAsync());
        }

        // GET: Fines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fines = await _context.Fines
                .Include(f => f.IdDriverNavigation)
                .Include(f => f.IdFineNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fines == null)
            {
                return NotFound();
            }

            return View(fines);
        }

        // GET: Fines/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["IdDriver"] = new SelectList(_context.Drivers, "Id", "Name");
            ViewData["IdFine"] = new SelectList(_context.FinesList, "Id", "Description");
            return View();
        }

        // POST: Fines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile uploadedFile, [Bind("Id,IdDriver,IdFine,DateFine,PhotoFine")] Fines fines)
        {
            if (uploadedFile != null)
            {
                // путь к папке Files
                string path = "/photo/Fines/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.OpenOrCreate))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                fines.PhotoFine = uploadedFile.FileName;
            }

            if (ModelState.IsValid)
            {
                _context.Add(fines);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdDriver"] = new SelectList(_context.Drivers, "Id", "Name", fines.IdDriver);
            ViewData["IdFine"] = new SelectList(_context.FinesList, "Id", "Description", fines.IdFine);
            return View(fines);
        }

        // GET: Fines/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fines = await _context.Fines.FindAsync(id);
            if (fines == null)
            {
                return NotFound();
            }
            ViewData["IdDriver"] = new SelectList(_context.Drivers, "Id", "Name", fines.IdDriver);
            ViewData["IdFine"] = new SelectList(_context.FinesList, "Id", "Description", fines.IdFine);
            return View(fines);
        }

        // POST: Fines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IFormFile uploadedFile, int id, [Bind("Id,IdDriver,IdFine,DateFine,PhotoFine")] Fines fines)
        {
            if (uploadedFile != null)
            {
                // путь к папке Files
                string path = "/photo/Fines/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.OpenOrCreate))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                fines.PhotoFine = uploadedFile.FileName;
            }

            if (id != fines.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fines);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FinesExists(fines.Id))
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
            ViewData["IdDriver"] = new SelectList(_context.Drivers, "Id", "Name", fines.IdDriver);
            ViewData["IdFine"] = new SelectList(_context.FinesList, "Id", "Description", fines.IdFine);
            return View(fines);
        }

        // GET: Fines/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fines = await _context.Fines
                .Include(f => f.IdDriverNavigation)
                .Include(f => f.IdFineNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fines == null)
            {
                return NotFound();
            }

            return View(fines);
        }

        // POST: Fines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fines = await _context.Fines.FindAsync(id);
            _context.Fines.Remove(fines);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FinesExists(int id)
        {
            return _context.Fines.Any(e => e.Id == id);
        }

        public IActionResult ExportExcel()
        {
            var carInspectionContext = _context.Fines.Include(f => f.IdDriverNavigation).Include(f => f.IdFineNavigation);
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add($"Отчет-{DateTime.Now.ToString("yyyyMMddHH")}");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Водитель";
                worksheet.Cell(currentRow, 2).Value = "Описание штрафа";
                worksheet.Cell(currentRow, 3).Value = "Дата";
                worksheet.Cell(currentRow, 4).Value = "Размер штрафа (руб.)";
                foreach (var fines in carInspectionContext)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = fines.IdDriverNavigation.Name;
                    worksheet.Cell(currentRow, 2).Value = fines.IdFineNavigation.Description;
                    worksheet.Cell(currentRow, 3).Value = fines.DateFine;
                    worksheet.Cell(currentRow, 4).Value = fines.IdFineNavigation.Fine;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Отчет - {DateTime.Now.ToString("yyyyMMdd")}.xlsx");
                }
            }
        }
    }
}
