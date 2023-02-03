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
    public class LOAISPController : Controller
    {
        private BanBanhEntities db = new BanBanhEntities();

        // GET: Admin/LOAISP
        public ActionResult Index()
        {
            var data = db.sp_LayLSP();
            return View(data.ToList());
        }

        // GET: Admin/LOAISP/Details/5
        public ActionResult Details(string id)
        {
            var d = db.sp_LayidLSP(id).SingleOrDefault();
            return View(d);
        }

        // GET: Admin/LOAISP/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/LOAISP/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MALOAISP,TENLOAISP")] LOAISP lOAISP)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.sp_ThemLSP(lOAISP.MALOAISP, lOAISP.TENLOAISP);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(lOAISP);
            }
            catch { return View(); }
        }

        // GET: Admin/LOAISP/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOAISP lOAISP = db.LOAISPs.Find(id);
            if (lOAISP == null)
            {
                return HttpNotFound();
            }
            return View(lOAISP);
        }

        // POST: Admin/LOAISP/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MALOAISP,TENLOAISP")] LOAISP lOAISP)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.sp_CapNhatLSP(lOAISP.MALOAISP, lOAISP.TENLOAISP);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(lOAISP);
            }
            catch { return View(); }
        }

        // GET: Admin/LOAISP/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOAISP lOAISP = db.LOAISPs.Find(id);
            if (lOAISP == null)
            {
                return HttpNotFound();
            }
            return View(lOAISP);
        }

        // POST: Admin/LOAISP/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                LOAISP lOAISP = db.LOAISPs.Find(id);
                db.sp_XoaLSP(lOAISP.MALOAISP);
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
