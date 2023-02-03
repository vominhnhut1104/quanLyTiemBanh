using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DoAnWeb.Models;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;

namespace DoAnWeb.Controllers
{
    public class GioHangController : Controller
    {
        BanBanhEntities db = new BanBanhEntities();
        //Lấy giỏ hàng
        public List<GIOHANG> LayGioHang()
        {
            List<GIOHANG> listGH = Session["GioHang"] as List<GIOHANG>;
            if (listGH == null)
            {
                //Nếu giỏ hàng chưa tồn tại tiến hành khởi tạo list giỏ hàng (session GioHang)
                listGH = new List<GIOHANG>();
                Session["GioHang"] = listGH;
            }
            return listGH;
        }
        //Thêm giỏ hàng
        public ActionResult ThemGioHang(string gMASP, string strUrl)
        {
            SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MASP == gMASP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            } 
            //Lấy ra session giỏ hàng
            List<GIOHANG> listGH = LayGioHang();
            //Kiểm tra sp này đã tồn tại trong session GioHang chưa?
            GIOHANG sanpham = listGH.Find(s => s.gMASP == gMASP);
            if (sanpham == null)
            {
                sanpham = new GIOHANG(gMASP);
                //Add sp mới thêm vào list
                listGH.Add(sanpham);
                return Redirect(strUrl);
            }
            else
            {
                sanpham.gSL++;
                return Redirect(strUrl);
            }
        }
        //Cập nhật giỏ hàng
        public ActionResult CapnhatGioHang(string gMASP, FormCollection f)
        {
            //Kiểm tra mã sản phẩm 
            SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MASP == gMASP);
            //Nếu get sai mã sp thì sẽ trả về trang lỗi
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy giỏ hàng từ session
            List<GIOHANG> listGH = LayGioHang();
            //Kiểm tra sp có tồn tại trong session GioHang
            GIOHANG sanpham = listGH.SingleOrDefault(n => n.gMASP == gMASP);
            //Nếu tồn tại thì chúng ta cho sửa
            if (sanpham != null)
            {
                sanpham.gSL = int.Parse( f["txtSoLuong"].ToString());
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaGioHang(string gMASP)
        {
            //Kiểm tra mã sản phẩm 
            SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MASP == gMASP);
            //Nếu get sai mã sp thì sẽ trả về trang lỗi
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy giỏ hàng từ session
            List<GIOHANG> listGH = LayGioHang();
            GIOHANG sanpham = listGH.SingleOrDefault(n => n.gMASP == gMASP);
            //Nếu tồn tại thì chúng ta cho sửa
            if (sanpham != null)
            {
                listGH.RemoveAll(n => n.gMASP == sanpham.gMASP);               
            }
            if(listGH.Count == 0)
            {
                return RedirectToAction("Index","Home");
            }
            return RedirectToAction("GioHang");
        }
        //Xây dựng trang giỏ hàng
        public ActionResult GioHang()
        {
            if (TongSL() == 0)
            {
                return View();
            }
            ViewBag.TongSL = TongSL();

            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<GIOHANG> listGH = LayGioHang();
            return View(listGH);
        }
        //Tính tổng số lượng và tổng tiền
        //Tính tổng số lượng
        private int TongSL()
        {
            int gTONGSL = 0;
            List<GIOHANG> listGH = Session["GioHang"] as List<GIOHANG>;
            if (listGH != null)
            {
                gTONGSL = listGH.Sum(n => n.gSL);
            }
            return gTONGSL;
        }
        //Tính tổng thành tiền
        private int TongTien()
        {
            int gTONGTIEN = 0;
            List<GIOHANG> listGH = Session["GioHang"] as List<GIOHANG>;
            if (listGH != null)
            {
                gTONGTIEN = listGH.Sum(n => n.gTHANHTIEN);
            }
            return gTONGTIEN;
        }

        //Tạo partial giỏ hàng
        public ActionResult GioHangPartial()
        {
            if (TongSL() == 0)
            {
                return PartialView();
            }
            ViewBag.TongSL = TongSL();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }

        //Tạo view sửa SL giỏ hàng
        public ActionResult EditGH()
        {
            if (TongSL() == 0)
            {
                return View();
            }
            ViewBag.TongSL = TongSL();

            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<GIOHANG> listGH = LayGioHang();
            return View(listGH);
        }
        
        // Đặt Hàng
        [HttpPost]
        public ActionResult Order()
        {
            //Kiểm tra đăng nhập người dùng
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("Login", "Home");
            }
            //Kiểm tra giỏ hàng
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            
            //Lỗi
            //var id = (from d in db.DONHANGs select d).Max(x => x.SODH);k 
            //id = "DH" + (int.Parse(id.Substring(2)) + 1).ToString();
            var don = (from n in db.DONHANGs where n.NGAYDAT == null select n).FirstOrDefault();

            //Thêm đơn hàng
            USER kh = (USER)Session["TaiKhoan"];
            List<GIOHANG> gh = LayGioHang();
            don.USER_ID = kh.USER_ID;
            don.NGAYDAT = DateTime.Now;
            db.Entry(don).State = EntityState.Modified;

            string id = don.SODH;
            int num = int.Parse(id.Substring(2)) + 1;
            string DHnext = "DH" + num.ToString();

            DONHANG DHmoi = new DONHANG();
            DHmoi.SODH = DHnext;
            //Thêm đơn hàng vào csdl
            db.DONHANGs.Add(DHmoi);
            db.SaveChanges();
            //Thêm chi tiết đơn hàng
            foreach(var item in gh)
            {
                CTDH ctdh = new CTDH();
                ctdh.SODH = don.SODH;
                ctdh.MASP = item.gMASP;
                ctdh.SL = item.gSL;
                ctdh.THANHTIEN = item.gTHANHTIEN;
                db.CTDHs.Add(ctdh);
            }
            db.SaveChanges();
            List<GIOHANG> listGH =new  List<GIOHANG>();
            Session["GioHang"] = listGH;
            ViewBag.Message = "Your order was successfull!";
            return RedirectToAction("Index", "Home");
        }

        public ActionResult InfoThanhtoan (string id)
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
        public ActionResult InfoThanhtoan([Bind(Include = "USER_ID,USER_NAME,PASSWORD,EMAIL,SODT,DCHI,Allowed")] USER uSER)
        {
            if (ModelState.IsValid)
            {
                db.Entry(uSER).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("GioHang");
            }
            return View(uSER);
        }
    }
}