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
    public class ProductPurchasesController : Controller
    {
        private readonly theta_posContext _context;

        public ProductPurchasesController(theta_posContext context)
        {
            _context = context;
        }

        // GET: ProductPurchases
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProductPurchase.ToListAsync());
        }

        // GET: ProductPurchases/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productPurchase = await _context.ProductPurchase
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productPurchase == null)
            {
                return NotFound();
            }

            return View(productPurchase);
        }

        // GET: ProductPurchases/Create
        public IActionResult Create()
        {
            ViewBag.ProductsList = _context.Product.ToList();
            return View();
        }

        // POST: ProductPurchases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PurchaseDate,PurchasePrice,Discount,FinalPrice,Status,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] ProductPurchase productPurchase)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productPurchase);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productPurchase);
        }

        // GET: ProductPurchases/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productPurchase = await _context.ProductPurchase.FindAsync(id);
            if (productPurchase == null)
            {
                return NotFound();
            }
            return View(productPurchase);
        }

        // POST: ProductPurchases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PurchaseDate,PurchasePrice,Discount,FinalPrice,Status,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] ProductPurchase productPurchase)
        {
            if (id != productPurchase.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productPurchase);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductPurchaseExists(productPurchase.Id))
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
            return View(productPurchase);
        }

        // GET: ProductPurchases/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productPurchase = await _context.ProductPurchase
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productPurchase == null)
            {
                return NotFound();
            }

            return View(productPurchase);
        }

        // POST: ProductPurchases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productPurchase = await _context.ProductPurchase.FindAsync(id);
            _context.ProductPurchase.Remove(productPurchase);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductPurchaseExists(int id)
        {
            return _context.ProductPurchase.Any(e => e.Id == id);
        }
    }
}
