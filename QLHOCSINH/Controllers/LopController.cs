using QLHOCSINH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLHOCSINH.Controllers
{
    public class LopController : Controller
    {
        QLHOCSINHEntities db = new QLHOCSINHEntities();
        // GET: Lop
        public ActionResult ViewLop()
        {
            return View();
        }
        public ActionResult GetLop()
        {
            var lop = db.LOPs.Select(l => new {
                MaLop = l.MaLop,
                TenLop = l.TenLop,
            }).ToList();
            return Json(new { data = lop, TotalItems = lop.Count }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Add(LOP lop)
        {
            var status = "";
            if (ModelState.IsValid)
            {
                if (lop.TenLop == null)
                {
                    status = "Ho ten chua co";
                }
                else
                {
                    status = "Thêm lớp thành công";
                    db.LOPs.Add(lop);
                    db.SaveChanges();
                }

            }
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetMaxLop()
        {
            var maxId = db.LOPs.OrderByDescending(x => x.MaLop).FirstOrDefault()?.MaLop ?? "L0";
            // Kiểm tra xem maxId có phải là một số không, nếu không thì chuyển đổi thành số nguyên
            int maxIdNumber;
            if (!int.TryParse(maxId.Substring(1), out maxIdNumber))
            {
                maxIdNumber = 0;
            }
            return Json(new { maxId = maxIdNumber }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetById(string malop)
        {
            var item = db.LOPs
                         .Where(h => h.MaLop == malop)
                         .Select(h => new {
                             MaLop = h.MaLop,
                             TenLop = h.TenLop,
                         })
                         .FirstOrDefault();

            return Json(new { data = item }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult Edit(LOP l)
        {
            var status = "";
            var lop = db.LOPs.Find(l.MaLop);
            if (lop != null)
            {
                lop.TenLop = l.TenLop;

                db.SaveChanges();
                status = "Cập nhật lớp thành công";
            }
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            var status = "";
            var lop = db.LOPs.Find(id);
            if (lop != null)
            {
                db.LOPs.Remove(lop);
                db.SaveChanges();
                status = "Xóa lớp với với mã lớp là " + lop.MaLop + " | Tên lớp là : " + lop.TenLop + " thành công";
            }
            return Json(new { status = status });
        }
    }
}