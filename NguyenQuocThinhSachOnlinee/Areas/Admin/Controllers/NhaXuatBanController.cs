using NguyenQuocThinhSachOnlinee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace NguyenQuocThinhSachOnlinee.Areas.Admin.Controllers
{
    public class NhaXuatBanController : Controller
    {
        SachOnlDataContext dataContext = new SachOnlDataContext();
        // GET: Admin/NhaXuatBan
        public ActionResult Index()
        {
            return View(dataContext.NHAXUATBANs);
        }
       
        public ActionResult ThemMoi()
        {
            return View();
        }
        public NHAXUATBAN Lay1NXB(int maNXB)
        {
          return  dataContext.NHAXUATBANs.Where(nxb => nxb.MaNXB == maNXB).SingleOrDefault();
        }
        public ActionResult ChiTiet()
        {
            var maNXB = Request.QueryString["MaNXB"];
            return View(Lay1NXB(int.Parse(maNXB)));
        }
        [HttpGet]
        public ActionResult SuaNXB()
        {
            var maNXB = Request.QueryString["MaNXB"];
            return View("Sua",Lay1NXB(int.Parse(maNXB)));
        }
        public ActionResult Xoa()
        {
            var maNXB = Request.QueryString["MaNXB"];
            return View("Xoa", Lay1NXB(int.Parse(maNXB)));
        }

        [HttpPost]
        public ActionResult SuaNXB(FormCollection f) {
            var nxb = Lay1NXB(int.Parse(f["MaNXB"]));
            nxb.TenNXB = f["TenNXB"];
            nxb.DiaChi = f["DiaChi"];
            nxb.DienThoai = f["DienThoai"];
            dataContext.SubmitChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ThemMoi(NHAXUATBAN nxb)
        {
            if (ModelState.IsValid)
            {
                dataContext.NHAXUATBANs.InsertOnSubmit(nxb);
                dataContext.SubmitChanges();
                return RedirectToAction("Index");
            }
            return View(nxb);
        }


        [HttpPost]
        public ActionResult Xoa(int maNXB)
        {
            // Tìm nhà xuất bản cần xóa dựa trên mã NXB
            var nxb = dataContext.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == maNXB);

            if (nxb == null)
            {
                // Nếu không tìm thấy nhà xuất bản, bạn có thể xử lý lỗi hoặc chuyển hướng đến trang khác.
                // Ví dụ: TempData["ErrorMessage"] = "Không tìm thấy nhà xuất bản.";
                return RedirectToAction("Index");
            }

            // Xóa nhà xuất bản khỏi cơ sở dữ liệu
            dataContext.NHAXUATBANs.DeleteOnSubmit(nxb);
            dataContext.SubmitChanges();

            // Chuyển hướng về trang danh sách nhà xuất bản sau khi xóa
            return RedirectToAction("Index");
        }




    }
}