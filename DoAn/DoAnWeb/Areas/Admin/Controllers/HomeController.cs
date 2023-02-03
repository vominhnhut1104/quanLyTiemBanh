using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnWeb.Models;
namespace DoAnWeb.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        BanBanhEntities db = new BanBanhEntities();
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}