using Rework;
using ServisTakip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServisTakip.Controllers
{
    public class VeliController : Controller
    {
        public ServisTakipEntities db = new ServisTakipEntities();

        #region // Sayfa Çağırımları
        public ActionResult Cikis(VeliGirisModel LoginModel)
        {
            VeliCookieControl veliCookie = new VeliCookieControl();
            veliCookie.CookieSil();
            Session.RemoveAll();
            return RedirectToAction("VeliGiris", "Veli");
        }
        public ActionResult VeliGiris(VeliGirisModel LoginModel)
        {
            VeliCookieControl veliCookie = new VeliCookieControl();
            Session.RemoveAll();

            if (veliCookie.CookieGetir() != null)
            {
                HttpCookie cookie = veliCookie.CookieGetir();
                string veliEmail = cookie["veliEmail"].ToString();
                string veliParola = cookie["veliParola"].ToString();
                var veli = db.Veliler.Where(x => x.veliEmail == veliEmail && x.veliParola == veliParola).FirstOrDefault();
                if (veli != null)
                {
                    veliCookie.CookieSil();
                    Veliler veliBilgisi = new Veliler
                    {
                        veliAd = veli.veliAd,
                        veliSoyad = veli.veliSoyad,
                        veliEmail = veli.veliEmail,
                        veliParola = veli.veliParola
                    };
                    veliCookie.CookieKaydet(veliBilgisi);
                    
                    Session["veliAd"] = veli.veliAd;
                    Session["veliSoyad"] = veli.veliSoyad;
                    Session["girenid"] = veli.veliId;
                    Session["veliId"] = veli.veliId;
                    Session["veliEmail"] = veli.veliEmail;
                    Session["veliLogin"] = true;
                    Session["GirilenYer"] = "Veli";

                    if (LoginModel.returnUrl != null)
                    {//Eğer çıkış yap butonundan buraya getirilmediyse
                        return RedirectToAction(LoginModel.returnUrl.Split('&')[0], "Veli");
                    }
                    else
                    {//Eğer çıkış yap butonundan buraya getirildiyse
                        return RedirectToAction("Anasayfa", "Veli");
                    }

                }
                else
                {
                    TempData["Mesaj"] = "Email / şifre bilgileriniz hatalıdır. Lütfen tekrar deneyiniz.";
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    string veliParola = LoginModel.veliParola.ToSHA(Crypto.SHA_Type.SHA256);
                    var veli = db.Veliler.Where(x => x.veliEmail == LoginModel.veliEmail && x.veliParola == veliParola).FirstOrDefault();
                    if (veli != null)
                    {
                        veliCookie.CookieSil();
                        Veliler veliBilgisi = new Veliler
                        {
                            veliAd = veli.veliAd,
                            veliSoyad = veli.veliSoyad,
                            veliEmail = veli.veliEmail,
                            veliParola = veli.veliParola
                        };
                        veliCookie.CookieKaydet(veliBilgisi);
                        Session["veliAd"] = veli.veliAd;
                        Session["veliSoyad"] = veli.veliSoyad;
                        Session["girenid"] = veli.veliId;
                        Session["veliId"] = veli.veliId;
                        Session["veliEmail"] = veli.veliEmail;
                        Session["veliLogin"] = true;
                        Session["GirilenYer"] = "Veli";

                        if (LoginModel.returnUrl != null)
                        {//Eğer çıkış yap butonundan buraya getirilmediyse
                            return RedirectToAction(LoginModel.returnUrl.Split('&')[0], "Veli");
                        }
                        else
                        {//Eğer çıkış yap butonundan buraya getirildiyse
                            return RedirectToAction("Anasayfa", "Veli");
                        }
                        //return RedirectToAction("Anasayfa", "Firma");
                    }
                    else
                    {
                        TempData["Mesaj"] = "Email / şifre bilgileriniz hatalıdır. Lütfen tekrar deneyiniz.";
                    }
                }
                else
                {
                    TempData["Mesaj"] = "Lütfen tüm bilgileri eksiksiz doldurunuz.";
                }
            }
            return View();
        }

        [Attributes.VeliRoleControl]
        public ActionResult Anasayfa()
        {
            int veliId = (int)Session["veliId"];
            if (true == (bool)Session["veliLogin"])
            {
                ViewModel vm = new ViewModel
                {
                    Duraklar = db.Duraklar.ToList(),
                    Faturalar = db.Faturalar.ToList(),
                    Firmalar = db.Firmalar.ToList(),
                    FirmaSoforleri = db.FirmaSoforleri.ToList(),
                    Ilceler = db.Ilceler.ToList(),
                    Iller = db.Iller.ToList(),
                    IndiBindiler = db.IndiBindiler.ToList(),
                    Mudurler = db.Mudurler.ToList(),
                    Odemeler = db.Odemeler.ToList(),
                    Ogrenciler = db.Ogrenciler.ToList(),
                    OgrenciVelileri = db.OgrenciVelileri.Where(x => x.veliId == veliId).ToList(),
                    OkulCinsleri = db.OkulCinsleri.ToList(),
                    OkulTurleri = db.OkulTurleri.ToList(),
                    Okullar = db.Okullar.ToList(),
                    OkulServisleri = db.OkulServisleri.ToList(),
                    Rotalar = db.Rotalar.ToList(),
                    Araclar = db.Araclar.ToList(),
                    ServistekiOgrenciler = db.ServistekiOgrenciler.ToList(),
                    AracSoforleri = db.AracSoforleri.ToList(),
                    Soforler = db.Soforler.ToList(),
                    FirmaAraclari = db.FirmaAraclari.ToList(),
                    Veliler = db.Veliler.Where(x=>x.veliId==veliId).ToList()
                };
                return View(vm);
            }
            return null;
        }
        [Attributes.VeliRoleControl]
        public ActionResult Sorumluluklar()
        {
            int veliId = (int)Session["veliId"];
            if (true == (bool)Session["veliLogin"])
            {
                ViewModel vm = new ViewModel
                {
                    Duraklar = db.Duraklar.ToList(),
                    Faturalar = db.Faturalar.ToList(),
                    Firmalar = db.Firmalar.ToList(),
                    FirmaSoforleri = db.FirmaSoforleri.ToList(),
                    Ilceler = db.Ilceler.ToList(),
                    Iller = db.Iller.ToList(),
                    IndiBindiler = db.IndiBindiler.ToList(),
                    Mudurler = db.Mudurler.ToList(),
                    Odemeler = db.Odemeler.ToList(),
                    Ogrenciler = db.Ogrenciler.ToList(),
                    OgrenciVelileri = db.OgrenciVelileri.Where(x => x.veliId == veliId).ToList(),
                    OkulCinsleri = db.OkulCinsleri.ToList(),
                    OkulTurleri = db.OkulTurleri.ToList(),
                    Okullar = db.Okullar.ToList(),
                    OkulServisleri = db.OkulServisleri.ToList(),
                    Rotalar = db.Rotalar.ToList(),
                    Araclar = db.Araclar.ToList(),
                    ServistekiOgrenciler = db.ServistekiOgrenciler.ToList(),
                    AracSoforleri = db.AracSoforleri.ToList(),
                    Soforler = db.Soforler.ToList(),
                    FirmaAraclari = db.FirmaAraclari.ToList(),
                    Veliler = db.Veliler.Where(x => x.veliId == veliId).ToList()
                };
                return View(vm);
            }
            return null;
        }

        [Attributes.VeliRoleControl]
        public ActionResult Araclar()
        {
            int veliId = (int)Session["veliId"];
            if (true == (bool)Session["veliLogin"])
            {
                ViewModel vm = new ViewModel
                {
                    Duraklar = db.Duraklar.ToList(),
                    Faturalar = db.Faturalar.ToList(),
                    Firmalar = db.Firmalar.ToList(),
                    FirmaSoforleri = db.FirmaSoforleri.ToList(),
                    Ilceler = db.Ilceler.ToList(),
                    Iller = db.Iller.ToList(),
                    IndiBindiler = db.IndiBindiler.ToList(),
                    Mudurler = db.Mudurler.ToList(),
                    Odemeler = db.Odemeler.ToList(),
                    Ogrenciler = db.Ogrenciler.ToList(),
                    OgrenciVelileri = db.OgrenciVelileri.Where(x => x.veliId == veliId).ToList(),
                    OkulCinsleri = db.OkulCinsleri.ToList(),
                    OkulTurleri = db.OkulTurleri.ToList(),
                    Okullar = db.Okullar.ToList(),
                    OkulServisleri = db.OkulServisleri.ToList(),
                    Rotalar = db.Rotalar.ToList(),
                    Araclar = db.Araclar.ToList(),
                    ServistekiOgrenciler = db.ServistekiOgrenciler.ToList(),
                    AracSoforleri = db.AracSoforleri.ToList(),
                    Soforler = db.Soforler.ToList(),
                    FirmaAraclari = db.FirmaAraclari.ToList(),
                    Veliler = db.Veliler.Where(x => x.veliId == veliId).ToList()
                };
                return View(vm);
            }
            return null;
        }

        #endregion

        [HttpPost]
        [Attributes.VeliRoleControl]
        public ActionResult VeliGuncelle(Veliler veliBilgisi)
        {
            if (veliBilgisi.veliId == (int)Session["veliId"])
            {
                try
                {
                    var veli = db.Veliler.Where(x => x.veliId == veliBilgisi.veliId).First();
                    veli.veliAd = veliBilgisi.veliAd;
                    veli.veliSoyad = veli.veliSoyad;
                    veli.veliAdres = veliBilgisi.veliAdres;
                    veli.veliGsm = veliBilgisi.veliGsm;
                    veli.ilce_id = veliBilgisi.ilce_id;
                    if (veli.veliParola.Length > 0)
                    {
                        veli.veliParola = veliBilgisi.veliParola.ToSHA(Crypto.SHA_Type.SHA256);
                    }
                    db.SaveChanges();
                    TempData["Mesaj"] = "İşlem Başarılı.";
                    return RedirectToAction("Ayarlar", "Veli");
                    //return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    Console.Write(e);
                    TempData["Mesaj"] = "Bir hata oluştu. Lütfen tekrar deneyiniz.";
                    return RedirectToAction("Ayarlar", "Veli");
                    //return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                TempData["Mesaj"] = "Bu fonksiyonu çalıştırma yetkiniz bulunmamaktadır.";
                return RedirectToAction("VeliGiris", "Veli");
                //return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }
        private class LatLng
        {
            public string lat { get; set; }
            public string lng { get; set; }
        }

        [HttpPost]
        [Attributes.VeliRoleControl]
        public JsonResult ServisKonumGetir(int veliId, int aracId)
        {
            if (veliId == Convert.ToInt32(Session["veliId"]))
            {
                try
                {
                    if (db.Araclar.Where(x => x.aracId == aracId).Count() > 0)
                    {
                        var servis = db.Araclar.Find(aracId);
                        LatLng latlng = new LatLng
                        {
                            lat = servis.latitude,
                            lng = servis.longitude
                        };
                        return Json(latlng, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(1, JsonRequestBehavior.AllowGet);

                    }

                }
                catch (Exception e)
                {
                    Console.Write(e);
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                TempData["Mesaj"] = "Bu fonksiyonu çalıştırma yetkiniz bulunmamaktadır.";
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
