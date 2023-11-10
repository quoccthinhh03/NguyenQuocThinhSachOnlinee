using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NguyenQuocThinhSachOnlinee
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");



            routes.MapRoute(
             name: "Trang chu",
             url: "",
             defaults: new { controller = "TrangChu", action = "TrangChu", id = UrlParameter.Optional },
              namespaces: new string[] { "NguyenQuocThinhSachOnlinee.Controllers" }
         );
            routes.MapRoute(
        name: "Sach theo Chu de",
        url: "sach-theo-chu-de-{id}",
        defaults: new { controller = "TrangChu", action = "SachTheoChuDe", MaCD = UrlParameter.Optional },
        namespaces: new string[] { "NguyenQuocThinhSachOnlinee.Controllers" }
      );
            routes.MapRoute(
         name: "Sach theo NXB",
         url: "sach-theo-nxb-{id}",
         defaults: new { controller = "TrangChu", action = "SachTheoNhaXuatBan", MaNXB = UrlParameter.Optional },
         namespaces: new string[] { "NguyenQuocThinhSachOnlinee.Controllers" }
     );
            routes.MapRoute(
          name: "Chi tiet sach",
          url: "chi-tiet-sach-{id}",
          defaults: new { controller = "TrangChu", action = "ChiTietSach", MaSach = UrlParameter.Optional },
          namespaces: new string[] { "NguyenQuocThinhSachOnlinee.Controllers" }
      );
            routes.MapRoute(
        name: "Dang ky",
        url: "dang-ky",
        defaults: new { controller = "User", action = "dangky" },
        namespaces: new string[] { "NguyenQuocThinhSachOnlinee.Controllers" }
    );
            routes.MapRoute(
         name: "Dang nhap",
         url: "dang-nhap",
         defaults: new { controller = "User", action = "DangNhap", url = UrlParameter.Optional },
         namespaces: new string[] { "NguyenQuocThinhSachOnlinee.Controllers" }
     );

            routes.MapRoute(
           name: "Trang tin",
           url: "{metatitle}",
           defaults: new { controller = "TrangChu", action = "TrangTin", MaTT = UrlParameter.Optional },
           namespaces: new string[] { "NguyenQuocThinhSachOnlinee.Controllers" }
       );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "TrangChu", action = "TrangChu", id = UrlParameter.Optional },
                namespaces: new[] { "NguyenQuocThinhSachOnlinee.Controllers" }
            );
       
      

        }
    }
}
