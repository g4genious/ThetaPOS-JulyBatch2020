﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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

        // GET: SystemUsers/Create updated by Rizwan

        public IActionResult Create()
        {
            return View();
        }

        // POST: SystemUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SystemUser systemUser,IFormFile user_pic)
        {
            if (ModelState.IsValid)
            {
               Boolean usr = _context.SystemUser.Any(user => user.Username == systemUser.Username);
                Boolean usrmail = _context.SystemUser.Any(user => user.Email == systemUser.Email);
                if (!usr && !usrmail)
                {
                    if (user_pic != null)
                    {
                        string pic_name = Guid.NewGuid().ToString() + Path.GetExtension(user_pic.FileName);
                        string pic_path = _env.WebRootPath.ToString() + "/WebData/SystemUsersImages/" + (pic_name);
                        System.IO.FileStream FS = new System.IO.FileStream(pic_path, FileMode.Create);
                        await user_pic.CopyToAsync(FS);
                        systemUser.ProfilePicture = pic_name;
                    }
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress("bsef17m526@pucit.edu.pk", "TheetaPOS");
                    mail.To.Add(systemUser.Email);
                    mail.Subject = "Welcome!" + systemUser.DisplayName + " You are Registered Successfully";
                    mail.Body = "<h3>Congratulations!</h3><br><p>You will be happy after using our Services</p>";
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Credentials = new System.Net.NetworkCredential("bsef17m526@pucit.edu.pk", "rizwan67");
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                    systemUser.Role = "Staff";
                    _context.Add(systemUser);
                    await _context.SaveChangesAsync();
                    ViewBag.SuccMsg = "Successfuly Registered";
                    return RedirectToAction(nameof(Login));
                    
                }
                else
                {
                   ViewBag.ErrMsg = "This "+systemUser.Username+" or "+systemUser.Email+" is Already Exist";
                   return View(systemUser);
                }
            
            }
            return View(nameof(Create));

        }
        public IActionResult AdminView()
        {
            return View();
        }
        public IActionResult StaffView()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string username,string password)
        {
            SystemUser usr = _context.SystemUser.FirstOrDefault(user => user.Username ==username && user.Password==password);
            if (usr!=null)
            {
                HttpContext.Session.SetString("Role", usr.Role);
                HttpContext.Session.SetString("Username", usr.Username);
                HttpContext.Session.SetInt32("Id", usr.Id);
                if (usr.Role=="Admin")
                {
                    return RedirectToAction(nameof(AdminView
                        ));
                }
                else
                {
                    return RedirectToAction(nameof(StaffView));
                }
            }
            else
            {
                ViewBag.ErrMsg = "Invalid username or password";
                return View(usr);
            }
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }
        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ResetPassword(String password,int id)

        {
            SystemUser ps = _context.SystemUser.Where(usr=>usr.Id==id).FirstOrDefault();
            ps.Password = password;
            _context.Update(ps);
            _context.SaveChanges();
          
            if (ModelState.IsValid)
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("bsef17m526@pucit.edu.pk", "TheetPOS");
                mail.To.Add(ps.Email);
                mail.Subject = "Welcome Back! " + ps.DisplayName;
                mail.Body = "Your  New Password is: " + ps.Password + " </ br > ";
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Credentials = new System.Net.NetworkCredential("bsef17m526@pucit.edu.pk", "rizwan67");
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Send(mail);
                ViewBag.passwordSend = "Your New password has been send to this email please check!";
                HttpContext.Session.Clear();
                return RedirectToAction(nameof(Login));

            }
            return View();
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost][AllowAnonymous]
        [ValidateAntiForgeryToken]
        public  IActionResult ForgotPassword(SystemUser usr)
        {

            SystemUser ps = _context.SystemUser.Where(user => user.Email == usr.Email).FirstOrDefault();
            if (ModelState.IsValid)
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("bsef17m526@pucit.edu.pk", "TheetPOS");
                mail.To.Add(usr.Email);
                mail.Subject = "Welcome Back! " + ps.DisplayName; 
                mail.Body = "Your Password is: "+ps.Password+" </ br > " ;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Credentials = new System.Net.NetworkCredential("bsef17m526@pucit.edu.pk", "rizwan67");
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Send(mail);
                ViewBag.passwordSend = "Your password has been send to this email please check!";
                return View(ps);
            }
            return View();

            
        }
   
        public IActionResult Profile()
        {
            return View();
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

        //public IActionResult Register()
        //{

        //}

        // POST: SystemUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,SystemUser systemUser, IFormFile pp)
        {
            if (id != systemUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (pp != null)
                {
                    string pic_name = Guid.NewGuid().ToString() + Path.GetExtension(pp.FileName);
                    string pic_path = _env.WebRootPath.ToString() + "/WebData/SystemUsersImages/" + (pic_name);
                    System.IO.FileStream FS = new System.IO.FileStream(pic_path, FileMode.Create);
                    await pp.CopyToAsync(FS);
                    systemUser.ProfilePicture = pic_name;
                }
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
