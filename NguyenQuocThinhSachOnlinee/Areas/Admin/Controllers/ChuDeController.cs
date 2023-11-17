using NguyenQuocThinhSachOnlinee.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace NguyenQuocThinhSachOnlinee.Areas.Admin.Controllers
{
    public class ChuDeController : Controller
    {
        SachOnlDataContext dataContext = new SachOnlDataContext();
        // GET: Admin/ChuDE

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DsChuDe()
        {
            try
            {
                var dsCD = (from cd in dataContext.CHUDEs
                            select new
                            {
                                MaCD = cd.MaCD,
                                TenCD = cd.TenChuDe
                            }).ToList();
                return Json(new { code = 200, dsCD = dsCD, msg = "Lay danh sach chu de thanh cong" },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "lay danh sach chu de that bai" + ex.Message },
                    JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]

        public JsonResult Detail(int maCD)
        {
            try
            {
                var cd = (from s in dataContext.CHUDEs
                          where (s.MaCD == maCD)
                          select new
                          {
                              MaCD = s.MaCD,
                              TenChuDe = s.TenChuDe
                          }).SingleOrDefault();
                //db.CHUDEs.SingleOrDefault(c => c.MaCD == maCD);
                return Json(new { code = 200, cd = cd, msg = "Lấy thông tin chủ đề thành công" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy thông tin chủ đề thất bại." + ex.Message }, JsonRequestBehavior.AllowGet);
            }


        }


        [HttpPost]

        public JsonResult AddChuDe(string strTenCD)
        {
            try
            {
                var cd = new CHUDE();
                cd.TenChuDe = strTenCD;
                dataContext.CHUDEs.InsertOnSubmit(cd);
                dataContext.SubmitChanges();
                return Json(new { code = 200, msg = "Thêm chủ đề thành công." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Thêm chủ đề thất bại. LỖI " + ex.Message }, JsonRequestBehavior.AllowGet);
            }


        }


        [HttpPost]

        public JsonResult Update(int maCD, string strTenCD)
        {
            try
            {
                var cd = dataContext.CHUDEs.SingleOrDefault(c => c.MaCD == maCD);
                cd.TenChuDe = strTenCD;
                dataContext.SubmitChanges();
                return Json(new { code = 200, msg = "sửa chủ đề thành công." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                {
                    return Json(new { code = 580, msg = "sửa chủ đề thất bại Lỗi " + ex.Message }, JsonRequestBehavior.AllowGet);


                }
            }


        }
        [HttpPost]

        public JsonResult Delete(int maCD)
        {
            try
            {
                var cd = dataContext.CHUDEs.SingleOrDefault(c => c.MaCD == maCD);
                dataContext.CHUDEs.DeleteOnSubmit(cd);

                dataContext.SubmitChanges();
                return Json(new { code = 200, msg = "Xóa chủ đề thành công." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                {
                    return Json(new { code = 500, msg = "Xóa chủ đề không  thành công. Lỗi " + ex.Message }, JsonRequestBehavior.AllowGet);

                }
            }
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