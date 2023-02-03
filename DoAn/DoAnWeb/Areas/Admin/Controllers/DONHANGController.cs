using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAnWeb.Models;

namespace DoAnWeb.Areas.Admin.Controllers
{
    public class DONHANGController : Controller
    {
        private BanBanhEntities db = new BanBanhEntities();

        // GET: Admin/DONHANG
        public ActionResult Index()
        {
            var data = db.sp_LayDH();
            return View(data.ToList());
        }

        // GET: Admin/DONHANG/Details/5
        public ActionResult Details(string id)
        {
            var d = db.sp_LayidDH(id).SingleOrDefault();
            return View(d);
        }

        // GET: Admin/DONHANG/Create
        public ActionResult Create()
        {
            ViewBag.USER_ID = new SelectList(db.USERS, "USER_ID", "USER_NAME");
            return View();
        }

        // POST: Admin/DONHANG/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SODH,USER_ID,NGAYDAT,NGAYGIAO,TONGGIATIEN")] DONHANG dONHANG)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.sp_ThemDH(dONHANG.USER_ID);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.USER_ID = new SelectList(db.USERS, "USER_ID", "USER_NAME", dONHANG.USER_ID);
                return View(dONHANG);
            }
            catch { return View(); }
        }

        // GET: Admin/DONHANG/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DONHANG dONHANG = db.DONHANGs.Find(id);
            if (dONHANG == null)
            {
                return HttpNotFound();
            }
            ViewBag.USER_ID = new SelectList(db.USERS, "USER_ID", "USER_NAME", dONHANG.USER_ID);
            return View(dONHANG);
        }

        // POST: Admin/DONHANG/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SODH,USER_ID,NGAYDAT,NGAYGIAO,TONGGIATIEN")] DONHANG dONHANG)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.sp_CapNhatDH(dONHANG.SODH, dONHANG.USER_ID, dONHANG.NGAYDAT, dONHANG.NGAYGIAO);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.USER_ID = new SelectList(db.USERS, "USER_ID", "USER_NAME", dONHANG.USER_ID);
                return View(dONHANG);
            }
            catch { return View(); }
        }

        // GET: Admin/DONHANG/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DONHANG dONHANG = db.DONHANGs.Find(id);
            if (dONHANG == null)
            {
                return HttpNotFound();
            }
            return View(dONHANG);
        }

        // POST: Admin/DONHANG/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                DONHANG dONHANG = db.DONHANGs.Find(id);
                db.sp_XoaDH(dONHANG.SODH);
                db.SaveChanges();
                return RedirectToAction("Index");
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
