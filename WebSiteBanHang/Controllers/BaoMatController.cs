using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;


namespace WebSiteBanHang.Controllers
{
   
    public class BaoMatController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection f)
        {
            string username = f.Get("username").ToString();
            string password = f.Get("password").ToString();
            ThanhVien u = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == username && n.MatKhau == password);
            if (u != null)
            {
                if (u.MaThanhVien == 5)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                }
            }
            ViewBag.ThongBao = "Bạn Không có quyền truy cap";
            return View("Index");
        }
    }
}