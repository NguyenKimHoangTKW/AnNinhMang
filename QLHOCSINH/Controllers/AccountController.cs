using QLHOCSINH.Helpers;
using QLHOCSINH.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QLHOCSINH.Controllers
{
    public class AccountController : Controller
    {
        QLHOCSINHEntities db = new QLHOCSINHEntities();
        #region Các Interface
        // GET: Account
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        #endregion

        #region Đăng ký lúc chưa bảo mật
        //[HttpPost]
        //[Route("api/register")]
        //public async Task<ActionResult> register_post(ACCOUNT ac)
        //{
        //    if (db.ACCOUNTs.SingleOrDefault(x => x.username == ac.username) != null)
        //    {
        //        return Json(new { Message = "Tài khoản này đã tồn tại", success = false }, JsonRequestBehavior.AllowGet);
        //    }
        //    var check_user = await db.ACCOUNTs.FirstOrDefaultAsync(x => x.username == ac.username);
        //    if (check_user == null)
        //    {
        //        var add_user = new ACCOUNT
        //        {
        //            username = ac.username,
        //            password = ac.password,
        //            id_role = 1,
        //            count_failed_password = 0,
        //            is_locked = false,
        //            LocketEndTime = null
        //        };
        //        db.ACCOUNTs.Add(add_user);
        //    }
        //    await db.SaveChangesAsync();
        //    return Json(new { url = "/Account/Login", success = true }, JsonRequestBehavior.AllowGet);
        //}
        #endregion
        #region Đăng ký khi đã bảo mật
        public static string GenerateOtp(int length = 6)
        {
            var random = new Random();
            var otp = string.Empty;
            for (int i = 0; i < length; i++)
            {
                otp += random.Next(0, 10);
            }
            return otp;
        }

        public static void SendEmail(string toEmail, string subject, string body)
        {
            var fromEmail = "hoanggnk.it@gmail.com";
            var fromPassword = "uflx dsaw zahi tzgj";
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new System.Net.NetworkCredential(fromEmail, fromPassword),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(toEmail);

            smtpClient.Send(mailMessage);
        }
        [HttpPost]
        [Route("api/send-otp")]
        public async Task<ActionResult> SendOtp(string email)
        {
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                return Json(new { success = false, message = "Email không hợp lệ." }, JsonRequestBehavior.AllowGet);
            }

            var account = await db.ACCOUNTs.FirstOrDefaultAsync(x => x.email == email);
            if (account != null)
            {
                return Json(new { success = false, message = "Email này đã được sử dụng." }, JsonRequestBehavior.AllowGet);
            }

            var otp = GenerateOtp();
            HttpContext.Session["Otp"] = otp;

            try
            {
                SendEmail(email, "Mã OTP đăng ký tài khoản", $"Mã OTP của bạn là: {otp}");
                return Json(new { success = true, message = "Đã gửi OTP đến email của bạn." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Không thể gửi email. Vui lòng thử lại." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("api/verify-otp")]
        public JsonResult VerifyOtp(string otp)
        {
            var storedOtp = HttpContext.Session["Otp"]?.ToString();

            if (string.IsNullOrEmpty(storedOtp))
            {
                return Json(new { success = false, message = "Mã OTP đã hết hạn hoặc không tồn tại." }, JsonRequestBehavior.AllowGet);
            }

            if (storedOtp == otp)
            {
                HttpContext.Session["IsOtpVerified"] = true;
                HttpContext.Session.Remove("Otp");
                return Json(new { success = true, message = "Mã OTP chính xác." }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = false, message = "Mã OTP không hợp lệ." }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [Route("api/check-otp-verified")]
        public JsonResult CheckOtpVerified()
        {
            bool isOtpVerified = HttpContext.Session["IsOtpVerified"] != null && (bool)HttpContext.Session["IsOtpVerified"];
            return Json(new { success = isOtpVerified }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("api/register")]
        public async Task<ActionResult> register_post(ACCOUNT ac)
        {
            if (HttpContext.Session["IsOtpVerified"] == null || !(bool)HttpContext.Session["IsOtpVerified"])
            {
                return Json(new { Message = "Vui lòng xác thực OTP trước khi đăng ký.", success = false }, JsonRequestBehavior.AllowGet);
            }
            if (db.ACCOUNTs.SingleOrDefault(x => x.username == ac.username) != null)
            {
                return Json(new { Message = "Tài khoản này đã tồn tại", success = false }, JsonRequestBehavior.AllowGet);
            }

            var add_user = new ACCOUNT
            {
                email = ac.email,
                username = ac.username,
                password = AesEncryptionHelper.Encrypt(ac.password),
                id_role = 1,
                count_failed_password = 0,
                is_locked = false,
                LocketEndTime = null
            };

            db.ACCOUNTs.Add(add_user);
            await db.SaveChangesAsync();

            HttpContext.Session.Remove("IsOtpVerified");
            return Json(new { url = "/Account/Login", success = true }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Đăng nhập chưa bảo mật
        //[HttpPost]
        //[Route("api/login")]
        //public async Task<JsonResult> Login(ACCOUNT aCCOUNT)
        //{
        //    int MaxFailedAttemptsBeforeLockout = 3;
        //    int MaxFailedAttemptsToPermanentLock = 5;
        //    int LockoutDurationInMinutes = 1;
        //    var account = await db.ACCOUNTs.FirstOrDefaultAsync(x => x.username == aCCOUNT.username);

        //    if (account == null)
        //    {
        //        return Json(new { message = "Sai thông tin đăng nhập", success = false }, JsonRequestBehavior.AllowGet);
        //    }

        //    var decryptedPassword = AesEncryptionHelper.Decrypt(account.password);
        //    if (decryptedPassword == aCCOUNT.password)
        //    {
        //        return Json(new { message = "Đăng nhập thành công", success = true }, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(new { message = "Sai thông tin đăng nhập", success = false }, JsonRequestBehavior.AllowGet);
        //    }
        //}
        #endregion
        #region Đăng nhập đã bảo mật
        [HttpPost]
        [Route("api/login")]
        public async Task<JsonResult> Login(ACCOUNT aCCOUNT)
        {
            DateTime now = DateTime.UtcNow;
            int unixTimestamp = (int)(now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            int MaxFailedAttemptsBeforeLockout = 3;
            int MaxFailedAttemptsToPermanentLock = 5;
            int LockoutDurationInSeconds = 15;
            int LockoutDurationInSeconds2 = 30;
            

            var account = await db.ACCOUNTs.FirstOrDefaultAsync(x => x.username == aCCOUNT.username);

            if (account == null)
            {
                return Json(new { message = "Tài khoản không tồn tại", success = false }, JsonRequestBehavior.AllowGet);
            }

            if (account.is_locked == true)
            {
                return Json(new { message = "Tài khoản của bạn đã bị khóa vĩnh viễn vì nhập sai quá nhiều lần", success = false }, JsonRequestBehavior.AllowGet);
            }
            if (account.LocketEndTime.HasValue && account.LocketEndTime > DateTime.Now)
            {
                return Json(new
                {
                    message = $"Tài khoản bị tạm khóa. Vui lòng thử lại sau {(account.LocketEndTime.Value - DateTime.Now).Seconds} giây.",
                    success = false
                }, JsonRequestBehavior.AllowGet);
            }
            var decryptedPassword = AesEncryptionHelper.Decrypt(account.password);

            if (decryptedPassword == aCCOUNT.password)
            {
                account.count_failed_password = 0;
                account.is_locked = false;
                account.LocketEndTime = null;

                await db.SaveChangesAsync();
                Session["user"] = account;
                if (account.id_role == 1)
                {
                    return Json(new { url = "/HocSinh/Index", success = true }, JsonRequestBehavior.AllowGet);
                }
                else if(account.id_role == 2)
                {
                    return Json(new { url = "/Admin/Home/Index", success = true }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { message = "Đăng nhập thành công", success = true }, JsonRequestBehavior.AllowGet);
            }
            account.count_failed_password = (account.count_failed_password ?? 0) + 1;
            if (account.count_failed_password >= MaxFailedAttemptsToPermanentLock)
            {
                account.is_locked = true;
                account.LocketEndTime = null;
                account.Thoi_gian_khoa = unixTimestamp;
            }
            else if (account.count_failed_password >= MaxFailedAttemptsBeforeLockout)
            {
                account.LocketEndTime = DateTime.Now.AddSeconds(LockoutDurationInSeconds);
            }
            else if (account.count_failed_password == 4)
            {
                account.LocketEndTime = DateTime.Now.AddSeconds(LockoutDurationInSeconds2);
            }
            await db.SaveChangesAsync();
            return Json(new { message = "Sai thông tin đăng nhập", success = false }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        [HttpPost]
        [Route("api/logout")]
        public ActionResult Logout()
        {
            Session.Clear();
            return Json(new { url = "/Account/Login",success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}