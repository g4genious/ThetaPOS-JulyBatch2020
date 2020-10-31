using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThetaPOS.Models;

namespace ThetaPOS.Controllers
{
    public class ProductsController : Controller
    {
        private readonly theta_posContext _context;
        private readonly IWebHostEnvironment _env;
        public ProductsController(theta_posContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Product.ToListAsync());
        }
        public IActionResult ProductsData(int id)
        {
            ViewModelProductsData productdata = (from p in _context.Product.Where(p=>p.Id == id)
                               join c in _context.ProductCategory on p.ProductCategoryId equals c.Id
                               join b in _context.ProductBrand on p.ProductBrandId equals b.Id 
                               select new ViewModelProductsData() 
                               { 
                                  //sir code theek hai ?
                                   Id = p.Id,
                                   Name = p.Name,
                                   Barcode = p.Barcode,
                                   ShortDescription = p.ShortDescription,
                                   LongDescription = p.LongDescription,
                                   Features = p.Features,
                                   ProductBrand = b.Name,
                                   ProductCategory = c.Name,
                                   CurrentSalePrice = p.CurrentSalePrice,
                                   LatestPurchasePrice = p.LatestPurchasePrice,
                                   Images = p.Images,
                                   Views = p.Views,
                                   OpeningStock = p.OpeningStock,
                                   OpeningDate = p.OpeningDate,
                                   CurrentStock = p.CurrentStock,
                                   Status = p.Status,
                                   CreatedDate = p.CreatedDate,
                                   CreatedBy = p.CreatedBy,
                                   ModifiedDate = p.ModifiedDate,
                                   ModifiedBy = p.ModifiedBy
                               }).FirstOrDefault();
            return View(productdata);
        }
        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            ViewBag.pimages = product.Images;
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
           ViewBag.categorylist = _context.ProductCategory.ToList();
            ViewBag.brandlist = _context.ProductBrand.ToList();

            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IList<IFormFile> pimg)
        {
            if (ModelState.IsValid)
            {
                var Pname = await _context.Product.FirstOrDefaultAsync(m => m.Name == product.Name);
                if (Pname == null)
                {
                    if (pimg != null)
                    {
                        string productimgFPath = _env.WebRootPath + "/products/";
                        string Filepath = "";
                        foreach (var Images in pimg)
                        {
                            string filename = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(Images.FileName);
                            Filepath = productimgFPath + filename;
                            System.IO.FileStream fs = new System.IO.FileStream(Filepath,System.IO.FileMode.Create);
                            Images.CopyTo(fs);
                            product.Images += (filename+",");
                        
                        
                        }
                        if (!string.IsNullOrEmpty(product.Images))
                        {
                            product.Images = product.Images.Remove(product.Images.LastIndexOf(","));
                        }
                    }
                    product.Status = "Active";
                    product.CreatedDate = DateTime.Now;
                    product.OpeningDate = DateTime.Now;

                    _context.Add(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
              
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Barcode,ShortDescription,LongDescription,Features,ProductBrandId,ProductCategoryId,CurrentSalePrice,LatestPurchasePrice,Images,Views,OpeningStock,OpeningDate,CurrentStock,Status,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
