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

        //Login
        [HttpPost]
        public IActionResult Login(User obj)
        {
            User user = _db.Users.Where(x => x.Email == obj.Email && x.Password == obj.Password).SingleOrDefault();
            if (user != null)
            {
                if (user.Status == 1)
                {
                    var username = user.FirstName + " " + user.LastName;
                    HttpContext.Session.SetInt32("UserID_Session", user.UserId);
                    HttpContext.Session.SetString("UserName_Session", username);
                    HttpContext.Session.SetInt32("UserType_Session", user.UserTypeId);
                    int usertype = user.UserTypeId;

                    switch (usertype)
                    {
                        case 1:
                            return RedirectToAction("AdminServiceRequests");
                        case 2:
                            return RedirectToAction("SPDashboard");
                        case 3:
                            return RedirectToAction("CustomerDashboard");
                        default:
                            return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["Msg4Popup"] = "You are not Approved...You can login after Admin Authentication";
                    return RedirectToAction("Index");
                }

            }
            else
            {
                TempData["Msg4Popup"] = "Username or Password is Incorrect. Please try again";
                return RedirectToAction("Index");
            }
        }

        //Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserID_Session");
            HttpContext.Session.Remove("UserName_Session");
            HttpContext.Session.Remove("UserType_Session");
            return RedirectToAction("Index");
        }

        //====================================
        //          Public-Pages
        //====================================

        //Index
        public IActionResult Index()
        {

            
            ViewBag.IsHomePage = true;
            return View();
        }

        //About
        public IActionResult About()
        {
            return View();
        }

        //Contact Us
        public IActionResult Contact()
        {
            return View();
        }

        //Contact Us (POST)
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

        //FAQs
        public IActionResult Faq()
        {
            return View();
        }

        //Prices
        public IActionResult Prices()
        {
            return View();
        }

        //ForgotPassword
        [HttpPost]
        public IActionResult ForgotPassword(User e)
        {
            var _objuserdetail = (from i in _db.Users where i.Email == e.Email select i).SingleOrDefault();

            if (_objuserdetail != null)
            {
                string BaseUrl = string.Format("{0}://{1}", HttpContext.Request.Scheme, HttpContext.Request.Host);
                var ResetUrl = $"{BaseUrl}/Home/ResetPassword/" + _objuserdetail.UserId;
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
                TempData["Msg4Popup"] = "Mail sent successfully";
                HttpContext.Session.SetInt32("ResetPassword",_objuserdetail.UserId);
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Msg4Popup"] = "Account not found based on this Email. Please try again with valid Email Id";
                return RedirectToAction("Index");
            }
        }

        //Reset Password
        [HttpGet]
        public IActionResult ResetPassword(int? id)
        {
            var userid = HttpContext.Session.GetInt32("ResetPassword");
            if (userid != null)
            {
                ViewData["ResetUser"] = id;
                User user = _db.Users.Where(x => x.UserId == id).FirstOrDefault();
                return View(user);
            }
            TempData["Msg4Popup"] = "Authorization Denied!";
            return RedirectToAction("Index");
        }

        //Reset Password (POST)
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
                TempData["Msg4Popup"] = "Password has been changed successfully";
                HttpContext.Session.Remove("ResetPassword");
                return RedirectToAction("Index");
            }
            return View();
        }

        //====================================
        //          Customer-Pages
        //====================================

        //Customer SignUp
        public IActionResult CustomerSignup()
        {
            return View();
        }

        //Customer SignUp (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CustomerSignup(User obj)
        {
            if (ModelState.IsValid)
            {
                if (!_db.Users.Any(x => x.Email == obj.Email))
                {
                    obj.UserTypeId = 3;
                    obj.IsRegisteredUser = false;
                    obj.WorksWithPets = false;
                    obj.CreatedDate = DateTime.Now;
                    obj.ModifiedDate = DateTime.Now;
                    obj.ModifiedBy = 0;
                    obj.Status = 1;
                    obj.IsApproved = false;
                    obj.IsActive = true;
                    obj.IsDeleted = false;
                    _db.Users.Add(obj);
                    _db.SaveChanges();
                    TempData["SuccessMsg"] = "Account created";
                    return RedirectToAction("CustomerSignup");
                }
                else
                {
                    TempData["ErrorMsg"] = "Email already exists. Try using different email id.";
                }
            }
            return View();
        }

        //Customer Dashboard
        public IActionResult CustomerDashboard()
        {
            int userid = (int)HttpContext.Session.GetInt32("UserID_Session");
            User cust = _db.Users.Where(x => x.UserId == userid).FirstOrDefault();
            if (cust != null)
            {
                if (cust.UserTypeId == 3)
                {
                    var query = from sr in _db.ServiceRequests
                                join user in _db.Users on sr.ServiceProviderId equals user.UserId into abc
                                from user in abc.DefaultIfEmpty()
                                join r in _db.Ratings on sr.ServiceRequestId equals r.ServiceRequestId into x
                                from rate in x.DefaultIfEmpty()
                                where sr.UserId == userid && sr.Status == 0 || sr.Status == 1
                                select new CustomModel
                                {
                                    ServiceRequestId = sr.ServiceRequestId,
                                    ServiceId = sr.ServiceId,
                                    ServiceStartDate = sr.ServiceStartDate,
                                    Status = sr.Status,
                                    TotalCost = sr.TotalCost,
                                    Ratings = rate == null? 0 : rate.Ratings,
                                    FirstName = user.FirstName,
                                    LastName = user.LastName,
                                    UserProfilePicture = user.UserProfilePicture
                                };

                    return View(query);
                }
                else
                {
                    TempData["ErrorMsg"] = "You don't have customer access. Please Contact Admin for further details.";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["ErrorMsg"] = "You have to login to access the page.";
                return RedirectToAction("Index");
            }
        }

        //Customer Dashboard Modal View
        public IActionResult SRDashModal(int id = 5)
        {
            ServiceRequestExtra[] obj = _db.ServiceRequestExtras.Where(x => x.ServiceRequestId == id).ToArray();
            List<string> extras = new List<string>();
            foreach (var item in obj)
            {
                switch (item.ServiceExtraId)
                {
                    case 1:
                        extras.Add("Inside Cabinet");
                        break;
                    case 2:
                        extras.Add("Inside Fridge");
                        break;
                    case 3:
                        extras.Add("Inside Oven");
                        break;
                    case 4:
                        extras.Add("Laundry Wash & Dry");
                        break;
                    case 5:
                        extras.Add("Interior Windows");
                        break;
                    default:
                        break;
                }
            }
            var query = (from SR in _db.ServiceRequests
                        join SRaddress in _db.ServiceRequestAddresses on SR.ServiceRequestId equals SRaddress.ServiceRequestId
                        //join SRextra in _db.ServiceRequestExtras on SRaddress.ServiceRequestId equals SRextra.ServiceRequestId
                        where SR.ServiceRequestId == id
                        select new CustomModel
                        {
                            ServiceRequestId = SR.ServiceRequestId,
                            ServiceId = SR.ServiceId,
                            ServiceStartDate = SR.ServiceStartDate,
                            ServiceHours = SR.ServiceHours,
                            Comments = SR.Comments,
                            HasPets = SR.HasPets,
                            TotalCost = SR.TotalCost,

                            AddressLine1 = SRaddress.AddressLine1,
                            AddressLine2 = SRaddress.AddressLine2,
                            City = SRaddress.City,
                            State = SRaddress.State,
                            PostalCode = SRaddress.PostalCode,
                            Mobile = SRaddress.Mobile,

                            ServiceExtraId = String.Join(", ", extras)
                        }).Single();

            return View(query);
        }

        //Customer Dashboard Reschedule Request
        public bool CustomerDashboardRescheduleRequest([FromBody] ServiceRequest sr)
        {
            ServiceRequest obj = _db.ServiceRequests.Where(x => x.ServiceRequestId == sr.ServiceRequestId).FirstOrDefault();
            var output = DateTime.Compare(DateTime.Now, DateTime.Parse(sr.Date));
            if (output == -1)
            {
                obj.ServiceStartDate = DateTime.Parse(sr.Date);
                _db.ServiceRequests.Update(obj);
                _db.SaveChanges();
                return true;
            }
            return false;
        }

        //Customer Dashboard Cancel Request
        public bool CustomerDashboardCancelRequest([FromBody] ServiceRequest sr)
        {
            ServiceRequest obj = _db.ServiceRequests.Where(x => x.ServiceRequestId == sr.ServiceRequestId).FirstOrDefault();
            obj.Status = 3;
            obj.Comments = sr.Comments;
            _db.ServiceRequests.Update(obj);
            _db.SaveChanges();
            return true;
        }

        //Customer Settings Tab
        public IActionResult CustomerSettings()
        {
            int userid = (int)HttpContext.Session.GetInt32("UserID_Session");
            User user = _db.Users.Where(x => x.UserId == userid).FirstOrDefault();
            if (user != null)
            {
                if (user.UserTypeId == 3)
                {
                    return View(user);
                }
                else
                {
                    TempData["ErrorMsg"] = "You don't have customer access. Please Contact Admin for further details.";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["ErrorMsg"] = "You have to login to access the page.";
                return RedirectToAction("Index");
            }
        }

        //Customer Settings Tab (POST)
        [HttpPost]
        public IActionResult CustomerSettings(User user)
        {
            User obj = _db.Users.Where(x => x.UserId == user.UserId).FirstOrDefault();
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
            if (ModelState.IsValid)
            {
                obj.FirstName = user.FirstName;
                obj.LastName = user.LastName;
                obj.Mobile = user.Mobile;
                obj.DateOfBirth = user.DateOfBirth;
                _db.Users.Update(obj);
                _db.SaveChanges();

                var username = user.FirstName + " " + user.LastName;
                HttpContext.Session.SetString("UserName_Session", username);

                TempData["Msg4Popup"] = "Profile Updated Successfully";
            }

            return View(obj);
        }

        //Customer Settings Password Change (POST)
        public bool CustomerPasswordChange([FromBody] User passwords)
        {
            int userid = (int)HttpContext.Session.GetInt32("UserID_Session");
            User user = _db.Users.Where(x => x.UserId == userid).FirstOrDefault();

            if (user.Password != passwords.Password)
            {
                return false;
            }
            else
            {
                try
                {
                    user.Password = passwords.ConfirmPassword;
                    _db.Users.Update(user);
                    _db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        //Customer Address in Settings Tab
        public IActionResult CustomerAddress()
        {
            int userid = (int)HttpContext.Session.GetInt32("UserID_Session");
            List<UserAddress> obj = _db.UserAddresses.Where(x => x.UserId == userid).ToList();
            return View(obj);
        }

        //Customer Address Add or Edit
        public IActionResult CustomerAddressAddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View();
            }
            else
            {
                UserAddress obj = _db.UserAddresses.Where(x => x.AddressId == id).FirstOrDefault();
                return View(obj);
            }
        }

        //Customer Address Add or Edit (POST)
        [HttpPost]
        public string CustomerAddressAddOrEdit(UserAddress obj)
        {
            int userid = (int)HttpContext.Session.GetInt32("UserID_Session");
            User user = _db.Users.Where(x => x.UserId == userid).FirstOrDefault();
            if (obj.AddressId == 0)
            {
                obj.UserId = userid;
                obj.Email = user.Email;
                obj.IsDefault = false;
                obj.IsDeleted = false;
                _db.UserAddresses.Add(obj);
                _db.SaveChanges();
            }
            else
            {
                _db.UserAddresses.Update(obj);
                _db.SaveChanges();
            }
            return "true";
        }

        //Customer Address Delete (POST)
        [HttpPost]
        public string CustomerAddressDelete(int id)
        {
            UserAddress obj = _db.UserAddresses.Where(x => x.AddressId == id).FirstOrDefault();
            _db.UserAddresses.Remove(obj);
            _db.SaveChanges();
            return "true";
        }

        //Customer Service History Tab
        public IActionResult CustomerServiceHistory()
        {
            int userid = (int)HttpContext.Session.GetInt32("UserID_Session");
            User cust = _db.Users.Where(x => x.UserId == userid).FirstOrDefault();

            if (cust != null)
            {
                if (cust.UserTypeId == 3)
                {
                    var query = from sr in _db.ServiceRequests
                                join user in _db.Users on sr.ServiceProviderId equals user.UserId into x
                                from user in x.DefaultIfEmpty()
                                join r in _db.Ratings on sr.ServiceRequestId equals r.ServiceRequestId into abc
                                from rate in abc.DefaultIfEmpty()
                                where sr.UserId == userid && sr.Status == 2 || sr.Status == 3
                                select new CustomModel
                                {
                                    ServiceStartDate = sr.ServiceStartDate,
                                    Ratings = rate == null ? 0 : rate.Ratings,
                                    TotalCost = sr.TotalCost,
                                    Status = sr.Status,
                                    ServiceProviderId = sr.ServiceProviderId,
                                    ServiceRequestId = sr.ServiceRequestId,
                                    FirstName = user == null ? "" : user.FirstName,
                                    LastName = user == null ? "" : user.LastName
                                };

                    return View(query);
                }
                else
                {
                    TempData["ErrorMsg"] = "You don't have customer access. Please Contact Admin for further details.";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["ErrorMsg"] = "You have to login to access the page.";
                return RedirectToAction("Index");
            }

        }

        public IActionResult _RateSPModal(int srid)
        {

            int userid = (int)HttpContext.Session.GetInt32("UserID_Session");

            var query = (from user in _db.Users
                        join sr in _db.ServiceRequests on user.UserId equals sr.ServiceProviderId 
                        join r in _db.Ratings on sr.ServiceRequestId equals r.ServiceRequestId into x
                        from rate in x.DefaultIfEmpty()
                         where rate == null ? (sr.ServiceRequestId == srid) : (sr.ServiceRequestId == srid && rate.RatingFrom == userid)
                        select new CustomModel
                        {
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Ratings = rate == null? 0 : rate.Ratings,
                            OnTimeArrival = rate == null ? 0 : rate.OnTimeArrival,
                            Friendly = rate == null ? 0 : rate.Friendly,
                            QualityOfService = rate == null ? 0 : rate.QualityOfService,
                            Comments = rate == null ? "" :rate.Comments
                        }).Single();

            return PartialView(query);
        }

        public string CustomerRateSP([FromBody] Rating rate)
        {
            int userid = (int)HttpContext.Session.GetInt32("UserID_Session");
           
            Rating r = _db.Ratings.Where(x => x.ServiceRequestId == rate.ServiceRequestId).FirstOrDefault();
            
            if (r != null)
            {
                r.Ratings = rate.Ratings;
                r.Comments = rate.Comments;
                r.Friendly = rate.Friendly;
                r.OnTimeArrival = rate.OnTimeArrival;
                r.QualityOfService = rate.QualityOfService;
                r.RatingDate = DateTime.Now;
                _db.Ratings.Update(r);
            }
            else
            {
                rate.RatingFrom = userid;
                rate.RatingDate = DateTime.Now;
                _db.Ratings.Add(rate);
            }
            _db.SaveChanges();
            return "true";
        }

        //Customer Service History Tab
        public IActionResult CustomerFavouritePros()
        {
            int userid = (int)HttpContext.Session.GetInt32("UserID_Session");
            User cust = _db.Users.Where(x => x.UserId == userid).FirstOrDefault();

            if (cust != null)
            {
                if (cust.UserTypeId == 3)
                {
                    return View();
                }
                else
                {
                    TempData["ErrorMsg"] = "You don't have customer access. Please Contact Admin for further details.";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["ErrorMsg"] = "You have to login to access the page.";
                return RedirectToAction("Index");
            }

        }

        //====================================
        //           Book Service
        //====================================
        public IActionResult BookService()
        {
            var userid = HttpContext.Session.GetInt32("UserID_Session");
            User cust = _db.Users.Where(x => x.UserId == userid).FirstOrDefault();

            if (cust != null)
            {
                if (cust.UserTypeId == 3)
                {
                    return View();
                }
                else
                {
                    TempData["ErrorMsg"] = "You don't have customer access. Please Contact Admin for further details.";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["Msg4Popup"] = "You have to login to Book a Service.";
                return RedirectToAction("Index");
            }
        }

        //Book Service (POST)
        [HttpPost]
        public string BookServiceRequest([FromBody] ServiceRequest book)
        {
            int userID = (int)HttpContext.Session.GetInt32("UserID_Session");
            UserAddress address = _db.UserAddresses.Where(x => x.AddressId == book.AddressId).SingleOrDefault();
            book.UserId = userID;
            book.ServiceId = 1000;
            book.ServiceStartDate = DateTime.Parse(book.Date);
            book.PaymentDue = true;
            book.CreatedDate = DateTime.Now;
            book.ModifiedDate = DateTime.Now;
            book.ModifiedBy = userID;
            book.Distance = 10;
            book.Status = 0;                // 0:New  1:Pending  2:Completed  3:Cancelled
            
            _db.ServiceRequests.Add(book);
            _db.SaveChanges();

            book.ServiceId = 1000 + book.ServiceRequestId;
            _db.ServiceRequests.Update(book);
            _db.SaveChanges();

            ServiceRequestAddress requestAddress = new ServiceRequestAddress();
            requestAddress.ServiceRequestId = book.ServiceRequestId;
            requestAddress.AddressLine1 = address.AddressLine1;
            requestAddress.AddressLine2 = address.AddressLine2;
            requestAddress.City = address.City;
            requestAddress.State = address.State;
            requestAddress.PostalCode = address.PostalCode;
            requestAddress.Email = address.Email;
            requestAddress.Mobile = address.Mobile;

            _db.ServiceRequestAddresses.Add(requestAddress);
            _db.SaveChanges();

            for (int i = 0; i < book.Extras.Length; i++)
            {
                if (book.Extras[i] == true)
                {
                    ServiceRequestExtra extras = new ServiceRequestExtra();
                    extras.ServiceExtraId = i+1;
                    extras.ServiceRequestId = book.ServiceRequestId;
                    _db.ServiceRequestExtras.Add(extras);
                }
            }
            _db.SaveChanges();



            MailMessage msg = new MailMessage();
            
            msg.From = new MailAddress("getpaswordback@gmail.com");
            msg.Subject = "New Service Request - Helperland";
            msg.Body = "Hello Service Provider,\n\nNew Service Request is Available in your area\n \n Visit Helperland for further Information.  \n\nRegards,\nHelperland Team";

            var emailList = from u in _db.Users
                            where u.UserTypeId == 2
                            select u.Email;

            foreach (var i in emailList)
            {
                msg.To.Add(new MailAddress(i));
            }

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            smtp.Port = 587;


            NetworkCredential NC = new NetworkCredential("getpaswordback@gmail.com", "Demo@123");
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = NC;
            smtp.Send(msg);

            return book.ServiceId.ToString();
        }

        //Validate Zip Code (Tab-1)
        public string ValidatePostalCode(string postalcode)
        {
            if (postalcode == null)
            {
                return "false";
            }
            var PostalCode = _db.Users.Any(x => x.ZipCode == postalcode);
            string IsValidated;
            if (PostalCode)
            {
                IsValidated = "true";
            }
            else
            {
                IsValidated = "false";
            }
            return IsValidated;

        }

        //Fetch User Address Page (Tab-3)
        public IActionResult BookServiceAddress()
        {
            System.Threading.Thread.Sleep(2000);
            var UserID = HttpContext.Session.GetInt32("UserID_Session");
            List<UserAddress> allAddress = new List<UserAddress>();
            allAddress = _db.UserAddresses.Where(x => x.UserId == UserID).ToList();

            return View(allAddress);
        }

        //Add Address (Tab-3)
        public string AddAddress([FromBody] UserAddress address)
        {
            int UserID = (int)HttpContext.Session.GetInt32("UserID_Session");
            User user = _db.Users.Where(x => x.UserId == UserID).SingleOrDefault();
            if (address == null)
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

        //====================================
        //      Service Provider-Pages
        //====================================
        
        //Service Provider SignUp
        public IActionResult BecomeAPro()
        {
            return View();
        }

        //Service Provider SignUp (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BecomeAPro(User obj)
        {
            if (ModelState.IsValid)
            {
                if (!_db.Users.Any(x => x.Email == obj.Email))
                {
                    obj.UserTypeId = 2;
                    obj.IsRegisteredUser = false;
                    obj.WorksWithPets = false;
                    obj.CreatedDate = DateTime.Now;
                    obj.ModifiedDate = DateTime.Now;
                    obj.ModifiedBy = 0;
                    obj.Status = 0;
                    obj.IsApproved = false;
                    obj.IsActive = true;
                    obj.IsDeleted = false;
                    _db.Users.Add(obj);
                    _db.SaveChanges();
                    TempData["SuccessMsg"] = "Account Created Successfully";
                    return RedirectToAction("BecomeAPro");
                }
                else
                {
                    TempData["ErrorMsg"] = "Email Already Exists. Try using different email id";
                }
            }
            
            return View();
        }

        public IActionResult SPDashboard()
        {
            int id = (int)HttpContext.Session.GetInt32("UserID_Session");
            User sp = _db.Users.Where(x => x.UserId == id).FirstOrDefault();

            if (sp != null)
            {
                if (sp.UserTypeId == 2)
                {
                    var query = from sr in _db.ServiceRequests
                                join sraddress in _db.ServiceRequestAddresses on sr.ServiceRequestId equals sraddress.ServiceRequestId into srad
                                from Sra in srad.DefaultIfEmpty()
                                join user in _db.Users on sr.UserId equals user.UserId
                                join f in _db.FavoriteAndBlockeds on sr.UserId equals f.TargetUserId into fav
                                from f in fav.DefaultIfEmpty()
                                where sp.ZipCode == sr.ZipCode && sr.Status == 0 && (f.UserId == id || f.UserId == null) && (f.IsBlocked != true || f.IsBlocked == null)
                                select new CustomModel
                                {
                                    ServiceRequestId = sr.ServiceRequestId,
                                    ServiceId = sr.ServiceId,
                                    ServiceStartDate = sr.ServiceStartDate,
                                    TotalCost = sr.TotalCost,

                                    FirstName = user.FirstName,
                                    LastName = user.LastName,

                                    AddressLine1 = Sra == null ? "" : Sra.AddressLine1,
                                    AddressLine2 = Sra == null ? "" : Sra.AddressLine2,
                                    PostalCode = Sra == null ? "" : Sra.PostalCode,
                                    City = Sra == null ? "" : Sra.City
                                };
                    return View(query);

                }
                else
                {
                    TempData["ErrorMsg"] = "You don't have Service Provider access. Please Contact Admin for further details.";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["ErrorMsg"] = "You have to login to access the page.";
                return RedirectToAction("Index");
            }

        }

        public IActionResult _SPDashModal(int id)
        {
            ServiceRequestExtra[] obj = _db.ServiceRequestExtras.Where(x => x.ServiceRequestId == id).ToArray();
            List<string> extras = new List<string>();
            foreach (var item in obj)
            {
                switch (item.ServiceExtraId)
                {
                    case 1:
                        extras.Add("Inside Cabinet");
                        break;
                    case 2:
                        extras.Add("Inside Fridge");
                        break;
                    case 3:
                        extras.Add("Inside Oven");
                        break;
                    case 4:
                        extras.Add("Laundry Wash & Dry");
                        break;
                    case 5:
                        extras.Add("Interior Windows");
                        break;
                    default:
                        break;
                }
            }
            
            var query = (from SR in _db.ServiceRequests
                         join SRaddress in _db.ServiceRequestAddresses on SR.ServiceRequestId equals SRaddress.ServiceRequestId into srad
                         from SRa in srad.DefaultIfEmpty()
                         join user in _db.Users on SR.UserId equals user.UserId
                         where SR.ServiceRequestId == id
                         select new CustomModel
                         {
                             ServiceRequestId = SR.ServiceRequestId,
                             ServiceId = SR.ServiceId,
                             ServiceStartDate = SR.ServiceStartDate,
                             ServiceHours = SR.ServiceHours,
                             Comments = SR.Comments,
                             HasPets = SR.HasPets,
                             TotalCost = SR.TotalCost,

                             FirstName = user.FirstName,
                             LastName = user.LastName,

                             AddressLine1 = SRa == null ? "" : SRa.AddressLine1,
                             AddressLine2 = SRa == null ? "" : SRa.AddressLine2,
                             City = SRa == null ? "" : SRa.City,
                             State = SRa == null ? "" : SRa.State,
                             PostalCode = SRa == null ? "" : SRa.PostalCode,
                             Mobile = SRa == null ? "" : SRa.Mobile,

                             ServiceExtraId = String.Join(", ", extras)
                         }).Single();

            return PartialView(query);
        }

        //Accept Service
        public IActionResult AcceptServiceRequest(int id)
        {
            int userid = (int)HttpContext.Session.GetInt32("UserID_Session");

            ServiceRequest sr = _db.ServiceRequests.Where(x => x.ServiceRequestId == id).FirstOrDefault();
            sr.Status = 1;
            sr.ServiceProviderId = userid;
            sr.SpacceptedDate = DateTime.Now;
            _db.ServiceRequests.Update(sr);
            _db.SaveChanges();
            return RedirectToAction("SPDashboard");
        }

        //Service Provider Settings
        public IActionResult SPSettings()
        {
            int id = (int)HttpContext.Session.GetInt32("UserID_Session");
            User sp = _db.Users.Where(x => x.UserId == id).FirstOrDefault();

            if (sp != null)
            {
                if (sp.UserTypeId == 2)
                {
                    var query = (from u in _db.Users
                                join ad in _db.UserAddresses on u.UserId equals ad.UserId into address
                                from ad in address.DefaultIfEmpty()
                                where u.UserId == id
                                select new SPSettings
                                {
                                    UserId = u.UserId,
                                    FirstName = u.FirstName,
                                    LastName = u.LastName,
                                    Email = u.Email,
                                    Mobile = u.Mobile,
                                    DateOfBirth = u.DateOfBirth,
                                    Gender = u.Gender,
                                    UserProfilePicture = u.UserProfilePicture,

                                    AddressId = ad == null ? 0 : ad.AddressId,
                                    AddressLine1 = ad == null ? "" : ad.AddressLine1,
                                    AddressLine2 = ad == null ? "" : ad.AddressLine2,
                                    City = ad == null ? "" : ad.City,
                                    PostalCode = ad == null ? "" : ad.PostalCode 
                                }).Single();
                    return View(query);
                }
                else
                {
                    TempData["ErrorMsg"] = "You don't have Service Provider access. Please Contact Admin for further details.";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["ErrorMsg"] = "You have to login to access the page.";
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        public IActionResult SPSettings(SPSettings sp)
        {
            
            if (sp.AddressLine1 == null && sp.AddressLine2 == null && sp.City == null && sp.PostalCode == null && sp.AddressId == 0)
            {
                ModelState.Remove("AddressLine1");
                ModelState.Remove("City");
                ModelState.Remove("PostalCode");
                
            }

            if (ModelState.IsValid)
            {
                int id = (int)HttpContext.Session.GetInt32("UserID_Session");
                User user = _db.Users.Where(x => x.UserId == id).FirstOrDefault();
                user.FirstName = sp.FirstName;
                user.LastName = sp.LastName;
                user.Mobile = sp.Mobile;
                user.Gender = sp.Gender;
                user.UserProfilePicture = sp.UserProfilePicture;
                user.DateOfBirth = sp.DateOfBirth;
                user.ZipCode = sp.PostalCode;
                _db.Users.Update(user);
                UserAddress useraddress = new UserAddress()
                {
                    UserId = id,
                    AddressLine1 = sp.AddressLine1,
                    AddressLine2 = sp.AddressLine2,
                    City = sp.City,
                    PostalCode = sp.PostalCode,
                    Mobile = user.Mobile,
                    Email = user.Email
                };
                if (sp.AddressId == 0)
                {
                    if (useraddress.AddressLine1 != null || useraddress.AddressLine2 != null || useraddress.City != null || useraddress.PostalCode != null)
                    {
                        _db.UserAddresses.Add(useraddress);
                    }
                }
                else
                {
                    useraddress.AddressId = sp.AddressId;
                    _db.UserAddresses.Update(useraddress);
                }

                var username = user.FirstName + " " + user.LastName;
                HttpContext.Session.SetString("UserName_Session", username);

                TempData["Msg4Popup"] = "Profile Updated Successfully";

                _db.SaveChanges();
            }

            return View();
        }

        public IActionResult SPUpcoming()
        {
            int id = (int)HttpContext.Session.GetInt32("UserID_Session");
            User sp = _db.Users.Where(x => x.UserId == id).FirstOrDefault();

            if (sp != null)
            {
                if (sp.UserTypeId == 2)
                {
                    var query = from sr in _db.ServiceRequests
                                join sraddress in _db.ServiceRequestAddresses on sr.ServiceRequestId equals sraddress.ServiceRequestId into srad
                                from Sra in srad.DefaultIfEmpty()
                                join user in _db.Users on sr.UserId equals user.UserId
                                where sr.ServiceProviderId == id && sr.Status == 1 //pending
                                select new CustomModel
                                {
                                    ServiceRequestId = sr.ServiceRequestId,
                                    ServiceId = sr.ServiceId,
                                    ServiceStartDate = sr.ServiceStartDate,
                                    TotalCost = sr.TotalCost,

                                    FirstName = user.FirstName,
                                    LastName = user.LastName,

                                    AddressLine1 = Sra == null ? "" : Sra.AddressLine1,
                                    AddressLine2 = Sra == null ? "" : Sra.AddressLine2,
                                    PostalCode = Sra == null ? "" : Sra.PostalCode,
                                    City = Sra == null ? "" : Sra.City
                                };
                    return View(query);
                }
                else
                {
                    TempData["ErrorMsg"] = "You don't have Service Provider access. Please Contact Admin for further details.";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["ErrorMsg"] = "You have to login to access the page.";
                return RedirectToAction("Index");
            }
        }

        public IActionResult _SPUpcomingModal(int id)
        {
            ServiceRequestExtra[] obj = _db.ServiceRequestExtras.Where(x => x.ServiceRequestId == id).ToArray();
            List<string> extras = new List<string>();
            foreach (var item in obj)
            {
                switch (item.ServiceExtraId)
                {
                    case 1:
                        extras.Add("Inside Cabinet");
                        break;
                    case 2:
                        extras.Add("Inside Fridge");
                        break;
                    case 3:
                        extras.Add("Inside Oven");
                        break;
                    case 4:
                        extras.Add("Laundry Wash & Dry");
                        break;
                    case 5:
                        extras.Add("Interior Windows");
                        break;
                    default:
                        break;
                }
            }

            var query = (from SR in _db.ServiceRequests
                         join SRaddress in _db.ServiceRequestAddresses on SR.ServiceRequestId equals SRaddress.ServiceRequestId into srad
                         from SRa in srad.DefaultIfEmpty()
                         join user in _db.Users on SR.UserId equals user.UserId
                         where SR.ServiceRequestId == id
                         select new CustomModel
                         {
                             ServiceRequestId = SR.ServiceRequestId,
                             ServiceId = SR.ServiceId,
                             ServiceStartDate = SR.ServiceStartDate,
                             ServiceHours = SR.ServiceHours,
                             Comments = SR.Comments,
                             HasPets = SR.HasPets,
                             TotalCost = SR.TotalCost,

                             FirstName = user.FirstName,
                             LastName = user.LastName,

                             AddressLine1 = SRa == null ? "" : SRa.AddressLine1,
                             AddressLine2 = SRa == null ? "" : SRa.AddressLine2,
                             City = SRa == null ? "" : SRa.City,
                             State = SRa == null ? "" : SRa.State,
                             PostalCode = SRa == null ? "" : SRa.PostalCode,
                             Mobile = SRa == null ? "" : SRa.Mobile,

                             ServiceExtraId = String.Join(", ", extras)
                         }).Single();

            return PartialView(query);
        }

        public IActionResult CancelServiceRequest(int id)
        {
            int userid = (int)HttpContext.Session.GetInt32("UserID_Session");

            ServiceRequest sr = _db.ServiceRequests.Where(x => x.ServiceRequestId == id).FirstOrDefault();
            sr.Status = 0; //new
            sr.ServiceProviderId = null;
            sr.SpacceptedDate = null;
            _db.ServiceRequests.Update(sr);
            _db.SaveChanges();
            return RedirectToAction("SPUpcoming");
        }

        public IActionResult CompleteServiceRequest(int id)
        {
            int userid = (int)HttpContext.Session.GetInt32("UserID_Session");

            ServiceRequest sr = _db.ServiceRequests.Where(x => x.ServiceRequestId == id).FirstOrDefault();
            sr.Status = 2; //completed

            bool isa = _db.FavoriteAndBlockeds.Any(x => x.UserId == userid && x.TargetUserId == sr.UserId);

            if (!isa)
            {
                FavoriteAndBlocked FB = new FavoriteAndBlocked()
                {
                    UserId = userid,
                    TargetUserId = sr.UserId,
                    IsFavorite = false,
                    IsBlocked = false
                };
                _db.FavoriteAndBlockeds.Add(FB);
            }
            
            _db.ServiceRequests.Update(sr);
            _db.SaveChanges();

            TempData["Msg4Popup"] = "Congratulations!! Your Service has been completed Successfully";
            return RedirectToAction("SPUpcoming");
        }

        //Service History
        public IActionResult SPServiceHistory()
        {
            int id = (int)HttpContext.Session.GetInt32("UserID_Session");
            User sp = _db.Users.Where(x => x.UserId == id).FirstOrDefault();

            if (sp != null)
            {
                if (sp.UserTypeId == 2)
                {
                    var query = from sr in _db.ServiceRequests
                                join sraddress in _db.ServiceRequestAddresses on sr.ServiceRequestId equals sraddress.ServiceRequestId into srad
                                from Sra in srad.DefaultIfEmpty()
                                join user in _db.Users on sr.UserId equals user.UserId
                                where sr.ServiceProviderId == id && sr.Status == 2 //completed
                                select new CustomModel
                                {
                                    ServiceId = sr.ServiceId,
                                    ServiceRequestId = sr.ServiceRequestId,
                                    ServiceStartDate = sr.ServiceStartDate,

                                    FirstName = user.FirstName,
                                    LastName = user.LastName,

                                    AddressLine1 = Sra == null ? "" : Sra.AddressLine1,
                                    AddressLine2 = Sra == null ? "" : Sra.AddressLine2,
                                    PostalCode = Sra == null ? "" : Sra.PostalCode,
                                    City = Sra == null ? "" : Sra.City
                                };
                    return View(query);
                }
                else
                {
                    TempData["ErrorMsg"] = "You don't have Service Provider access. Please Contact Admin for further details.";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["ErrorMsg"] = "You have to login to access the page.";
                return RedirectToAction("Index");
            }
        }

        //Service History Modal
        public IActionResult _SPServiceHistoryModal(int id)
        {
            ServiceRequestExtra[] obj = _db.ServiceRequestExtras.Where(x => x.ServiceRequestId == id).ToArray();
            List<string> extras = new List<string>();
            foreach (var item in obj)
            {
                switch (item.ServiceExtraId)
                {
                    case 1:
                        extras.Add("Inside Cabinet");
                        break;
                    case 2:
                        extras.Add("Inside Fridge");
                        break;
                    case 3:
                        extras.Add("Inside Oven");
                        break;
                    case 4:
                        extras.Add("Laundry Wash & Dry");
                        break;
                    case 5:
                        extras.Add("Interior Windows");
                        break;
                    default:
                        break;
                }
            }

            var query = (from SR in _db.ServiceRequests
                         join SRaddress in _db.ServiceRequestAddresses on SR.ServiceRequestId equals SRaddress.ServiceRequestId into srad
                         from SRa in srad.DefaultIfEmpty()
                         join user in _db.Users on SR.UserId equals user.UserId
                         where SR.ServiceRequestId == id
                         select new CustomModel
                         {
                             ServiceRequestId = SR.ServiceRequestId,
                             ServiceId = SR.ServiceId,
                             ServiceStartDate = SR.ServiceStartDate,
                             ServiceHours = SR.ServiceHours,
                             Comments = SR.Comments,
                             HasPets = SR.HasPets,
                             TotalCost = SR.TotalCost,

                             FirstName = user.FirstName,
                             LastName = user.LastName,

                             AddressLine1 = SRa == null ? "" : SRa.AddressLine1,
                             AddressLine2 = SRa == null ? "" : SRa.AddressLine2,
                             City = SRa == null ? "" : SRa.City,
                             State = SRa == null ? "" : SRa.State,
                             PostalCode = SRa == null ? "" : SRa.PostalCode,
                             Mobile = SRa == null ? "" : SRa.Mobile,

                             ServiceExtraId = String.Join(", ", extras)
                         }).Single();

            return PartialView(query);
        }

        // Service Provider Ratings page
        public IActionResult SPRatings()
        {
            int id = (int)HttpContext.Session.GetInt32("UserID_Session");
            User sp = _db.Users.Where(x => x.UserId == id).FirstOrDefault();

            if (sp != null)
            {
                if (sp.UserTypeId == 2)
                {
                    var query = from rate in _db.Ratings
                                join sr in _db.ServiceRequests on rate.ServiceRequestId equals sr.ServiceRequestId
                                join user in _db.Users on rate.RatingFrom equals user.UserId
                                where rate.RatingTo == id
                                select new CustomModel
                                {
                                    ServiceId = sr.ServiceId,
                                    FirstName = user.FirstName,
                                    LastName = user.LastName,

                                    Ratings = rate.Ratings,
                                    Comments = rate.Comments,
                                    RatingDate = rate.RatingDate
                                };
                    return View(query);
                }
                else
                {
                    TempData["ErrorMsg"] = "You don't have Service Provider access. Please Contact Admin for further details.";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["ErrorMsg"] = "You have to login to access the page.";
                return RedirectToAction("Index");
            }

        }

        // SP Block Customer Page
        public IActionResult SPBlockCustomer()
        {
            int id = (int)HttpContext.Session.GetInt32("UserID_Session");
            User sp = _db.Users.Where(x => x.UserId == id).FirstOrDefault();

            if (sp != null)
            {
                if (sp.UserTypeId == 2)
                {
                    var query = from user in _db.Users
                                join fav in _db.FavoriteAndBlockeds on user.UserId equals fav.TargetUserId
                                where fav.UserId == id
                                select new CustomModel
                                {
                                    FirstName = user.FirstName,
                                    LastName = user.LastName,
                                    IsBlocked = fav.IsBlocked,
                                    Id = fav.Id
                                };
                    return View(query);
                }
                else
                {
                    TempData["ErrorMsg"] = "You don't have Service Provider access. Please Contact Admin for further details.";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["ErrorMsg"] = "You have to login to access the page.";
                return RedirectToAction("Index");
            }

        }

        // SP Block Customer (POST)
        public IActionResult BlockCustomer(int id)
        {
            FavoriteAndBlocked fav = _db.FavoriteAndBlockeds.Where(x => x.Id == id).FirstOrDefault();
            fav.IsBlocked = true;
            _db.FavoriteAndBlockeds.Update(fav);
            _db.SaveChanges();
            return RedirectToAction("SPBlockCustomer");
        }

        // SP Unblock Customer (POST)
        public IActionResult UnblockCustomer(int id)
        {
            FavoriteAndBlocked fav = _db.FavoriteAndBlockeds.Where(x => x.Id == id).FirstOrDefault();
            fav.IsBlocked = false;
            _db.FavoriteAndBlockeds.Update(fav);
            _db.SaveChanges();
            return RedirectToAction("SPBlockCustomer");
        }

        //====================================
        //          Admin-Pages
        //====================================

        public IActionResult AdminUserManagement()
        {
            var userID = HttpContext.Session.GetInt32("UserID_Session");
            //var userName = HttpContext.Session.GetInt32("UserName_Session");

            if (userID != null)
            {
                User user = _db.Users.Where(x => x.UserId == userID).FirstOrDefault();
                if (user.UserTypeId == 1)
                {
                    List<User> users = _db.Users.ToList();
                    ViewBag.AdminPage = true;
                    return View(users);
                }
                else
                {
                    TempData["Msg4Popup"] = "Only Admin(s) are Authenticated to Visit this page.";
                    return RedirectToAction("Index");
                }
            }
            TempData["Msg4Popup"] = "You have to login to Visit this Page";
            return RedirectToAction("Index");
        }

        //Activate or Deactivate User
        public IActionResult UserStatus(int id)
        {
            User user = _db.Users.Where(x => x.UserId == id).FirstOrDefault();
            if (user.Status == 1)
            {
                user.Status = 0;
            }
            else
            {
                user.Status = 1;
            }
            _db.Users.Update(user);
            _db.SaveChanges();
            return RedirectToAction("AdminUserManagement");
        }

        //Admin Service Requests
        public IActionResult AdminServiceRequests()
        {
            var userID = HttpContext.Session.GetInt32("UserID_Session");
            //var userName = HttpContext.Session.GetInt32("UserName_Session");

            if (userID != null)
            {
                User user = _db.Users.Where(x => x.UserId == userID).FirstOrDefault();
                if (user.UserTypeId == 1)
                {
                    var query = from sr in _db.ServiceRequests
                                join sradd in _db.ServiceRequestAddresses on sr.ServiceRequestId equals sradd.ServiceRequestId
                                join customer in _db.Users on sr.UserId equals customer.UserId
                                join sp in _db.Users on sr.ServiceProviderId equals sp.UserId into abc
                                from sp in abc.DefaultIfEmpty()
                                join rate in _db.Ratings on sr.ServiceRequestId equals rate.ServiceRequestId into xyz
                                from rate in xyz.DefaultIfEmpty()
                                select new CustomModel
                                {
                                    ServiceRequestId = sr.ServiceRequestId,
                                    ServiceId = sr.ServiceId,
                                    ServiceStartDate = sr.ServiceStartDate,
                                    TotalCost = sr.TotalCost,
                                    Status = sr.Status,
                                    ServiceProviderId = sr.ServiceProviderId,

                                    FirstName = customer.FirstName,
                                    LastName = customer.LastName,
                                    AddressLine1 = sradd.AddressLine1,
                                    AddressLine2 = sradd.AddressLine2,
                                    PostalCode = sradd.PostalCode,
                                    City = sradd.City,

                                    SPFirstName = sp == null ? "" : sp.FirstName,
                                    SPLastName = sp == null ? "" : sp.LastName,
                                    Ratings = rate == null ? 0 : rate.Ratings
                                };

                    ViewBag.AdminPage = true;
                    return View(query);
                }
                else
                {
                    TempData["Msg4Popup"] = "Only Admin(s) are Authenticated to Visit this page.";
                    return RedirectToAction("Index");
                }
            }
            TempData["Msg4Popup"] = "You have to login to Visit this Page";
            return RedirectToAction("Index");
        }

        // Edit and Reschedule Modal
        public IActionResult _EditandReschedule(int id)
        {
            var query = (from sr in _db.ServiceRequests
                        join sradd in _db.ServiceRequestAddresses on sr.ServiceRequestId equals sradd.ServiceRequestId
                        where sr.ServiceRequestId == id
                        select new CustomModel
                        {
                            ServiceRequestId = sr.ServiceRequestId,
                            ServiceId = sr.ServiceId,
                            ServiceStartDate = sr.ServiceStartDate,
                            AddressLine1 = sradd.AddressLine1,
                            AddressLine2 = sradd.AddressLine2,
                            PostalCode = sradd.PostalCode,
                            City = sradd.City
                        }).Single();
            return PartialView(query);
        }

        // Edit and Reschedule Request (POST)
        public bool EditandRescheduleService(CustomModel obj)
        {
            int userid = (int)HttpContext.Session.GetInt32("UserID_Session");

            ServiceRequest sr = _db.ServiceRequests.Where(x => x.ServiceRequestId == obj.ServiceRequestId).FirstOrDefault();
            ServiceRequestAddress sradd = _db.ServiceRequestAddresses.Where(x => x.ServiceRequestId == obj.ServiceRequestId).FirstOrDefault();
            sr.ServiceStartDate = obj.ServiceStartDate;
            sradd.AddressLine1 = obj.AddressLine1;
            sradd.AddressLine2 = obj.AddressLine2;
            sradd.PostalCode = obj.PostalCode;
            sr.ZipCode = obj.PostalCode;
            sradd.City = obj.City;
            sr.Comments = obj.Comments;
            sr.ModifiedDate = DateTime.Now;
            sr.ModifiedBy = userid;
            _db.ServiceRequests.Update(sr);
            _db.ServiceRequestAddresses.Update(sradd);
            _db.SaveChanges();
            return true;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
