using NguyenQuocThinhSachOnlinee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NguyenQuocThinhSachOnlinee.Areas.Admin.Controllers
{
    public class DonHangController : Controller
    {
        // GET: Admin/DonHang
        SachOnlDataContext dataContext = new SachOnlDataContext();
        // GET: Admin/ChuDE

        public ActionResult Index()
        {
            return View(dataContext.DONDATHANGs);
        }

        public ActionResult ThemMoi()
        {
            return View();
        }
        public DONDATHANG Lay1CD(int maCD)
        {
            return dataContext.DONDATHANGs.Where(nxb => nxb.MaDonHang == maCD).SingleOrDefault();
        }
        public ActionResult ChiTiet()
        {
            var maCD = Request.QueryString["MaDonHang"];
            return View(Lay1CD(int.Parse(maCD)));
        }
        [HttpGet]
        public ActionResult SuaCD()
        {
            var maCD = Request.QueryString["MaDonHang"];
            return View("SuaCD", Lay1CD(int.Parse(maCD)));
        }


        [HttpPost]
        public ActionResult SuaCD(FormCollection f)
        {
            var maCD = Lay1CD(int.Parse(f["MaDonHang"]));
            maCD.TinhTrangGiaoHang = int.Parse(f["TinhTrangGiaoHang"]);
            maCD.DaThanhToan = bool.Parse(f["DaThanhToan"]);
            maCD.NgayDat = Convert.ToDateTime(f["NgayDat"]);
            maCD.NgayGiao = Convert.ToDateTime(f["NgayGiao"]);
            maCD.MaKH = int.Parse(f["MaKH"]);


            dataContext.SubmitChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ThemMoi(DONDATHANG cd)
        {
            if (ModelState.IsValid)
            {
                dataContext.DONDATHANGs.InsertOnSubmit(cd);
                dataContext.SubmitChanges();
                return RedirectToAction("Index");
            }
            return View(cd);
        }

        [HttpGet]
        public ActionResult Xoa(int id)
        {
            var sach = dataContext.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }
        [HttpPost, ActionName("Xoa")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            var sach = dataContext.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var ctdh = dataContext.CHITIETDATHANGs.Where(ct => ct.MaSach == id);
            if (ctdh.Count() > 0)
            {
                ViewBag.ThongBao = "Sách này đang có trong bảng chi tiết đặt hàng <br>" + "Nếu muốn xóa thì phải xóa hết mã sách này trong bảng chi tiết đặt hàng";
                return View(sach);
            }
            var vietsach = dataContext.VIETSACHes.Where(vs => vs.MaSach == id).ToList();
            if (vietsach != null)
            {
                dataContext.VIETSACHes.DeleteAllOnSubmit(vietsach);
                dataContext.SubmitChanges();
            }
            dataContext.DONDATHANGs.DeleteOnSubmit(sach);
            dataContext.SubmitChanges();
            return RedirectToAction("Index");
        }

    } 
}