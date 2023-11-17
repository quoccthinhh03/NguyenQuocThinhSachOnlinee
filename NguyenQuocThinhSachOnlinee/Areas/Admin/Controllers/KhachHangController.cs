using NguyenQuocThinhSachOnlinee.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace NguyenQuocThinhSachOnlinee.Areas.Admin.Controllers
{
    public class KhachHangController : Controller
    {
        // GET: Admin/KhachHang
        SachOnlDataContext dataContext = new SachOnlDataContext();
        public ActionResult Index()
        {
            return View(dataContext.KHACHHANGs);
        }

        public ActionResult ThemMoi()
        {
            return View();
        }
        public KHACHHANG Lay1KH(int maKH)
        {
            return dataContext.KHACHHANGs.Where(nxb => nxb.MaKH == maKH).SingleOrDefault();
        }
        public ActionResult ChiTiet()
        {
            var maNXB = Request.QueryString["MaKH"];
            return View(Lay1KH(int.Parse(maNXB)));
        }
        [HttpGet]
        public ActionResult Sua()
        {
            var maNXB = Request.QueryString["MaKH"];
            return View("Sua", Lay1KH(int.Parse(maNXB)));
        }
        public ActionResult Xoa()
        {
            var maNXB = Request.QueryString["MaKH"];
            return View("Xoa", Lay1KH(int.Parse(maNXB)));
        }

        [HttpPost]
        public ActionResult Sua(FormCollection f)
        {
            var nxb = Lay1KH(int.Parse(f["MaKH"]));
            nxb.HoTen = f["HoTen"];
            nxb.TaiKhoan = f["TaiKhoan"];
            nxb.MatKhau = f["MatKhau"];
            nxb.Email = f["Email"];
            nxb.DiaChi = f["DiaChi"];
            nxb.DienThoai = f["DienThoai"];
            nxb.NgaySinh = Convert.ToDateTime(f["NgaySinh"]);
            dataContext.SubmitChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ThemMoi(KHACHHANG kh)
        {
            if (ModelState.IsValid)
            {
                dataContext.KHACHHANGs.InsertOnSubmit(kh);
                dataContext.SubmitChanges();
                return RedirectToAction("Index");
            }
            return View(kh);
        }


        [HttpPost]
        public ActionResult Xoa(int maKH)
        {
            // Tìm nhà xuất bản cần xóa dựa trên mã NXB
            var nxb = dataContext.KHACHHANGs.SingleOrDefault(n => n.MaKH == maKH);

            if (nxb == null)
            {
                // Nếu không tìm thấy nhà xuất bản, bạn có thể xử lý lỗi hoặc chuyển hướng đến trang khác.
                // Ví dụ: TempData["ErrorMessage"] = "Không tìm thấy nhà xuất bản.";
                return RedirectToAction("Index");
            }

            // Xóa nhà xuất bản khỏi cơ sở dữ liệu
            dataContext.KHACHHANGs.DeleteOnSubmit(nxb);
            dataContext.SubmitChanges();

            // Chuyển hướng về trang danh sách nhà xuất bản sau khi xóa
            return RedirectToAction("Index");
        }

    }
}