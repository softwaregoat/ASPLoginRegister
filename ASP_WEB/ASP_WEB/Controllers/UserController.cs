using ASP_WEB.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ASP_WEB.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Registration()
        {
            User user = new User();
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(User user)
        {
            user.SecurityLevel = 1;
            user.Activate = true;
            string Message = "";
            bool Status = false;
            if (ModelState.IsValid)
            {
                var isExist = IsEmailExist(user.Email);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already exist");
                    return View(user);
                }
                user.ActivationCode = Guid.NewGuid();
                user.Password = Crypto.Hash(user.Password);
                user.IsEmailVerified = false;

                using(MyDatabaseEntities de = new MyDatabaseEntities())
                {
                    de.Users.Add(user);
                    de.SaveChanges();
                    SendVerificationLinkEmail(user.Email, user.ActivationCode.ToString());
                    Message = "Registration successfully done. Account activation link " +
                        " has been sent to your email id: " + user.Email;
                    Status = true;
                }
            }
            else
            {
                Message = "Invalid Request";
                Status = false;
            }
            ViewBag.Message = Message;
            ViewBag.Status = Status;
            return View(user);
        }

        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            using(MyDatabaseEntities de = new MyDatabaseEntities())
            {
                de.Configuration.ValidateOnSaveEnabled = false;

                var v = de.Users.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
                if(v != null)
                {
                    v.IsEmailVerified = true;
                    de.SaveChanges();
                    Status = true;
                }
                else
                {
                    ViewBag.Message = "Invalid Request";
                }
            }
            ViewBag.Status = Status;
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login, string ReturnUrl = "")
        {
            string message = "";
            using (MyDatabaseEntities de = new MyDatabaseEntities())
            {
                var v = de.Users.Where(a => a.Email == login.Email).FirstOrDefault();
                if (v != null)
                {
                    if (string.Compare(Crypto.Hash(login.Password), v.Password) ==0)
                    {
                        int timeout = login.RememberMe ? 3600 : 1200;
                        var ticket = new FormsAuthenticationTicket(login.Email, login.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);

                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Details", new { id = v.UserID });
                        }

                    }
                    else
                    {
                        message = "Invalid credential provided";
                    }
                }
                else
                {
                    message = "Invalid credential provided";
                }
            }
            ViewBag.Message = message;
            return View();
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }

        // GET: User/Details/5
        public ActionResult Details(int id = 0)
        {

            User user = new User();
            using (MyDatabaseEntities de = new MyDatabaseEntities())
            {
                if (id == 0)
                {
                    var email = HttpContext.User.Identity.Name;
                    user = de.Users.Where(a => a.Email == email).FirstOrDefault();
                }
                else
                {
                    user = de.Users.Where(a => a.UserID == id).FirstOrDefault();
                }
                
            }
            return View(user);
        }

        public ActionResult List()
        {
            List<User> users = new List<User>();
            var userEmail = System.Web.HttpContext.Current.User.Identity.Name;
            using (MyDatabaseEntities de = new MyDatabaseEntities())
            {
                var user = de.Users.Where(a => a.Email == userEmail).FirstOrDefault();
                users = de.Users.Where(a => a.CompanyID == user.CompanyID).ToList();
            }
            return View(users);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                User user = new User();
                var userEmail = System.Web.HttpContext.Current.User.Identity.Name;
                using (MyDatabaseEntities de = new MyDatabaseEntities())
                {
                    var user1 = de.Users.Where(a => a.Email == userEmail).FirstOrDefault();
                    user.FirstName = collection["FirstName"];
                    user.LastName = collection["LastName"];
                    user.Email = collection["Email"];
                    user.DisplayName = collection["DisplayName"];
                    user.CompanyID = user1.CompanyID;
                    user.Password = Crypto.Hash(collection["Password"]);
                    user.Phone = collection["Phone"];
                    user.Address = collection["Address"];
                    user.SecurityLevel = Convert.ToInt32(collection["SecurityLevel"]);
                    user.Activate = true;

                    de.Users.Add(user);

                    de.SaveChanges();
                }
                   
                // TODO: Add insert logic here

                return RedirectToAction("List");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id = 0)
        {
            User user = new User();
            using (MyDatabaseEntities de = new MyDatabaseEntities())
            {
                if (id == 0)
                {
                    var email = HttpContext.User.Identity.Name;
                    user = de.Users.Where(a => a.Email == email).FirstOrDefault();
                }
                else
                {
                    user = de.Users.Where(a => a.UserID == id).FirstOrDefault();
                }
            }
            return View(user);
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(FormCollection collection)
        {
            var userID = Convert.ToInt32(collection["UserID"]);
            using (MyDatabaseEntities de = new MyDatabaseEntities())
            {
                
                var user = de.Users.Where(a => a.UserID == userID).FirstOrDefault();
                if (user!=null)
                {

                    user.FirstName = collection["FirstName"];
                    user.LastName = collection["LastName"];
                    user.Email = collection["Email"];
                    user.DisplayName = collection["DisplayName"];
                    user.CompanyID = Convert.ToInt32(collection["CompanyID"]);
                    user.Password = Crypto.Hash(collection["Password"]);
                    user.Phone = collection["Phone"];
                    user.Address = collection["Address"];
                    user.SecurityLevel = Convert.ToInt32(collection["SecurityLevel"]);
                    de.SaveChanges();

                    HttpPostedFileBase fileUploadPDF = Request.Files.Get("profile");
                    if (fileUploadPDF != null)
                    {
                        string StrPath = Server.MapPath("~/Content/User/" + user.Email.Replace('@', '_') + ".jpg");
                        fileUploadPDF.SaveAs(StrPath);
                    }
                }
            }
            return RedirectToAction("Details", new { id = userID });
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            using (MyDatabaseEntities de = new MyDatabaseEntities())
            {

                var user = de.Users.Where(a => a.UserID == id).FirstOrDefault();
                if (user != null)
                {
                    de.Users.Remove(user);
                    de.SaveChanges();
                }
            }
            return RedirectToAction("List");
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        [NonAction]
        public bool IsEmailExist(string email)
        {
            using(MyDatabaseEntities de = new MyDatabaseEntities())
            {
                var v = de.Users.Where(a => a.Email == email).FirstOrDefault();
                return v != null;
            }
        }
        [NonAction]
        public void SendVerificationLinkEmail(string email, string activationCode)
        {
            var verifyUrl = "/User/VerifyAccount/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
            var fromEmail = new MailAddress("pongha425@gmail.com", "Donet Awesome");
            var toEmail = new MailAddress(email);
            var fromEmailPassword = "irontiger!!!";
            string subject = "Your account is successfully created";
            string body = "<br/><br/>We are excited to tell you that your account has been activated" +
                ". Click on the below link to verify your account " +
                "<a href='" + link + "'>" + link + "</a>";
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var messge = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            smtp.Send(messge);
        }
    }
}
