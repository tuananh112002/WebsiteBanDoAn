using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;
using System.Net.Mail; 


namespace WebSiteBanHang.Controllers
{
     [Authorize(Roles = "QuanTri,QuanLySanPham")]
    public class QuanLyDonHangController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult ChuaThanhToan()
        {
            var lst = db.DonDatHangs.Where(n => n.DaThanhToan==false).OrderBy(n=>n.NgayDat);
            return View(lst);
        }
        public ActionResult ChuaGiao()
        {
            var lstDSDHCG = db.DonDatHangs.Where(n => n.TinhTrangGiaoHang == false&&n.DaThanhToan==true).OrderBy(n=>n.NgayGiao);
            return View(lstDSDHCG);
        }
        public ActionResult DaGiaoDaThanhToan()
        {
            var lstDSDHCG = db.DonDatHangs.Where(n => n.TinhTrangGiaoHang == true && n.DaThanhToan==true);
            return View(lstDSDHCG);
        }
        [HttpGet]
        public ActionResult DuyetDonHang(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonDatHang model = db.DonDatHangs.SingleOrDefault(n => n.MaDDH == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            var lstChiTietDH = db.ChiTietDonDatHangs.Where(n => n.MaDDH == id);
            ViewBag.ListChiTietDH = lstChiTietDH;
            return View(model);
        }
        [HttpPost]
        public ActionResult DuyetDonHang(DonDatHang ddh)
        { 
            DonDatHang ddhUpdate = db.DonDatHangs.Single(n => n.MaDDH == ddh.MaDDH);
            ddhUpdate.DaThanhToan = ddh.DaThanhToan;
            ddhUpdate.TinhTrangGiaoHang = ddh.TinhTrangGiaoHang;
            db.SaveChanges();
            var lstChiTietDH = db.ChiTietDonDatHangs.Where(n => n.MaDDH == ddh.MaDDH);
            ViewBag.ListChiTietDH = lstChiTietDH;
            try
            {
                GuiEmail("Xác nhận đơn hàng của hệ thống ", "johnytuananh@gmail.com", "ksshop.com.vn@gmail.com", "google123456", "Đơn hàng của bạn đã được đặt thành công!");
            }
            catch(Exception)
            {
                ViewBag.ThongBao = "Không gửi được email";
            }
            
            return View(ddhUpdate);
        }
        public void GuiEmail(string Title, string ToEmail, string FromEmail, string PassWord, string Content)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(ToEmail); 
            mail.From = new MailAddress(ToEmail); 
            mail.Subject = Title;  
            mail.Body = Content;                 
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com"; 
            smtp.Port = 587;               
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential
            (FromEmail, PassWord);
            smtp.EnableSsl = true;   
            smtp.Send(mail);   
        }

      
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                    db.Dispose();
                db.Dispose();
            }
            base.Dispose(disposing);
        }
	}
}