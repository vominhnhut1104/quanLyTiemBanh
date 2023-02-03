using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnWeb.Models;
using System.Data.Entity;
using System.Net;
using System.Net.Mail;

namespace DoAnWeb.Controllers
{
    public class HomeController : Controller
    {
        BanBanhEntities db = new BanBanhEntities();

        public ActionResult Index()
        {
            ViewBag.Sanphammoi = db.SANPHAMs.OrderByDescending(x => x.NGAYCAPNHAT).Take(8).ToList();
            //Sản phẩm hot best seller
            var sp = db.f_slBan().OrderByDescending(x => x.SLBan).Take(6).ToList();
            ViewBag.SLBanchay = sp;
            return View();
        }

        public ActionResult Details(string id)
        {
            var a = db.f_slBan().OrderByDescending(x => x.SLBan).Take(6).ToList();
            ViewBag.SLBanchay = a;
            SANPHAM sp = db.SANPHAMs.SingleOrDefault(x => x.MASP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }
            return View(sp);
        }

        public ActionResult About()
        {
            return View("About");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Shop()
        {
            var sp = db.f_slBan().OrderByDescending(x => x.SLBan).Take(6).ToList();
            ViewBag.SLBanchay = sp;
            ViewBag.Allsp = db.SANPHAMs.ToList();
            return View("Shop");
        }
        public ActionResult Drink()
        {
            var sp = db.f_slBan().OrderByDescending(x => x.SLBan).Take(6).ToList();
            ViewBag.SLBanchay = sp;
            ViewBag.Drink = db.SANPHAMs.Where(x => x.MALOAISP == "LSP4").ToList();
            return View("Drink");
        }
        public ActionResult Cake()
        {
            var sp = db.f_slBan().OrderByDescending(x => x.SLBan).Take(6).ToList();
            ViewBag.SLBanchay = sp;
            ViewBag.cake = db.SANPHAMs.Where(x => x.MALOAISP == "LSP1").ToList();
            return View("Cake");
        }
        public ActionResult Combo()
        {
            var sp = db.f_slBan().OrderByDescending(x => x.SLBan).Take(6).ToList();
            ViewBag.SLBanchay = sp;
            ViewBag.combo = db.SANPHAMs.Where(x => x.MALOAISP == "LSP5").ToList();
            return View("Combo");
        }

        public ActionResult Search(string search)
        {
            var allSP = db.SANPHAMs.Where(a => a.TENSP.Contains(search)).ToList();
            ViewBag.Allsp = allSP;
            var sp = db.f_slBan().OrderByDescending(x => x.SLBan).Take(6).ToList();
            ViewBag.SLBanchay = sp;
            return View("Shop");
        }

        public ActionResult Sorttd(string sort)
        {
            var sxgia = db.SANPHAMs.OrderBy(s => s.GIA).ToList();
            ViewBag.Allsp = sxgia;
            var sp = db.f_slBan().OrderByDescending(x => x.SLBan).Take(6).ToList();
            ViewBag.SLBanchay = sp;
            return View("Shop");
        }
        public ActionResult Sortgd(string sort)
        {
            var sxgia = db.SANPHAMs.OrderByDescending(s => s.GIA).ToList();
            ViewBag.Allsp = sxgia;
            var sp = db.f_slBan().OrderByDescending(x => x.SLBan).Take(6).ToList();
            ViewBag.SLBanchay = sp;
            return View("Shop");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            USER user = db.USERS.SingleOrDefault(x => x.USER_NAME == username && x.PASSWORD == password && x.Allowed == 1);
            if (user != null)
            {
                Session["userid"] = user.USER_ID;
                Session["username"] = user.USER_NAME;
                if (username == "admin")
                {
                    return Redirect("~/Admin/Home/Index");
                }
                Session["TaiKhoan"] = user;
                Session["isLogin"] = "T";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "Wrong Username or Password";
                return View();
            }
        }

        [HttpGet]
        public ActionResult Register()
        {
            ViewBag.error = false;
            return View();
        }

        [HttpPost]
        public ActionResult Register(string username, string password, string mail, string phone)
        {
            var taikhoan = (from a in db.USERS select a.USER_NAME).ToList();
            foreach (var tk in taikhoan)
            {
                if (username == tk)
                {
                    ViewBag.Message = "User already exists";
                    ViewBag.error_tk = true;
                    ViewBag.tentk = username;
                    ViewBag.sdt = phone;
                    ViewBag.mail = mail;
                    return View("Register");
                }
            }
            var sodt = (from p in db.USERS select p.SODT).ToList();
            foreach (var so in sodt)
            {
                if (phone == so)
                {
                    ViewBag.Message = "Number already exists";
                    ViewBag.error_sodt = true;
                    ViewBag.tentk = username;
                    ViewBag.sdt = phone;
                    ViewBag.mail = mail;
                    return View("Register");
                }
            }
            var email = (from e in db.USERS select e.EMAIL).ToList();
            foreach (var maill in email)
            {
                if (mail == maill)
                {
                    ViewBag.Message = "Email already exists";
                    ViewBag.error_email = true;
                    ViewBag.tentk = username;
                    ViewBag.sdt = phone;
                    ViewBag.mail = mail;
                    return View("Register");
                }
            }

            var ngdung = (from m in db.USERS where m.SODT == null select m).Single();
            ngdung.USER_NAME = username;
            ngdung.PASSWORD = password;
            ngdung.SODT = phone;
            ngdung.EMAIL = mail;
            db.Entry(ngdung).State = EntityState.Modified;

            string ID = ngdung.USER_ID;
            int num = int.Parse(ID.Substring(2)) + 1;
            string IDnext = "US" + num.ToString();

            USER IDmoi = new USER();
            IDmoi.USER_ID = IDnext;
            db.USERS.Add(IDmoi);

            db.SaveChanges();
            ViewBag.Message = "Success!";
            return View("Login");
        }

        public ActionResult Signout()
        {
            Session["userid"] = "";
            Session["username"] = "";
            Session["avatar"] = "";
            Session["isLogin"] = "";
            ViewBag.Message = "Signout Success!";
            return RedirectToAction("Index");
        }

        public ActionResult EditInfo(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USER uSER = db.USERS.Find(id);
            if (uSER == null)
            {
                return HttpNotFound();
            }
            return View(uSER);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditInfo([Bind(Include = "USER_ID,USER_NAME,PASSWORD,EMAIL,SODT,DCHI,Allowed")] USER uSER)
        {
            if (ModelState.IsValid)
            {
                db.Entry(uSER).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.Message = "Edit Success!";
                return RedirectToAction("Index");
            }
            return View(uSER);
        }
        public ActionResult ListOrder()
        {
            return View();
        }
        public ActionResult ViewOrder()
        {
            USER ngdung = Session["TaiKhoan"] as USER;
            var list = db.f_CartUs(ngdung.USER_ID).OrderByDescending(x => x.NGAYDAT);
            List<string> tinhTrang = new List<string>();
            foreach (var l in list)
            {
                if (l.NGAYDAT == null)
                    tinhTrang.Add("Đang lựa");
                else if (l.NGAYGIAO == null)
                    tinhTrang.Add("Chưa thanh toán");
                else if (l.NGAYGIAO == new DateTime(1999,1,1))
                    tinhTrang.Add("Đã hủy");
                else
                    tinhTrang.Add("Đã thanh toán");
            }
            ViewBag.tinhtrang = tinhTrang;
            ViewBag.Tongdh = list; 
            return View();
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            var kh = (from k in db.USERS where k.EMAIL == email select k).SingleOrDefault();
            if (kh == null)
            {
                ViewBag.Message = "User use this email not exist!";
                return View("ForgotPassword");
            }
            ViewBag.id = kh.USER_ID;
            ViewBag.mail = kh.EMAIL;
            return View("NewPassword");
        }

        public ActionResult NewPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewPassword(string id)
        {
            var kh = (from c in db.USERS where c.USER_ID == id select c).SingleOrDefault();
            Random r = new Random();
            string newpass = r.Next(0000,9999).ToString();
            kh.PASSWORD = newpass;
            db.Entry(kh).State = EntityState.Modified;
            db.SaveChanges();

            var mail = new SmtpClient("smtp.gmail.com", 25)
            {
                Credentials = new NetworkCredential("quido2410@gmail.com", "dolequi2410"),
                EnableSsl = true
            };
            var mes = new MailMessage();
            mes.From = new MailAddress("quido2410@gmail.com");
            mes.ReplyToList.Add("quido2410@gmail.com");
            mes.To.Add(new MailAddress(kh.EMAIL));
            mes.Subject = "Thay Đổi Mật Khẩu";
            mes.Body = "L'appétit nhận thấy bạn gặp khó khăn trong vấn đề đăng nhập. " +
                "Đây là mật khẩu mới để bạn reset lại mật khẩu cũ. " +
                "Mật khẩu mới của bạn là: " + newpass;

            mail.Send(mes);
            ViewBag.Message = "PassWord Reset!";
            return Redirect("~/Home/Login");
        }
    }
}
    