using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DoAnWeb.Models;
namespace DoAnWeb
{
    public class GIOHANG
    {
        BanBanhEntities db = new BanBanhEntities();
        public string gHINH { get; set; }
        public string gMASP { get; set; }
        public string gTENSP { get; set; }
        public int gGIA { get; set; }
        public int gSL { get; set; }
        public int gTHANHTIEN
        {
            get
            {
                return gSL * gGIA;
            }
        }
        //Hàm tạo giỏ hàng
        public GIOHANG(string masp)
        {
            gMASP = masp;
            SANPHAM sp = db.SANPHAMs.Single(s => s.MASP == gMASP);
            gTENSP = sp.TENSP;
            gHINH = sp.HINH;
            gGIA = int.Parse(sp.GIA.ToString());
            gSL = 1;
        }
    }
}