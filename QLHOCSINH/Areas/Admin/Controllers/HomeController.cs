using QLHOCSINH.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QLHOCSINH.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        QLHOCSINHEntities db = new QLHOCSINHEntities();

        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("api/get_full_tk_bi_khoa")]
        public async Task<JsonResult> load_full_tai_khoan_bi_khoa_do_spam()
        {
            var _user_locked = await db.ACCOUNTs.Where(x => x.is_locked == true)
                .Select(x => new
                {
                    x.email,
                    x.username,
                    x.password,
                    x.ROLE.ten_role,
                    x.Thoi_gian_khoa,
                }).ToListAsync();
            if (_user_locked.Any())
            {
                return Json(new {data = _user_locked, success = true},JsonRequestBehavior.AllowGet); 
            }
            else
            {
                return Json(new { message ="Không tồn tại tài khoản bị khóa", success = false }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}