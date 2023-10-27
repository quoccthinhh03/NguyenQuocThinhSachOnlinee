using NguyenQuocThinhSachOnlinee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Linq.Dynamic;
using System.Linq.Expressions;


namespace NguyenQuocThinhSachOnlinee.Controllers
{
    public class SearchController : Controller
    {
        SachOnlDataContext db = new SachOnlDataContext();
        public ActionResult Search(string keyword = null, int maCD = 0)
        {
            ViewBag.cd = db.CHUDEs.ToList();
            var searchResults = db.SACHes.AsQueryable();

            if (maCD != 0)
            {
                searchResults = searchResults.Where(book => book.CHUDE.MaCD == maCD);
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                searchResults = searchResults.Where(book => book.TenSach.Contains(keyword));
            }

            return View(searchResults.ToList());
        }

      
        public ActionResult SearchTheoDanhMuc(string strSearch = null, int maCD = 0)
        {
            // 1. Lưu từ khóa tìm kiếm
            ViewBag.Search = strSearch;

            //2.Tạo câu truy cơ bản
            var kq = db.SACHes.Select(b => b);
            //3. Tìm kiếm theo searchString
            if (!String.IsNullOrEmpty(strSearch))
                kq = kq.Where(b => b.TenSach.Contains(strSearch));
            //4. Tìm kiếm theo MaCD
            if (maCD != 0)
            {
                kq = kq.Where(b => b.CHUDE.MaCD == maCD);
            }
            //5. Tạo danh sách danh mục để hiển thị ở giao diện View thông qua DropDownList
            ViewBag.MaCD = new SelectList(db.CHUDEs, "MaCD", "TenChuDe"); // danh sách 
            
             //ViewBag.cd = db.CHUDEs.ToList();
             return View(kq.ToList());
        }

        public ActionResult Group()
        {
            //var kq = from s in db.SACHes group s by s.MaCD;
            var kq = db.SACHes.GroupBy(s => s.MaCD);

            return View(kq);
        }

        public ActionResult ThongKe()
        {
            var kq = from s in db.SACHes
                     join cd in db.CHUDEs on s.MaCD equals cd.MaCD
                     group s by new { cd.MaCD, cd.TenChuDe } into g
                     select new ReportInfo
                     {
                         Id = g.Key.MaCD.ToString(), // Mã CD
                         Name = g.Key.TenChuDe,      // Tên CD
                         Count = g.Count(),
                         Sum = g.Sum(n => n.SoLuongBan),
                         Max = g.Max(n => n.SoLuongBan),
                         Min = g.Min(n => n.SoLuongBan),
                         Avg = Convert.ToDecimal(g.Average(n => n.SoLuongBan))
                     };
            return View(kq);
        }

        public ActionResult Searchphantrang(int? page, string strSearch = null)
        {
            ViewBag.Search = strSearch;
            if (!string.IsNullOrEmpty(strSearch))
            {
                int iSize = 3;
                int iPageNumber = (page ?? 1);
                var kq = from s in db.SACHes where s.TenSach.Contains(strSearch) || s.MoTa.Contains(strSearch) select s;
                return View(kq.ToPagedList(iPageNumber, iSize));
            }
            return RedirectToAction("Index");
        }
       
        public ActionResult SearchPhanTrangTuyChon(int? page, int? size, string strSearch = null)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "3", Value = "3" });
            items.Add(new SelectListItem { Text = "5", Value = "5" });
            items.Add(new SelectListItem { Text = "10", Value = "10" });
            items.Add(new SelectListItem { Text = "20", Value = "20" });
            items.Add(new SelectListItem { Text = "25", Value = "25" });
            items.Add(new SelectListItem { Text = "50", Value = "50" });

            foreach (var item in items)
            {
                if (item.Value == size.ToString()) item.Selected = true;
            }

            ViewBag.size = items;
            ViewBag.currentSize = size;
            ViewBag.Search = strSearch;

            int iSize = size ?? 3;
            int iPageNumber = page ?? 1;

            var kq = from s in db.SACHes select s;

            if (!string.IsNullOrEmpty(strSearch))
            {
                kq = kq.Where(s => s.TenSach.Contains(strSearch) || s.MoTa.Contains(strSearch));
            }

            return View(kq.ToPagedList(iPageNumber, iSize));
        }

        public ActionResult Searchphantrangsapxep(int? page, string sortProperty, string sortOrder = "", string strSearch = null)
        {
            ViewBag.Search = strSearch;
            if (!string.IsNullOrEmpty(strSearch))
            {
                int iSize = 3;
                int iPageNumber = page ?? 1;
                if (sortOrder == "") ViewBag.SortOrder = "desc";
                if (sortOrder == "desc") ViewBag.SortOrder = "";
                if (sortOrder == "") ViewBag.SortOrder = "asc";
                if (string.IsNullOrEmpty(sortProperty))
                    sortProperty = "TenSach";
                ViewBag.SortProperty = sortProperty;
                var kq = from s in db.SACHes where s.TenSach.Contains(strSearch) || s.MoTa.Contains(strSearch) select s;
               // if (sortOrder == "desc")
                //{
                   kq = kq.OrderBy(sortProperty + " desc" + sortOrder);
               // }
              // else
               // {
                 //   kq = kq.OrderBy(sortProperty);
               // }
                return View(kq.ToPagedList(iPageNumber, iSize));

            }
            return View();
        }

    }
}