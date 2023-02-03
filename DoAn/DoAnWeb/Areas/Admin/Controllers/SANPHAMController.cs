using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAnWeb.Models;

namespace DoAnWeb.Areas.Admin.Controllers
{
    public class SANPHAMController : Controller
    {
        private BanBanhEntities db = new BanBanhEntities();

        // GET: Admin/SANPHAM
        public ActionResult Index()
        {

            var data = db.sp_LaySP();
            return View(data.ToList());
        }

        // GET: Admin/SANPHAM/Details/5
        public ActionResult Details(string id)
        {
            var d = db.sp_LayidSP(id).SingleOrDefault();
            return View(d);
        }

        // GET: Admin/SANPHAM/Create
        public ActionResult Create()
        {
            ViewBag.MALOAISP = new SelectList(db.LOAISPs, "MALOAISP", "TENLOAISP");
            return View();
        }

        // POST: Admin/SANPHAM/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MASP,TENSP,DVT,NOISX,GIA,MOTA,NGAYCAPNHAT,MALOAISP,HINH,filename")] SANPHAM sANPHAM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (sANPHAM.filename != null)
                    {
                        String fileName = Path.GetFileNameWithoutExtension(sANPHAM.filename.FileName);
                        String extension = Path.GetExtension(sANPHAM.filename.FileName);
                        fileName = fileName + extension;
                        sANPHAM.HINH = fileName;
                        fileName = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                        sANPHAM.filename.SaveAs(fileName);
                        using (var fileStream = new FileStream(fileName,FileMode.Create))
                        {
                        }
                    }
                    db.sp_ThemSP(sANPHAM.TENSP, sANPHAM.DVT, sANPHAM.NOISX, sANPHAM.GIA, sANPHAM.MOTA, sANPHAM.MALOAISP, sANPHAM.HINH);
                    db.SaveChanges();
                    return RedirectToAction("Index","SANPHAM");
                }

                ViewBag.MALOAISP = new SelectList(db.LOAISPs, "MALOAISP", "TENLOAISP", sANPHAM.MALOAISP);
                return View(sANPHAM);
            }
            catch (Exception e)
            {
                ViewBag.Status = true;
                ViewBag.Mess = e.Message;
                ViewBag.Hinh = sANPHAM;
                return View();
            }
        }

        // GET: Admin/SANPHAM/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM sANPHAM = db.SANPHAMs.Find(id);
            if (sANPHAM == null)
            {
                return HttpNotFound();
            }
            ViewBag.MALOAISP = new SelectList(db.LOAISPs, "MALOAISP", "TENLOAISP", sANPHAM.MALOAISP);
            return View(sANPHAM);
        }

        // POST: Admin/SANPHAM/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MASP,TENSP,DVT,NOISX,GIA,MOTA,NGAYCAPNHAT,MALOAISP,HINH")] SANPHAM sANPHAM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.sp_CapNhatSP(sANPHAM.MASP, sANPHAM.TENSP, sANPHAM.DVT, sANPHAM.NOISX, sANPHAM.GIA, sANPHAM.MOTA, sANPHAM.NGAYCAPNHAT, sANPHAM.MALOAISP);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.MALOAISP = new SelectList(db.LOAISPs, "MALOAISP", "TENLOAISP", sANPHAM.MALOAISP);
                return View(sANPHAM);
            }
            catch { return View(); }
            
        }

        // GET: Admin/SANPHAM/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM sANPHAM = db.SANPHAMs.Find(id);
            if (sANPHAM == null)
            {
                return HttpNotFound();
            }
            return View(sANPHAM);
        }

        // POST: Admin/SANPHAM/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                SANPHAM sANPHAM = db.SANPHAMs.Find(id);
                db.sp_XoaSP(sANPHAM.MASP);
                db.SaveChanges();
                return RedirectToAction("Index","SANPHAMController");
            }
            catch { return View(); }
            
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
