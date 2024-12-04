using QLHOCSINH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLHOCSINH.Controllers
{
    public class HocSinhController : Controller
    {
        QLHOCSINHEntities db = new QLHOCSINHEntities();
        // GET: HocSinh
        public ActionResult Index()
        {
            ViewBag.LopList = new SelectList(db.LOPs.OrderBy(l => l.MaLop), "MaLop", "TenLop");
            return View();
        }
        public ActionResult GetHocSinh(string searchName, string malop)
        {
            var hocsinh = db.HOCSINHs.Select(hs => new {
                MaHS = hs.MaHS,
                HoTen = hs.HoTen,
                GioiTinh = hs.GioiTinh ? "Nam" : "Nữ",
                NgaySinh = hs.NgaySinh,
                DiaChi = hs.DiaChi,
                DiemTB = hs.DiemTB,
                Lop = hs.LOP.TenLop,
                MaLop = hs.MaLop,
            }).ToList();
            if (!string.IsNullOrEmpty(malop))
            {
                hocsinh = hocsinh.Where(x => x.MaLop == malop).ToList();
            }
            else
            {
                hocsinh = hocsinh;
            }

            if (!string.IsNullOrEmpty(searchName))
            {
                hocsinh = hocsinh.Where(x => x.HoTen.ToLower().Contains(searchName.Trim().ToLower())).ToList();
            }
            else
            {
                hocsinh = hocsinh;
            }
            return Json(new { data = hocsinh, TotalItems = hocsinh.Count }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Add(HOCSINH hs)
        {
            var status = "";
            if (ModelState.IsValid)
            {
                if(db.HOCSINHs.SingleOrDefault(a => a.MaHS == hs.MaHS) != null)
                {
                    status = "Mã học sinh đã tồn tại, vui lòng nhập mã học sinh mới";
                }
                else if (hs.HoTen == null)
                {
                    status = "Ho ten chua co";
                }
                else if (hs.DiaChi == null)
                {
                    status = "dia chi chua co";
                }
                else
                {
                    status = "Thêm học sinh thành công";
                    db.HOCSINHs.Add(hs);
                    db.SaveChanges();
                }

            }
            ViewBag.LopList = new SelectList(db.LOPs.OrderBy(l => l.MaLop), "MaLop", "TenLop",hs.MaLop);
            return Json(new {status = status }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetMaxMaHS()
        {
            var maxId = db.HOCSINHs.OrderByDescending(x => x.MaHS).FirstOrDefault()?.MaHS ?? "HS0";
            // Kiểm tra xem maxId có phải là một số không, nếu không thì chuyển đổi thành số nguyên
            int maxIdNumber;
            if (!int.TryParse(maxId.Substring(2), out maxIdNumber))
            {
                maxIdNumber = 0;
            }
            return Json(new { maxId = maxIdNumber }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetById(string mahs)
        {
            var item = db.HOCSINHs
                         .Where(h => h.MaHS == mahs)
                         .Select(h => new {
                             MaHS = h.MaHS,
                             HoTen = h.HoTen,
                             NgaySinh = h.NgaySinh,
                             GioiTinh = h.GioiTinh,
                             DiaChi = h.DiaChi,
                             MaLop = h.MaLop,
                             DiemTB = h.DiemTB
                         })
                         .FirstOrDefault();

            return Json(new { data = item }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(HOCSINH hs)
        {
            var status = "";
            var hocsinh = db.HOCSINHs.Find(hs.MaHS);
            if(hocsinh != null)
            {
                hocsinh.HoTen = hs.HoTen;
                hocsinh.NgaySinh = hs.NgaySinh;
                hocsinh.GioiTinh = hs.GioiTinh;
                hocsinh.DiaChi = hs.DiaChi;
                hocsinh.MaLop = hs.MaLop;
                hocsinh.DiemTB = hs.DiemTB;
                db.SaveChanges();
                status = "Cập nhật học sinh thành công";
            }
            ViewBag.LopList = new SelectList(db.LOPs.OrderBy(l => l.MaLop), "MaLop", "TenLop", hs.MaLop);
            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            var status = "";
            var student = db.HOCSINHs.Find(id);
            if (student != null)
            {
                db.HOCSINHs.Remove(student);
                db.SaveChanges();
                status = "Xóa học với với mã học sinh là "+student.MaHS+" | Họ tên là : "+student.HoTen+" thành công";
            }
            return Json(new { status = status });
        }

    }
}