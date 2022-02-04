using Helperland.Data;
using Helperland.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MailKit.Net.Smtp;
using MimeKit;

namespace Helperland.Controllers
{
    public class HomeController : Controller
    {

        private readonly HelperlandContext _db;
        private readonly ILogger<HomeController> _logger;

        // Dependency Injection
        public HomeController(ILogger<HomeController> logger, HelperlandContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            ViewBag.IsHomePage = true;
            return View();
        }

        [HttpPost]
        public IActionResult Login(User obj)
        {
            User user = _db.Users.Where(x => x.Email == obj.Email && x.Password == obj.Password).SingleOrDefault();
            if (user != null)
            {
                HttpContext.Session.SetInt32("User_Session", user.UserId);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ErrorMsg = "Username or Password is Incorrect";
                return RedirectToAction("Index");
            }
        }
        //ForgotPassword
       

        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            var _objuserdetail = (from i in _db.Users where i.Email == email select i).FirstOrDefault();
            if (_objuserdetail != null)
            {
                MimeMessage message = new MimeMessage();

                MailboxAddress from = new MailboxAddress("Admin",
                "getpaswordback@gmail.com");
                message.From.Add(from);

                MailboxAddress to = new MailboxAddress("User",_objuserdetail.Email);
                message.To.Add(to);

                message.Subject = "Reset Password";

                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = "<h1>Hello World!</h1>";
                bodyBuilder.TextBody = "Hello World!";

                SmtpClient client = new SmtpClient();
                client.Connect("smtp.gmail.com", 587, true);
                client.Authenticate("getpaswordback@gmail.com", "Demo@123");

                client.Send(message);
                client.Disconnect(true);
                client.Dispose();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Contact");
                //ViewBag.Message = 0;
            }
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(ContactU obj)
        {
            obj.Name = HttpContext.Request.Form["FName"] + " " + HttpContext.Request.Form["LName"];
            obj.CreatedOn = DateTime.Now;
            _db.ContactUs.Add(obj);
            _db.SaveChanges();
            return RedirectToAction("Contact");
        }

        public IActionResult Faq()
        {
            return View();
        }

        public IActionResult Prices()
        {
            return View();
        }

        public IActionResult CustomerSignup()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CustomerSignup(User obj)
        {
            if (ModelState.IsValid)
            {
                obj.UserTypeId = 3;
                obj.IsRegisteredUser = false;
                obj.WorksWithPets = false;
                obj.CreatedDate = DateTime.Now;
                obj.ModifiedDate = DateTime.Now;
                obj.ModifiedBy = 0;
                obj.IsApproved = false;
                obj.IsActive = true;
                obj.IsDeleted = false;
                _db.Users.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult BecomeAPro()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BecomeAPro(User obj)
        {
            if (ModelState.IsValid)
            {
                obj.UserTypeId = 2;
                obj.IsRegisteredUser = false;
                obj.WorksWithPets = false;
                obj.CreatedDate = DateTime.Now;
                obj.ModifiedDate = DateTime.Now;
                obj.ModifiedBy = 0;
                obj.IsApproved = false;
                obj.IsActive = true;
                obj.IsDeleted = false;
                _db.Users.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("BecomeAPro");
            }
            
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("User_Session");
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
