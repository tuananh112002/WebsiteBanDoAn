using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;


namespace WebSiteBanHang.Controllers
{
    [Authorize(Roles = "QuanTri,QuanLySanPham")]
    public class KhachHangController : Controller
    {
       QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        [Authorize(Roles = "QuanLySanPham")]
        public ActionResult Index()
        {          
            var lstKH = db.KhachHangs;
            return View(lstKH);
        }
                 [Authorize(Roles = "QuanTri")]
        public ActionResult Index1()
        {
            var lstKH = from KH in db.KhachHangs select KH;
            return View(lstKH);
        }
        public ActionResult TruyVan1DoiTuong()
        {
            var lstKH = from kh in db.KhachHangs where kh.MaKH==2 select kh ;
            KhachHang khang = db.KhachHangs.SingleOrDefault(n=>n.MaKH==2);
            return View(khang);
        }
        public ActionResult SortDuLieu()
        { 
            List<KhachHang> lstKH = db.KhachHangs.OrderByDescending(n => n.TenKH).ToList();
            return View(lstKH);
                 
        }
        public ActionResult GroupDuLieu()
        {
            List<ThanhVien> lstKH = db.ThanhViens.OrderByDescending(n => n.TaiKhoan).ToList();
            return View(lstKH);
        }
       
	}
}