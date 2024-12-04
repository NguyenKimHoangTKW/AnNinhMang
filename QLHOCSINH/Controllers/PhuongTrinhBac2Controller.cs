using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLHOCSINH.Controllers
{
    public class PhuongTrinhBac2Controller : Controller
    {
        // GET: PhuongTrinhBac2
        [HttpGet]
        public ActionResult PhuongTrinhBac2()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PhuongTrinhBac2(FormCollection f)
        {
            var heso1 = float.Parse(f["HeSo1"]);
            var heso2 = float.Parse(f["HeSo2"]);
            var heso3 = float.Parse(f["HeSo3"]);
            float delta = (heso2 * heso2) - (4 * heso1 * heso3);
            float x1, x2;

            string filePath = Path.Combine(Server.MapPath("~/App_Data"), "ketqua.txt");

            using (StreamWriter file = new StreamWriter(filePath, true))
            {
                file.WriteLine("{0}x² + {1}x + {2}", heso1, heso2, heso3);
                if (heso1 == 0)
                {
                    ViewData["KQLoi"] = "Hệ số a phải khác không 0";
                }
                else if (delta > 0)
                {
                    x1 = (-heso2 + (float)Math.Sqrt(delta)) / (2 * heso1);
                    x2 = (-heso2 - (float)Math.Sqrt(delta)) / (2 * heso1);
                    ViewData["KetQua"] = "Phương trình có 2 nghiệm phân biệt x1 = " + x1 + " và x2 = " + x2;
                    file.WriteLine("Kết quả: Phương trình có 2 nghiệm phân biệt x1 = " + x1 + " và x2 = " + x2);
                }
                else if (delta == 0)
                {
                    x1 = x2 = (-heso2) / (2 * heso1);
                    ViewData["KetQua"] = "Phương trình có nghiệm kép x1 = x2 = " + x1;
                    file.WriteLine("Kết quả: Phương trình có nghiệm kép x1 = x2 = " + x1);
                }
                else
                {
                    ViewData["KetQua"] = "Phương trình vô nghiệm";
                    file.WriteLine("Kết quả: Phương trình vô nghiệm");
                }
            }

            return View();
        }
    }
}