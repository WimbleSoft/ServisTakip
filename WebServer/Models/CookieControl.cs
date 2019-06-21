using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServisTakip.Models
{
    public class MudurCookieControl
    {
        public void CookieKaydet(Mudurler mudurBilgi)
        {
            HttpCookie Cookie = null;
            if (HttpContext.Current.Response.Cookies["mudurCookie"] != null)
            {
                //Cookie varsa devam.
                Cookie = HttpContext.Current.Response.Cookies["mudurCookie"];
            }
            else
            {
                //Yoksa oluşturuyoruz.
                Cookie = new HttpCookie("mudurCookie");
            }
            Cookie.Expires = DateTime.Now.AddDays(7);
            Cookie["mudurAd"] = mudurBilgi.mudurAd;
            Cookie["mudurSoyad"] = mudurBilgi.mudurSoyad;
            Cookie["mudurEmail"] = mudurBilgi.mudurEmail;
            Cookie["mudurParola"] = mudurBilgi.mudurParola;

            HttpContext.Current.Response.Cookies.Add(Cookie);
        }

        public HttpCookie CookieGetir()
        {
            if (HttpContext.Current.Request.Cookies["mudurCookie"] != null)
                return HttpContext.Current.Request.Cookies["mudurCookie"];
            else
                return null;
        }

        public void CookieSil() => HttpContext.Current.Response.Cookies["mudurCookie"].Expires = DateTime.Now.AddDays(-1);
    }
    public class FirmaCookieControl
    {
        public void CookieKaydet(Firmalar firmaBilgi)
        {
            HttpCookie Cookie = null;
            if (HttpContext.Current.Response.Cookies["firmaCookie"] != null)
            {
                //Cookie varsa devam.
                Cookie = HttpContext.Current.Response.Cookies["firmaCookie"];
            }
            else
            {
                //Yoksa oluşturuyoruz.
                Cookie = new HttpCookie("firmaCookie");
            }
            Cookie.Expires = DateTime.Now.AddDays(7);
            Cookie["firAd"] = firmaBilgi.firAd;
            Cookie["firEmail"] = firmaBilgi.firEmail;
            Cookie["firParola"] = firmaBilgi.firParola;

            HttpContext.Current.Response.Cookies.Add(Cookie);
        }

        public HttpCookie CookieGetir()
        {
            if (HttpContext.Current.Request.Cookies["firmaCookie"] != null)
                return HttpContext.Current.Request.Cookies["firmaCookie"];
            else
                return null;
        }

        public void CookieSil() => HttpContext.Current.Response.Cookies["firmaCookie"].Expires = DateTime.Now.AddDays(-1);
    }
    public class VeliCookieControl
    {
        public void CookieKaydet(Veliler veliBilgi)
        {
            HttpCookie Cookie = null;
            if (HttpContext.Current.Response.Cookies["veliCookie"] != null)
            {
                //Cookie varsa devam.
                Cookie = HttpContext.Current.Response.Cookies["veliCookie"];
            }
            else
            {
                //Yoksa oluşturuyoruz.
                Cookie = new HttpCookie("veliCookie");
            }
            Cookie.Expires = DateTime.Now.AddDays(7);
            Cookie["veliAd"] = veliBilgi.veliAd;
            Cookie["veliEmail"] = veliBilgi.veliEmail;
            Cookie["veliParola"] = veliBilgi.veliParola;

            HttpContext.Current.Response.Cookies.Add(Cookie);
        }

        public HttpCookie CookieGetir()
        {
            if (HttpContext.Current.Request.Cookies["veliCookie"] != null)
                return HttpContext.Current.Request.Cookies["veliCookie"];
            else
                return null;
        }

        public void CookieSil() => HttpContext.Current.Response.Cookies["veliCookie"].Expires = DateTime.Now.AddDays(-1);
    }
    public class SoforCookieControl
    {
        public void CookieKaydet(Soforler soforBilgi, string plaka)
        {
            HttpCookie Cookie = null;
            if (HttpContext.Current.Response.Cookies["soforCookie"] != null)
            {
                //Cookie varsa devam.
                Cookie = HttpContext.Current.Response.Cookies["soforCookie"];
            }
            else
            {
                //Yoksa oluşturuyoruz.
                Cookie = new HttpCookie("soforCookie");
            }
            Cookie.Expires = DateTime.Now.AddDays(7);
            Cookie["sofAd"] = soforBilgi.sofAd;
            Cookie["sofEmail"] = soforBilgi.sofEmail;
            Cookie["sofParola"] = soforBilgi.sofParola;
            Cookie["plaka"] = plaka;

            HttpContext.Current.Response.Cookies.Add(Cookie);
        }

        public HttpCookie CookieGetir()
        {
            if (HttpContext.Current.Request.Cookies["soforCookie"] != null)
                return HttpContext.Current.Request.Cookies["soforCookie"];
            else
                return null;
        }

        public void CookieSil() => HttpContext.Current.Response.Cookies["soforCookie"].Expires = DateTime.Now.AddDays(-1);
    }
}