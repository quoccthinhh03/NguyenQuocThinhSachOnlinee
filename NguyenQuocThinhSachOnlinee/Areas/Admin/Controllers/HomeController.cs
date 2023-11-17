using NguyenQuocThinhSachOnlinee.Models;
using System.Linq;
using System.Web.Mvc;

namespace NguyenQuocThinhSachOnlinee.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Admin/Home
        SachOnlDataContext db = new SachOnlDataContext();
        public ActionResult Index()
        {
            if (Session["Admin"] == null)
            {
                // If not authenticated, redirect to the login page
                return RedirectToAction("Login", "Home");
            }

            // If authenticated, continue with the normal flow for the Index page
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            //Gán các giá trị người dùng nhập liệu cho các biến
            var sTenDN = f["UserName"];
            var sMatKhau = f["Password"];
            //Gán giá trị cho đối tượng được tạo mới (ad) 
            ADMIN ad = db.ADMINs.SingleOrDefault(n => n.TenDN == sTenDN && n.MatKhau == sMatKhau);
            if (ad != null)
            {
                Session["Admin"] = ad;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }

        public ActionResult LoginLogout()
        {
            return PartialView("LoginLogoutPartial");
        }
    }
}