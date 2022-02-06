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
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity;

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
        public IActionResult ForgotPassword(User e)
        {
            var _objuserdetail = (from i in _db.Users where i.Email == e.Email select i).SingleOrDefault();

            string BaseUrl = string.Format("{0}://{1}", HttpContext.Request.Scheme, HttpContext.Request.Host);
            var ResetUrl = $"{BaseUrl}/Home/ResetPassword";

            if (_objuserdetail != null)
            {
                MailMessage msg = new MailMessage();
                msg.To.Add(_objuserdetail.Email);
                msg.From = new MailAddress("getpaswordback@gmail.com");
                msg.Subject = "Reset Password - Helperland";
                msg.Body =  "Hello "+ _objuserdetail.FirstName + ",\n\nYour can reset your password by clicking the link below \n" + ResetUrl + "\nThank you for visiting Helperland \n\nRegards,\nHelperland Team";

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                smtp.Port = 587;
                

                NetworkCredential NC = new NetworkCredential("getpaswordback@gmail.com", "Demo@123");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NC;
                smtp.Send(msg);
                ViewData["MessageSuccess"] = "Mail sent successfully";
                return RedirectToAction("Index");
            }
            else
            {
                ViewData["MessageFail"] = "Mail not sent";
                return RedirectToAction("Index");
            }
        }

        public IActionResult ResetPassword()
        {
            return View();
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
