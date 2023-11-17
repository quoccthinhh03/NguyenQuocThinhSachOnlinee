using NguyenQuocThinhSachOnlinee.Models;
using PagedList;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;


namespace NguyenQuocThinhSachOnlinee.Controllers
{
    public class TrangChuController : Controller
    {
        SachOnlDataContext dataContext = new SachOnlDataContext();
        // GET: TrangChu

        public ActionResult TrangChu(string currentFilter, string SearchString, int? page)
        {
            var listSachMoi = new List<SACH>();
            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }
            if (!string.IsNullOrEmpty(SearchString))
            {
                listSachMoi = dataContext.SACHes.Where(n => n.TenSach.Contains(SearchString)).ToList();
            }
            else
            {
                listSachMoi = dataContext.SACHes.ToList();

            }
            ViewBag.CurrentFilter = SearchString;
            int pageSize = 8;
            int pageNumber = (page ?? 1);
            listSachMoi = listSachMoi.OrderByDescending(n => n.MaSach).ToList();
            return View(listSachMoi.ToPagedList(pageNumber, pageSize));
        }
        [ChildActionOnly]
        public ActionResult NavPartial()
        {
            List<MENU> lst = new List<MENU>();
            lst = dataContext.MENUs.Where(m => m.ParentId == null).OrderBy(m => m.OrderNumber).ToList();
            int[] a = new int[lst.Count()];
            for (int i = 0; i < lst.Count; i++)
            {
                var l = dataContext.MENUs.Where(m => m.ParentId == lst[i].Id);
                a[i] = l.Count();
            }
            ViewBag.lst = a;
            return PartialView(lst);
        }
        [ChildActionOnly]
        public ActionResult LoadChildMenu(int parentId)
        {
            List<MENU> lst = new List<MENU>();
            lst = dataContext.MENUs.Where(m => m.ParentId == parentId).OrderBy(m => m.OrderNumber).ToList();
            ViewBag.Count = lst.Count;
            int[] a = new int[lst.Count()];
            for (int i = 0; i < lst.Count; i++)
            {
                var l = dataContext.MENUs.Where(m => m.ParentId == lst[i].Id);
                a[i] = l.Count();
            }
            ViewBag.lst = a;
            return PartialView("LoadChildMenu", lst);
        }
        public ActionResult QuangCaoPartial()
        {
            return PartialView();
        }

        public ActionResult ChuDePartial(string currentFilter, string SearchString, int? page)
        {
            var cd = from c in dataContext.CHUDEs select c; return PartialView(cd);
        }


        public ActionResult SachBanNhieuPartial()
        {
            var cd = from c in dataContext.SACHes select c; return PartialView(cd);
        }
        public ActionResult FooterPartial()
        {
            return PartialView();
        }
        public ActionResult NXBPartial()
        {
            var cd = from c in dataContext.NHAXUATBANs select c; return PartialView(cd);
        }
        public ActionResult ViewTimKiem(string keyword)
        {
            var searchResults = dataContext.SACHes.Where(book => book.TenSach.Contains(keyword) || book.MoTa.Contains(keyword));

            return View(searchResults);
        }
        public ActionResult ChiTietSach(int? id)
        {
            var sach = from s in dataContext.SACHes
                       where s.MaSach == id
                       select s;
            return View(sach.Single());

        }

        public ActionResult SachTheoChuDe(int id, int? page)
        {
            ViewBag.MaCD = id;
            int iSize = 2;
            int iPageNum = (page ?? 1);
            var sach = from m in dataContext.SACHes where m.MaCD == id select m;
            return View(sach.ToPagedList(iPageNum, iSize));
        }


        public ActionResult SachTheoNhaXuatBan(int id, int? page)
        {
            ViewBag.MaCD = id;
            int iSize = 2;
            int iPageNum = (page ?? 1);
            var sach = from m in dataContext.SACHes where m.MaNXB == id select m;
            return View(sach.ToPagedList(iPageNum, iSize));
        }
        public ActionResult loginlogout()
        {
            return PartialView();
        }
        public ActionResult TrangTin(string metatitle)
        {
            var tt = (from t in dataContext.TRANGTINs where t.MetaTitle == metatitle select t).Single();
            return View(tt);
        }
    }
}