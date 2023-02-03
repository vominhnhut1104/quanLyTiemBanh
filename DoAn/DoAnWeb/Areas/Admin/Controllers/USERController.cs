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
    public class USERController : Controller
    {
        private BanBanhEntities db = new BanBanhEntities();

        // GET: Admin/USER
        public ActionResult Index()
        {
            var data = db.sp_LayUser(); 
            return View(data.ToList());
        }

        // GET: Admin/USER/Details/5
        public ActionResult Details(string id)
        {
            var d = db.sp_LayidUser(id).SingleOrDefault(); 
            return View(d);
        }

        // GET: Admin/USER/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/USER/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "USER_ID,USER_NAME,PASSWORD,EMAIL,SODT,DCHI,Allowed")] USER uSER)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.sp_ThemUser(uSER.USER_NAME, uSER.PASSWORD, uSER.EMAIL, uSER.SODT, uSER.DCHI);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(uSER);
            }
            catch { return View(); }
        }

        // GET: Admin/USER/Edit/5
        public ActionResult Edit(string id)
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

        // POST: Admin/USER/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "USER_ID,USER_NAME,PASSWORD,EMAIL,SODT,DCHI,Allowed")] USER uSER)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.sp_CapNhatUser(uSER.USER_ID, uSER.USER_NAME, uSER.PASSWORD, uSER.EMAIL, uSER.SODT, uSER.DCHI);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(uSER);
            }
            catch { return View(); }
        }

        // GET: Admin/USER/Delete/5
        public ActionResult Delete(string id)
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

        // POST: Admin/USER/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                USER uSER = db.USERS.Find(id);
                db.sp_XoaUser(uSER.USER_ID);
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
