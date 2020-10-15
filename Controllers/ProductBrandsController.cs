using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThetaPOS.Models;

namespace ThetaPOS.Controllers
{
    public class ProductBrandsController : Controller
    {
        private readonly theta_posContext _context;

        public ProductBrandsController(theta_posContext context)
        {
            _context = context;
        }

        // GET: ProductBrands
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProductBrand.ToListAsync());
        }

        // GET: ProductBrands/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productBrand = await _context.ProductBrand
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productBrand == null)
            {
                return NotFound();
            }

            return View(productBrand);
        }

        // GET: ProductBrands/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductBrands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Logo,Website,Description,Status,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] ProductBrand productBrand)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productBrand);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productBrand);
        }

        // GET: ProductBrands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productBrand = await _context.ProductBrand.FindAsync(id);
            if (productBrand == null)
            {
                return NotFound();
            }
            return View(productBrand);
        }

        // POST: ProductBrands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Logo,Website,Description,Status,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] ProductBrand productBrand)
        {
            if (id != productBrand.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productBrand);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductBrandExists(productBrand.Id))
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
            return View(productBrand);
        }

        // GET: ProductBrands/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productBrand = await _context.ProductBrand
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productBrand == null)
            {
                return NotFound();
            }

            return View(productBrand);
        }

        // POST: ProductBrands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productBrand = await _context.ProductBrand.FindAsync(id);
            _context.ProductBrand.Remove(productBrand);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductBrandExists(int id)
        {
            return _context.ProductBrand.Any(e => e.Id == id);
        }
    }
}
