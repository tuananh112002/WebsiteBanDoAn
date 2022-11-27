using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;
using System.Web.Security;

namespace WebSiteBanHang.Controllers
{
    public class LoginAdminController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection ad)
        {
            string username = ad.Get("username").ToString();
            string password = ad.Get("password").ToString();
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == username && n.MatKhau == password);
            if (tv != null)
            {
                if (tv.MaThanhVien == 1)
                {

                    var lstQuyen = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == tv.MaLoaiTV);
                    string Quyen = "";
                    if (lstQuyen.Count() != 0)
                    {
                        foreach (var item in lstQuyen)
                        {
                            Quyen += item.Quyen.MaQuyen + ",";
                        }
                        Quyen = Quyen.Substring(0, Quyen.Length - 1);
                        PhanQuyen(tv.TaiKhoan.ToString(), Quyen);
                        Session["TaiKhoan"] = tv;
                        Session["TaiKhoan"] = tv.HoTen.ToString();
                        return RedirectToAction("Index", "Admin");
                    }
                }
            }
            ViewBag.ThongBao = "Bạn Không có quyền truy cap";
            return View("Index");

        }
        public void PhanQuyen(string TaiKhoan, string Quyen)
        {
            FormsAuthentication.Initialize();
            var ticket = new FormsAuthenticationTicket(1,
                                          TaiKhoan, 
                                          DateTime.Now, 
                                          DateTime.Now.AddHours(3), 
                                          false, 
                                          Quyen, 
                                          FormsAuthentication.FormsCookiePath);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;
            Response.Cookies.Add(cookie);
        }
        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}