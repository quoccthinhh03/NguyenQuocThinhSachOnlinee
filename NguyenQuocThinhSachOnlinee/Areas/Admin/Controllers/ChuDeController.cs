using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NguyenQuocThinhSachOnlinee.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace NguyenQuocThinhSachOnlinee.Areas.Admin.Controllers
{
    public class ChuDeController : Controller
    {
        SachOnlDataContext dataContext = new SachOnlDataContext();
        // GET: Admin/ChuDE

        public ActionResult Index()
        {
            return View(dataContext.CHUDEs);
        }

        public ActionResult ThemMoi()
        {
            return View();
        }
        public CHUDE Lay1CD(int maCD)
        {
            return dataContext.CHUDEs.Where(nxb => nxb.MaCD == maCD).SingleOrDefault();
        }
        public ActionResult ChiTiet()
        {
            var maCD = Request.QueryString["MaCD"];
            return View(Lay1CD(int.Parse(maCD)));
        }
        [HttpGet]
        public ActionResult SuaCD()
        {
            var maCD = Request.QueryString["MaCD"];
            return View("SuaCD", Lay1CD(int.Parse(maCD)));
        }
        public ActionResult Xoa()
        {
            var maCD = Request.QueryString["MaCD"];
            return View("Xoa", Lay1CD(int.Parse(maCD)));
        }

        [HttpPost]
        public ActionResult SuaCD(FormCollection f)
        {
            var maCD = Lay1CD(int.Parse(f["MaCD"]));
            maCD.TenChuDe = f["TenChuDe"];
            dataContext.SubmitChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ThemMoi(CHUDE cd)
        {
            if (ModelState.IsValid)
            {
                dataContext.CHUDEs.InsertOnSubmit(cd);
                dataContext.SubmitChanges();
                return RedirectToAction("Index");
            }
            return View(cd);
        }


        [HttpPost]
        public ActionResult Xoa(int maCD)
        {
            // Tìm nhà xuất bản cần xóa dựa trên mã NXB
            var nxb = dataContext.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == maCD);

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