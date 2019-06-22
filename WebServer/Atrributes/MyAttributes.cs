using ServisTakip.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace ServisTakip.Attributes
{
    public class FirmaRoleControl : ActionFilterAttribute, IActionFilter
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // action çalışmadan önce yapılacak işlemler

            var servisSistem = new ServisTakipEntities();


            if (HttpContext.Current.Session["firmaLogin"] == null || HttpContext.Current.Session["GirilenYer"].ToString() != "Firma")
            {//SESSİON YOKSA COOKİE KONTROL ET
               
                filterContext.HttpContext.Response.Redirect("~/Firma/FirmaGiris?returnUrl=" + filterContext.HttpContext.Request.FilePath.Substring(7).ToString());
            }
            else
            {
                string firEmail = HttpContext.Current.Session["firEmail"].ToString();
                int firId = Convert.ToInt32(HttpContext.Current.Session["firId"]);

                var firma_rol = servisSistem.Firmalar.Where(x =>x.firId == firId).FirstOrDefault();
                if (firma_rol == null)
                {
                    filterContext.HttpContext.Response.Redirect("~/Firma/Anasayfa");
                    //(string)filterContext.RouteData.Values.Values.LastOrDefault();
                }
            }
            base.OnActionExecuting(filterContext);


        }
        
    }
    public class OkulRoleControl : ActionFilterAttribute, IActionFilter
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // action çalışmadan önce yapılacak işlemler

            var servisSistem = new ServisTakipEntities();


            if (HttpContext.Current.Session["okulLogin"] == null || HttpContext.Current.Session["GirilenYer"].ToString() != "Okul")
            {//SESSİON YOKSA COOKİE KONTROL ET
                
                filterContext.HttpContext.Response.Redirect("~/Okul/OkulGiris?returnUrl=" + filterContext.HttpContext.Request.FilePath.Substring(6).ToString());
            }
            else
            {
                string mudurEmail = HttpContext.Current.Session["mudurEmail"].ToString();
                int mudurId = Convert.ToInt32(HttpContext.Current.Session["mudurId"]);

                var mudur_rol = servisSistem.Mudurler.Where(x => x.mudurId == mudurId).FirstOrDefault();
                if (mudur_rol == null)
                {
                    filterContext.HttpContext.Response.Redirect("~/Okul/Anasayfa");
                    //(string)filterContext.RouteData.Values.Values.LastOrDefault();
                }
            }
            base.OnActionExecuting(filterContext);


        }

        
    }
    public class SoforRoleControl : ActionFilterAttribute, IActionFilter
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // action çalışmadan önce yapılacak işlemler

            var servisSistem = new ServisTakipEntities();


            if (HttpContext.Current.Session["soforLogin"] == null || HttpContext.Current.Session["GirilenYer"].ToString() != "Sofor")
            {//SESSİON YOKSA COOKİE KONTROL ET

                filterContext.HttpContext.Response.Redirect("~/Sofor/SoforGiris?returnUrl=" + filterContext.HttpContext.Request.FilePath.Substring(7).ToString());
            }
            else
            {
                string sofEmail = HttpContext.Current.Session["sofEmail"].ToString();
                int sofId = Convert.ToInt32(HttpContext.Current.Session["sofId"]);

                var sofor_rol = servisSistem.Soforler.Where(x => x.sofId == sofId).FirstOrDefault();
                if (sofor_rol == null)
                {
                    filterContext.HttpContext.Response.Redirect("~/Sofor/Okullar");
                    //(string)filterContext.RouteData.Values.Values.LastOrDefault();
                }
            }
            base.OnActionExecuting(filterContext);


        }

    }
    public class VeliRoleControl : ActionFilterAttribute, IActionFilter
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // action çalışmadan önce yapılacak işlemler

            var servisSistem = new ServisTakipEntities();


            if (HttpContext.Current.Session["veliLogin"] == null || HttpContext.Current.Session["GirilenYer"].ToString() != "Veli")
            {//SESSİON YOKSA COOKİE KONTROL ET

                filterContext.HttpContext.Response.Redirect("~/Veli/VeliGiris?returnUrl=" + filterContext.HttpContext.Request.FilePath.Substring(6).ToString());
            }
            else
            {
                string veliEmail = HttpContext.Current.Session["veliEmail"].ToString();
                int veliId = Convert.ToInt32(HttpContext.Current.Session["veliId"]);

                var veli_rol = servisSistem.Veliler.Where(x => x.veliId == veliId).FirstOrDefault();
                if (veli_rol == null)
                {
                    filterContext.HttpContext.Response.Redirect("~/Veli/Anasayfa");
                    //(string)filterContext.RouteData.Values.Values.LastOrDefault();
                }
            }
            base.OnActionExecuting(filterContext);


        }


    }
}