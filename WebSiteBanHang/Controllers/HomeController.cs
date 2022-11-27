using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;
using CaptchaMvc.HtmlHelpers;
using CaptchaMvc;
using System.Web.Security;

namespace WebSiteBanHang.Controllers
{
     
    public class HomeController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult Index()
        {
            
            var lstDTM = db.SanPhams.Where(n => n.MaLoaiSP == 1 && n.Moi == 1 && n.DaXoa == false);
            ViewBag.ListDTM = lstDTM;

            var lstLT = db.SanPhams.Where(n => n.MaLoaiSP == 2 && n.Moi == 2 && n.DaXoa == false);
            ViewBag.ListLTM = lstLT;
      
            var lstMTB = db.SanPhams.Where(n => n.MaLoaiSP == 3 && n.Moi == 3 && n.DaXoa == false);
            ViewBag.ListMTBM = lstMTB;

            return View();
        }
        public ActionResult MenuPartial()
        {
            var lstSP = db.SanPhams;
            return PartialView(lstSP); 
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());

            return View();
        }

    
        public ActionResult DangKy(ThanhVien tv,FormCollection f)
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
           
                if (ModelState.IsValid)
                {
                    ViewBag.ThongBao = "Thêm thành công";
                    tv.MaLoaiTV = 2;
                    db.ThanhViens.Add(tv);
                    db.SaveChanges();
                }
                else
                { 
                    ViewBag.ThongBao = "Thêm thất bại";
                }
                return View();
            
         

    
            
        }
        [HttpGet]
        public ActionResult DangKy1()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy1(ThanhVien tv)
        {
            return View();
        }
        public List<string> LoadCauHoi()
        {
            List<string> lstCauHoi = new List<string>();
            lstCauHoi.Add("Con vật mà bạn yêu thích?");
            lstCauHoi.Add("Bạn thích thể thao không?");
            lstCauHoi.Add("Đội bóng yêu thích cảu bạn là?");
            return lstCauHoi;
        }
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            
            string taikhoan = f["txtTenDangNhap"].ToString();
            string matkhau = f["txtMatKhau"].ToString();
            
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == taikhoan && n.MatKhau == matkhau);
            if (tv != null)
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
                        Session["TaiKhoan"] = tv.HoTen.ToString();

                    return Content("<script>window.location.reload();</script>");                
                }
            }
            return Content("Tài khoản hoặc mật khẩu không đúng!");
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
        public ActionResult LoiPhanQuyen()
        {

            return View();
        }
        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
	}
}