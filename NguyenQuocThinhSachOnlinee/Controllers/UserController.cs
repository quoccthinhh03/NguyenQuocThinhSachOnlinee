using NguyenQuocThinhSachOnlinee.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace NguyenQuocThinhSachOnlinee.Controllers
{
    public class UserController : Controller
    {
        SachOnlDataContext dataContext = new SachOnlDataContext();

        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            return RedirectToAction("TrangChu", "TrangChu");
        }

        // GET: User
        [HttpGet]
        public ActionResult DangNhap()
        {

            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var sTenDN = collection["TenDN"];
            var sMatkhau = collection["Matkhau"];
            if (string.IsNullOrEmpty(sTenDN))

            {
                ViewData["Err1"] = "Bạn chưa nhập tên đăng nhập";
            }
            if (string.IsNullOrEmpty(sTenDN))


            {
                ViewData["Err2"] = "Phải nhập mật khẩu";
            }
            else
            {
                KHACHHANG kh = dataContext.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == sTenDN && n.MatKhau == sMatkhau);
                if (kh != null)
                {
                    ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                    Session["TaiKhoan"] = kh;
                    if (collection["remember"].Contains("true"))
                    {
                        Response.Cookies["TenDN"].Value = sTenDN;
                        Response.Cookies["MatKhau"].Value = sMatkhau;
                        Response.Cookies["TenDN"].Expires = DateTime.Now.AddDays(1);
                        Response.Cookies["MatKhau"].Expires = DateTime.Now.AddDays(1);
                    }
                    else
                    {
                        Response.Cookies["MatKhau"].Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies["MatKhau"].Expires = DateTime.Now.AddDays(-1);
                    }
                    Session["tennd"] = kh.HoTen;

                    return RedirectToAction("GioHang", "GioHang");
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }

            }
            return View();


        }
        public ActionResult Thongtincanhan()
        {
            var maNXB = Request.QueryString["MaKH"];
            return View(Lay1KH(int.Parse(maNXB)));
        }
        public KHACHHANG Lay1KH(int maNXB)
        {
            return dataContext.KHACHHANGs.Where(nxb => nxb.MaKH == maNXB).SingleOrDefault();
        }
        [HttpGet]
        public ActionResult dangky()
        {
            return View();
        }
        [HttpPost]
        public ActionResult dangky(FormCollection f, KHACHHANG kh)
        {
            var sHoTen = f["HoTen"];
            var sTaiKhoan = f["TaiKhoan"];
            var sMatKhau = f["MatKhau"];
            var sMatKhauNhapLai = f["MatKhauNL"];
            var sDiaChi = f["DiaChi"];
            var sEmail = f["Email"];
            var sDienThoai = f["DienThoai"];
            var dNgaySinh = string.Format("{0:MM/dd/yyyy}", f["NgaySinh"]);
            if (dataContext.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == sTaiKhoan) != null)
            {
                ViewBag.ThongBao = "Tên đăng nhập đã tồn tại ";
            }
            else if (dataContext.KHACHHANGs.SingleOrDefault(n => n.Email == sEmail) != null)
            {
                ViewBag.ThongBao = "email đã được sử dụng ";
            }
            else if (ModelState.IsValid)
            {
                kh.HoTen = sHoTen;
                kh.TaiKhoan = sTaiKhoan;
                kh.Email = sEmail;
                kh.MatKhau = sMatKhau;
                kh.Email = sEmail;
                kh.DiaChi = sDiaChi;
                kh.DienThoai = sDienThoai;
                kh.NgaySinh = DateTime.Parse(dNgaySinh);
                dataContext.KHACHHANGs.InsertOnSubmit(kh);
                dataContext.SubmitChanges();
                return Redirect("~/User/dangnhap");
            }
            return View("dangky");
        }

        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;
            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");
            }
            return byte2String;
        }

    }
}