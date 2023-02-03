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
    public class CTDHController : Controller
    {
        private BanBanhEntities db = new BanBanhEntities();

        // GET: Admin/CTDH
        public ActionResult Index()
        {
            var cTDHs = db.CTDHs.Include(c => c.SANPHAM).Include(c => c.DONHANG);
            return View(cTDHs.ToList());
        }

        // GET: Admin/CTDH/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTDH cTDH = db.CTDHs.Find(id);
            if (cTDH == null)
            {
                return HttpNotFound();
            }
            return View(cTDH);
        }

        // GET: Admin/CTDH/Create
        public ActionResult Create()
        {
            ViewBag.MASP = new SelectList(db.SANPHAMs, "MASP", "TENSP");
            ViewBag.SODH = new SelectList(db.DONHANGs, "SODH", "USER_ID");
            return View();
        }

        // POST: Admin/CTDH/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SODH,MASP,SL,THANHTIEN")] CTDH cTDH)
        {
            if (ModelState.IsValid)
            {
                db.CTDHs.Add(cTDH);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MASP = new SelectList(db.SANPHAMs, "MASP", "TENSP", cTDH.MASP);
            ViewBag.SODH = new SelectList(db.DONHANGs, "SODH", "USER_ID", cTDH.SODH);
            return View(cTDH);
        }

        // GET: Admin/CTDH/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTDH cTDH = db.CTDHs.Find(id);
            if (cTDH == null)
            {
                return HttpNotFound();
            }
            ViewBag.MASP = new SelectList(db.SANPHAMs, "MASP", "TENSP", cTDH.MASP);
            ViewBag.SODH = new SelectList(db.DONHANGs, "SODH", "USER_ID", cTDH.SODH);
            return View(cTDH);
        }

        // POST: Admin/CTDH/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SODH,MASP,SL,THANHTIEN")] CTDH cTDH)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cTDH).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MASP = new SelectList(db.SANPHAMs, "MASP", "TENSP", cTDH.MASP);
            ViewBag.SODH = new SelectList(db.DONHANGs, "SODH", "USER_ID", cTDH.SODH);
            return View(cTDH);
        }

        // GET: Admin/CTDH/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CTDH cTDH = db.CTDHs.Find(id);
            if (cTDH == null)
            {
                return HttpNotFound();
            }
            return View(cTDH);
        }

        // POST: Admin/CTDH/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CTDH cTDH = db.CTDHs.Find(id);
            db.CTDHs.Remove(cTDH);
            db.SaveChanges();
            return RedirectToAction("Index");
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
