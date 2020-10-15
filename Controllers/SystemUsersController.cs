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
    public class SystemUsersController : Controller
    {
        private readonly theta_posContext _context;
        private readonly IWebHostEnvironment _env;

        public SystemUsersController(theta_posContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: SystemUsers
        public async Task<IActionResult> Index()
        {
            return View(await _context.SystemUser.ToListAsync());
        }

        // GET: SystemUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemUser = await _context.SystemUser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (systemUser == null)
            {
                return NotFound();
            }

            return View(systemUser);
        }

        // GET: SystemUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SystemUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( SystemUser systemUser, IFormFile PP)
        {
            if (ModelState.IsValid)
            {
                if(PP!=null && PP.Length>0)
                {
                    string ppsPath = _env.WebRootPath + "/pps/";
                    string FileUniqueName = Guid.NewGuid().ToString()+Path.GetExtension(PP.FileName);
                  await  PP.CopyToAsync(new FileStream(ppsPath+FileUniqueName,FileMode.CreateNew));

                    systemUser.ProfilePicture = FileUniqueName;
                }


                systemUser.Role = "Staff";
                systemUser.CreatedDate = DateTime.Now;
                systemUser.CreatedBy = "System";


                _context.Add(systemUser);
                await _context.SaveChangesAsync();

                //send registration email to new user

                if (!string.IsNullOrEmpty(systemUser.Email))
                {
                    MailMessage MM = new MailMessage();
                    MM.From = new MailAddress("students.thetasolutions@gmail.com", "Theta POS");
                    MM.To.Add(systemUser.Email);
                    MM.Subject = "Welcome to Theta POS";
                    MM.Body = "Dear " + systemUser.DisplayName + ",<br/><br/>Thanks for registering with Theta POS. Please find below your credentials and keep them safe to login on Theta POS.<br/><br/>" +

                        "<span style='color:green;'> Username: " + systemUser.Username + "<br/>" +
                        "Password: " + systemUser.Password + "</span><br/><br/>" +

                        "Feel free to contact us in case you need any assistance.<br/><br/>" +

                        "Regards,<br/>" +
                        "Team Theta POS";

                    MM.IsBodyHtml = true;




                    SmtpClient SC = new SmtpClient();
                    SC.Credentials = new System.Net.NetworkCredential("students.thetasolutions@gmail.com", "fBd6LHc3zX2t");
                    SC.Host = "smtp.gmail.com";
                    SC.Port = 587;
                    SC.EnableSsl = true;
                   await SC.SendMailAsync(MM);


                }
                
                
                return RedirectToAction(nameof(Index));
            }
            return View(systemUser);
        }

        // GET: SystemUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemUser = await _context.SystemUser.FindAsync(id);
            if (systemUser == null)
            {
                return NotFound();
            }
            return View(systemUser);
        }

        // POST: SystemUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Password,DisplayName,ProfilePicture,Address,Mobile,Role,Email,Status,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy")] SystemUser systemUser)
        {
            if (id != systemUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(systemUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SystemUserExists(systemUser.Id))
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
            return View(systemUser);
        }

        // GET: SystemUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var systemUser = await _context.SystemUser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (systemUser == null)
            {
                return NotFound();
            }

            return View(systemUser);
        }

        // POST: SystemUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var systemUser = await _context.SystemUser.FindAsync(id);
            _context.SystemUser.Remove(systemUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SystemUserExists(int id)
        {
            return _context.SystemUser.Any(e => e.Id == id);
        }
    }
}
