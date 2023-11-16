using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NguyenQuocThinhSachOnlinee.Models;

namespace NguyenQuocThinhSachOnlinee.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
        SachOnlDataContext db = new SachOnlDataContext();

        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }
        public ActionResult ThemGioHang(int ms, string url)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sp = lstGioHang.Find(n => n.iMaSach == ms);
            if (sp == null)
            {
                sp = new GioHang(ms);
                lstGioHang.Add(sp);
            }
            else
            {
                sp.iSoLuong++;
            }
            return Redirect(url);
        }
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                iTongSoLuong = lstGioHang.Sum(n => n.iSoLuong);

            }
            return iTongSoLuong;
        }
        private double TongTien()
        {
            double dTongTien = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                dTongTien = lstGioHang.Sum(n => n.dThanhTien);
            }
            return dTongTien;
        }
        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("TrangChu", "TrangChu");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);

        }
        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }
        public ActionResult XoaSPKhoiGioHang(int iMaSach)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sp = lstGioHang.SingleOrDefault(n => n.iMaSach == iMaSach);
            if (sp != null)
            {
                lstGioHang.RemoveAll(n => n.iMaSach == iMaSach);
                if (lstGioHang.Count == 0)
                {
                    return RedirectToAction("TrangChu", "TrangChu");
                }
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult CapNhatGioHang(int iMaSach, FormCollection f)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sp = lstGioHang.SingleOrDefault(n => n.iMaSach == iMaSach);
            if (sp != null)
            {
                sp.iSoLuong = int.Parse(f["txtSoLuong"].ToString());
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult XoaGioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            lstGioHang.Clear();
            return RedirectToAction("TrangChu", "TrangChu");

        }

        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "User");
            }

            if (Session["GioHang"] == null)
            {
                return RedirectToAction("TrangChu", "TrangChu");
            }
            List<GioHang> lstGioHang = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);

        }

        [HttpPost]
        
        public ActionResult DatHang(FormCollection f)
        {
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["TaiKhoan"];
            List<GioHang> lstGioHang = LayGioHang();
            ddh.MaKH = kh.MaKH;
            ddh.NgayDat = DateTime.Now;

            string customerEmail = kh.Email;
            

            // Gửi email thông báo cho khách hàng
            var fromAddress = new MailAddress("quocthinh5.v2003@gmail.com", "Đơn hàng ");
            var toAddress = new MailAddress(customerEmail);
            const string fromPassword = "uuiv yswn atcy yomu";
            const string subject = "Đơn hàng đã được đặt thành công";
            string body = $"Chào bạn {kh.HoTen},\n\n";
            body += "Cảm ơn bạn đã đặt hàng! Đơn hàng của bạn đã được đặt thành công.\n";
            body += "Chi tiết đơn hàng:\n\n";

            foreach (var item in lstGioHang)
            {
                body += $"{item.iSoLuong} x {item.sTenSach}: {item.dDonGia:C}\n";
            }

            body += $"\nTổng cộng: {TongTien():C}\n\n";
     
            body += "Cảm ơn bạn đã mua sắm tại cửa hàng chúng tôi!\n";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
            // Kiểm tra xem NgayGiao có giá trị hợp lệ hay không
            string ngayGiaoString = f["NgayGiao"];
            DateTime ngayGiao;
            if (DateTime.TryParse(ngayGiaoString, out ngayGiao))
            {
                ddh.NgayGiao = ngayGiao;
                ddh.TinhTrangGiaoHang = 1;
                ddh.DaThanhToan = false;

                db.DONDATHANGs.InsertOnSubmit(ddh);
                db.SubmitChanges();

                foreach (var item in lstGioHang)
                {
                    CHITIETDATHANG ctdh = new CHITIETDATHANG();
                    ctdh.MaDonHang = ddh.MaDonHang;
                    ctdh.MaSach = item.iMaSach;
                    ctdh.SoLuong = item.iSoLuong;
                    ctdh.DonGia = (decimal)item.dDonGia;
                    db.CHITIETDATHANGs.InsertOnSubmit(ctdh);
                }

                db.SubmitChanges();
                Session["GioHang"] = null;
                return RedirectToAction("XacNhanDonHang", "GioHang");
            }
            else
            {
                // Xử lý trường hợp NgayGiao không hợp lệ
                // Ví dụ: thông báo lỗi hoặc thực hiện hành động khác tùy thuộc vào yêu cầu của bạn.
                ModelState.AddModelError("NgayGiao", "Ngày giao hàng không hợp lệ.");
                return View(lstGioHang);
            }

            return RedirectToAction("DatHang","Giohang");
        }


        public ActionResult XacNhanDonHang()
        {
            return View();
        }



    }



}