using Helperland.Data;
using Helperland.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Helperland.Controllers
{
    public class HomeController : Controller
    {

        private readonly HelperlandContext _db;
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly ILogger<HomeController> _logger;

        // Dependency Injection
        public HomeController(ILogger<HomeController> logger, HelperlandContext db, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _db = db;
            this.hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {

            if (HttpContext.Session.GetInt32("UserID_Session") != null) 
            {
                return View();
            }
            ViewBag.IsHomePage = true;
            return View();
        }

        [HttpPost]
        public IActionResult Login(User obj)
        {
            User user = _db.Users.Where(x => x.Email == obj.Email && x.Password == obj.Password).SingleOrDefault();
            if (user != null)
            {
                var username = user.FirstName + " " + user.LastName;
                HttpContext.Session.SetInt32("UserID_Session", user.UserId);
                HttpContext.Session.SetString("UserName_Session", username);
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Msg4Popup"] = "Username or Password is Incorrect. Please try again";
                return RedirectToAction("Index");
            }
        }


        //ForgotPassword
        [HttpPost]
        public IActionResult ForgotPassword(User e)
        {
            var _objuserdetail = (from i in _db.Users where i.Email == e.Email select i).SingleOrDefault();
            string BaseUrl = string.Format("{0}://{1}", HttpContext.Request.Scheme, HttpContext.Request.Host);
            var ResetUrl = $"{BaseUrl}/Home/ResetPassword/" + _objuserdetail.UserId;

            if (_objuserdetail != null)
            {
                MailMessage msg = new MailMessage();
                msg.To.Add(_objuserdetail.Email);
                msg.From = new MailAddress("getpaswordback@gmail.com");
                msg.Subject = "Reset Password - Helperland";
                msg.Body = "Hello " + _objuserdetail.FirstName + ",\n\nYour can reset your password by clicking the link below \n" + ResetUrl + "\nThank you for visiting Helperland \n\nRegards,\nHelperland Team";

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

        [HttpGet]
        public IActionResult ResetPassword(int? id)
        
        {
            ViewData["ResetUser"] = id;
            User user = _db.Users.Where(x => x.UserId == id).FirstOrDefault();
            return View(user);
        }

        [HttpPost]
        public IActionResult ResetPassword(User obj)
        {
            if (ModelState.IsValid)
            {
                User user = _db.Users.Where(x => x == obj).FirstOrDefault();
                user.Password = obj.Password;
                user.ModifiedBy = obj.UserId;
                user.ModifiedDate = DateTime.Now; 
                _db.Users.Update(user);
                _db.SaveChanges();
                ViewBag.Msg = "Password has been changed successfully";
            }
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

            string uniqueFileName = null;
            if (obj.UploadFile != null)
            {
                string folderPath = Path.Combine(hostEnvironment.WebRootPath, "Uploads/ContactUsAttachments");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + obj.UploadFile.FileName;
                string filePath = Path.Combine(folderPath, uniqueFileName);
                obj.UploadFile.CopyTo(new FileStream(filePath, FileMode.Create));
            }

            obj.UploadFileName = uniqueFileName;
            obj.Name = HttpContext.Request.Form["FName"] + " " + HttpContext.Request.Form["LName"];
            obj.CreatedOn = DateTime.Now;
            _db.ContactUs.Add(obj);
            _db.SaveChanges();
            TempData["Msg"] = "Response has been recorded";
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
                ViewBag.Msg = "Account created";
                return View();
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

        public IActionResult BookService()
        {
            var userID = HttpContext.Session.GetInt32("UserID_Session");
            var userName = HttpContext.Session.GetInt32("UserName_Session");

            if (userID != null)
            {
                return View();
            }
            TempData["Msg4Popup"] = "You have to login to Book a Service.";
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public string BookServiceRequest([FromBody] ServiceRequest book)
        {
            int userID = (int)HttpContext.Session.GetInt32("UserID_Session");
            UserAddress address = _db.UserAddresses.Where(x => x.AddressId == book.AddressId).SingleOrDefault();
            book.UserId = userID;
            book.ServiceId = 123455;
            book.PaymentDue = true;
            book.CreatedDate = DateTime.Now;
            book.ModifiedDate = DateTime.Now;
            book.ModifiedBy = userID;
            book.Distance = 10;

            _db.ServiceRequests.Add(book);
            _db.SaveChanges();

            ServiceRequestAddress requestAddress = new ServiceRequestAddress();
            requestAddress.ServiceRequestId = book.ServiceRequestId;
            requestAddress.AddressLine1 = address.AddressLine1;
            requestAddress.City = address.City;
            requestAddress.PostalCode = address.PostalCode;
            if(address.AddressLine2 != null)
            {
                requestAddress.AddressLine2 = address.AddressLine2;
            }
            if (address.State != null)
            {
                requestAddress.State = address.State;
            }
            if (address.Email != null)
            {
                requestAddress.Email = address.Email;
            }
            if (address.Mobile != null)
            {
                requestAddress.Mobile = address.Mobile;
            }
            _db.ServiceRequestAddresses.Add(requestAddress);
            _db.SaveChanges();

            return "true";
        }

        public string ValidatePostalCode(string postalcode)
        {

            var PostalCode = _db.Users.Where(x => x.ZipCode == postalcode).SingleOrDefault();
            string IsValidated;
            if (PostalCode != null)
            {
                IsValidated = "true";
            }
            else
            {
                IsValidated = "false";
            }
            return IsValidated;

        }

        public IActionResult BookServiceAddress()
        {
            System.Threading.Thread.Sleep(2000);
            var UserID = HttpContext.Session.GetInt32("UserID_Session");
            List<UserAddress> allAddress = new List<UserAddress>();
            allAddress = _db.UserAddresses.Where(x => x.UserId == UserID).ToList();

            return View(allAddress);
        }

        public string AddAddress([FromBody] UserAddress address)
        {
            int UserID = (int)HttpContext.Session.GetInt32("UserID_Session");
            User user = _db.Users.Where(x => x.UserId == UserID).SingleOrDefault();
            if(address == null)
            {
                return "false";
            }
            else
            {
                address.UserId = UserID;
                address.Email = user.Email;
                address.IsDefault = false;
                address.IsDeleted = false;
                _db.UserAddresses.Add(address);
                _db.SaveChanges();
                return "true";
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserID_Session");
            HttpContext.Session.Remove("UserName_Session");
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
