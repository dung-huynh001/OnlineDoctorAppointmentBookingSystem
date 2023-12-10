using DoctorAppointmentSystem.Models.Account;
using DoctorAppointmentSystem.Models.Account.Login;
using DoctorAppointmentSystem.Models.Account.Register;
using DoctorAppointmentSystem.Models.DB;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoctorAppointmentSystem.HelperClasses;
using System.Web.Security;
using System.IO;
using System.Threading.Tasks;
using DoctorAppointmentSystem.Models.Account.ChangePassword;
using DoctorAppointmentSystem.Models.Account.AuthenQuestion;
using DoctorAppointmentSystem.Menu;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;


namespace DoctorAppointmentSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly RegisterIO registerIO;
        private readonly LoginIO loginIO;

        public AccountController()
        {
            this.registerIO = new RegisterIO();
            this.loginIO = new LoginIO();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            try
            {
                USER user = loginIO.GetUser(model.Username);
                if (user == null)
                {
                    throw new Exception("Username not found.");
                }

                if (!PasswordHelper.VerifyPassword(model.Password, user.PASSWORDHASH))
                {
                    throw new Exception("User failed to login.");
                }

                if (!user.STATUS)
                {
                    return Json(new { success = false, message = "Account has been locked." });
                }

                if (user.LASTLOGIN == null)
                {
                    return Json(new { success = false, message = "Please check your email to activate your account before logging in." });
                }

                FormsAuthentication.SetAuthCookie(model.Username, false);
                GetInfo.Username = model.Username.Trim();

                string action, controller, area;
                loginIO.UserRedirects(user, out area, out controller, out action);
                string url = Url.Action(action, controller, new { area = area });

                return Json(new { success = true, url = url }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string sEventCatg = GetInfo.GetUserType(model.Username) + " PORTAL";
                string sEventMsg = "Exception: " + ex.Message;
                string sEventSrc = nameof(Login);
                string sEventType = "S";
                string sInsBy = model.Username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);

                return Json(new { success = false, message = "Username or password is incorrect." });
            }
        }


        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            var questions = SystemParaHelper.GenerateAuthQuestion();
            ViewBag.questions = new SelectList(questions, "id", "paraval");
            var genders = SystemParaHelper.GenerateGender();
            ViewBag.genders = new SelectList(genders, "id", "paraval");
            return View();
        }

        [HttpPost]
        public ActionResult RegisterAPI(UserViewModel user, PatientViewModel patient)
        {
            string message;
            if (!registerIO.VerifyUserInfo(user, out message))
            {
                return Json(new { success = false, message }, JsonRequestBehavior.AllowGet);
            }
            if (!registerIO.VerifyPatientInfo(patient, out message))
            {
                return Json(new { success = false, message }, JsonRequestBehavior.AllowGet);
            }

            registerIO.CreateNewAccount(user, patient, out message);

            // Send activation mail asynchronously
            string activationToken = EmailSender.GenerateActivationToken(user.username);
            string activationLink = EmailSender.GenerateActivationLink(activationToken);

            EmailSender emailSender = new EmailSender();
            _ = Task.Run(() => emailSender.SendActivationEmailAsync(user.email, patient.fullName, activationLink));

            return Json(new { success = true, message }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult UploadFiles(string username)
        {
            username = string.IsNullOrEmpty(username.Trim()) ? User.Identity.Name : username;
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string imgFormat = Path.GetExtension(file.FileName);
                        string fileName = username + "_avatar" + imgFormat;
                        string filePath = Path.Combine(Server.MapPath("~/Uploads/"), fileName);

                        using (Image image = Image.Load(file.InputStream))
                        {
                            // Resize the image to the desired dimensions
                            int targetWidth = 400;
                            int targetHeight = 400;
                            image.Mutate(x => x.Resize(new ResizeOptions
                            {
                                Size = new Size(targetWidth, targetHeight),
                                Mode = ResizeMode.Max
                            }));

                            // Save the image as JPEG with reduced quality
                            var jpegEncoder = new JpegEncoder
                            {
                                Quality = 50 // Adjust the quality value as needed (0-100)
                            };
                            image.Save(filePath, jpegEncoder);
                        }

                        using (DBContext dbContext = new DBContext())
                        {
                            if (!string.IsNullOrEmpty(User.Identity.Name))
                            {
                                USER user = dbContext.USER.FirstOrDefault(u => u.USERNAME.Equals(User.Identity.Name));
                                if (user != null)
                                {
                                    user.AVATARURL = fileName;
                                    dbContext.SaveChanges();
                                }
                            }
                        }
                    }

                    return Json(new { success = true, message = "Good Job." });
                }
                catch (Exception ex)
                {
                    string sEventCatg = "PATIENT PORTAL";
                    string sEventMsg = "Exception: " + ex.Message;
                    string sEventSrc = "UploadFiles";
                    string sEventType = "I";
                    string sInsBy = username;

                    Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
                    return Json(new { success = false, message = "Error occurred. Error details: " + ex.Message });
                }
            }
            else
            {
                return Json(new { success = false, message = "No files selected." });
            }
        }

        public ActionResult Activate(string token, string expires)
        {
            string username = "";
            try
            {
                if (token == null)
                {
                    ViewBag.message = "Token not found.";
                    throw new ArgumentNullException();
                }
                else if (registerIO.IsValidToken(token, expires, ref username))
                {
                    registerIO.ActivateAccount(username);
                    ViewBag.message = "Congratulations! Your account has been successfully activated.";
                }
                else
                {
                    throw new Exception("Account activation failed.");
                }
            }
            catch (Exception ex)
            {
                string sEventCatg = "PATIENT PORTAL";
                string sEventMsg = "Exception: " + ex.Message;
                string sEventSrc = "Activate";
                string sEventType = "U";
                string sInsBy = username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
            }

            return View();
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            ViewBag.menu = RenderMenu.RenderPatientMenu("");
            ViewBag.name = GetInfo.GetFullName(User.Identity.Name);
            ViewBag.avatar = GetInfo.GetImgPath(User.Identity.Name);

            ViewBag.questions = new ChangePasswordIO().GetAuthQuestions(User.Identity.Name);
            return View();
        }

        public ActionResult VerifyAnswers(VerifyAccountViewModel answers)
        {
            try
            {
                bool success = new ChangePasswordIO().VerifyAccount(answers, User.Identity.Name);
                if (success)
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                string sEventCatg = "PATIENT PORTAL";
                string sEventMsg = "Exception: " + ex.Message;
                string sEventSrc = "VerifyAnswers";
                string sEventType = "S";
                string sInsBy = GetInfo.Username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
                return Json(new { success = false, message = "Failed to verify account! Please check your answers again." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult ChangePasswordAPI(ChangePasswordViewModel data)
        {
            try
            {
                bool success = new ChangePasswordIO().ChangePassword(data);
                if (success)
                {
                    return Json(new { success = true, message = "Change password successfully." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    throw new Exception("User failed to change password.");
                }
            }
            catch (Exception ex)
            {
                string sEventCatg = "PATIENT PORTAL";
                string sEventMsg = "Exception: " + ex.Message;
                string sEventSrc = "ChangePasswordAPI";
                string sEventType = "U";
                string sInsBy = GetInfo.Username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);

                return Json(new { success = false, message = "Password incorrect! Please check again" }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        public ActionResult AuthenQuestion()
        {
            var questions = SystemParaHelper.GenerateAuthQuestion();
            ViewBag.questions = new SelectList(questions, "id", "paraval");
            string username = User.Identity.Name;
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Account");
            }
            if (registerIO.IsAnswerPaswdRecovery(username))
            {

                var user = registerIO.GetUserByUserName(username);
                if (user.USERTYPE.Equals("Patient"))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    if (user.USERTYPE.Equals("Doctor"))

                        return RedirectToAction("Index", "Home", new { area = "Doctor" });
                }

                return RedirectToAction("Index", "Manage", new { area = "Admin" });

            }
            ViewBag.username = username;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult AuthenQuestion(AuthenQuestionModel question)
        {
            question.passwordRecoveryAns1 = question.passwordRecoveryAns1 != null ? question.passwordRecoveryAns1.Trim() : question.passwordRecoveryAns1;
            question.passwordRecoveryAns2 = question.passwordRecoveryAns2 != null ? question.passwordRecoveryAns2.Trim() : question.passwordRecoveryAns2;
            question.passwordRecoveryAns3 = question.passwordRecoveryAns3 != null ? question.passwordRecoveryAns3.Trim() : question.passwordRecoveryAns3;
            string username = User.Identity.Name;
            if (username == question.username)
            {
                var result = registerIO.SetAuthenQuestions(question);


                if (result)
                {
                    string action = "";
                    string controller = "";
                    string area = "";
                    var user = registerIO.GetUserByUserName(username);
                    loginIO.UserRedirects(user, out area, out controller, out action);
                    string url = Url.Action(action, controller, new { area = area });
                    return Json(new { success = true, message = "", url = url });

                }
            }
            return Json(new { success = false });
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Intro", "Home");
        }

        public ActionResult LogoutAPI()
        {
            FormsAuthentication.SignOut();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}