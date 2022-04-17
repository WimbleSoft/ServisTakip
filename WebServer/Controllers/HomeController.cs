using Rework;
using ServisTakip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServisTakip.Controllers
{
    public class HomeController : Controller
    {
        public ServisTakipEntities db = new ServisTakipEntities();
        // GET: Home
        public ActionResult Anasayfa()
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
                OgrenciVelileri = db.OgrenciVelileri.ToList(),
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
                Veliler = db.Veliler.ToList()
            };
            return View(vm);
        }
        public ActionResult Giris()
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
                OgrenciVelileri = db.OgrenciVelileri.ToList(),
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
                Veliler = db.Veliler.ToList()
            };
            return View(vm);
        }

        public ActionResult Kayit()
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
                OgrenciVelileri = db.OgrenciVelileri.ToList(),
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
                Veliler = db.Veliler.ToList()
            };
            return View(vm);
        }

        [HttpPost]
        public ActionResult VeliKayitOl(Veliler veliBilgisi)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string veliParola = veliBilgisi.veliParola;
                    veliBilgisi.veliParola = veliBilgisi.veliParola.ToSHA(Crypto.SHA_Type.SHA256);
                    db.Veliler.Add(veliBilgisi);
                    VeliCookieControl veliCookie = new VeliCookieControl();
                    veliCookie.CookieSil();
                    TempData["Mesaj"] = "Kaydınız başarılı bir şekilde oluşturuldu. Artık size okullar tarafından öğrenci atamaları yapılabilir.";
                    TempData["btn-renk"] = "btn-success";
                    db.SaveChanges();
                    return RedirectToAction("VeliGiris", "Veli", new { veliEmail = veliBilgisi.veliEmail, veliParola = veliParola, returnUrl = "Anasayfa" });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    TempData["Mesaj"] = "Bir hata olustu. Lutfen tekrar deneyiniz.";
                    TempData["btn-renk"] = "btn-danger";
                    return RedirectToAction("Kayit", "Home");
                }
            }
            else
            {
                TempData["Mesaj"] = "Lütfen gerekli tüm alanları doldurun.";
                TempData["btn-renk"] = "btn-warning";
                return RedirectToAction("Kayit", "Home");
            }
            
        }

        [HttpPost]
        public ActionResult SoforKayitOl(Soforler soforBilgisi)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    soforBilgisi.sofParola = soforBilgisi.sofParola.ToSHA(Crypto.SHA_Type.SHA256);
                    db.Soforler.Add(soforBilgisi);
                    SoforCookieControl soforCookie = new SoforCookieControl();
                    soforCookie.CookieSil();
                    TempData["Mesaj"] = "Kaydınız başarılı bir şekilde oluşturuldu. Artık size firmalar tarafından servis atamaları yapılabilir.";
                    TempData["btn-renk"] = "btn-success";
                    db.SaveChanges();
                    return RedirectToAction("SoforGiris", "Sofor");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    TempData["Mesaj"] = "Bir hata olustu. Lutfen tekrar deneyiniz.";
                    TempData["btn-renk"] = "btn-danger";
                    return RedirectToAction("Kayit", "Home");
                }
            }
            else
            {

            }
            TempData["Mesaj"] = "Lütfen gerekli tüm alanları doldurun.";
            TempData["btn-renk"] = "btn-warning";
            return RedirectToAction("Kayit", "Home");
        }

        [HttpPost]
        public ActionResult OkulKayitOl(Mudurler mudurBilgisi, Okullar okulBilgisi)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string mudurParola = mudurBilgisi.mudurParola;
                    mudurBilgisi.mudurParola = mudurBilgisi.mudurParola.ToSHA(Crypto.SHA_Type.SHA256);
                    mudurBilgisi.okulId = db.Okullar.Add(okulBilgisi).okulId;
                    db.Mudurler.Add(mudurBilgisi);
                    MudurCookieControl mudurCookie = new MudurCookieControl();
                    mudurCookie.CookieSil();
                    TempData["Mesaj"] = "Kaydınız başarılı bir şekilde oluşturuldu. Artık okulunuzun yönetimine başlayabilirsiniz.";
                    TempData["btn-renk"] = "btn-success";
                    db.SaveChanges();
                    return RedirectToAction("OkulGiris", "Okul", new { mudurEmail = mudurBilgisi.mudurEmail, mudurParola = mudurParola, returnUrl = "Anasayfa" });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    TempData["Mesaj"] = "Bir hata olustu. Lutfen tekrar deneyiniz.";
                    TempData["btn-renk"] = "btn-danger";
                    return RedirectToAction("Kayit", "Home");
                }
            }
            else
            {
                TempData["Mesaj"] = "Lütfen gerekli tüm alanları doldurun.";
                TempData["btn-renk"] = "btn-warning";
                return RedirectToAction("Kayit", "Home");
            }
            
        }

        [HttpPost]
        public ActionResult FirmaKayitOl(Firmalar firmaBilgisi)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string firParola = firmaBilgisi.firParola;
                    firmaBilgisi.firParola = firmaBilgisi.firParola.ToSHA(Crypto.SHA_Type.SHA256);
                    db.Firmalar.Add(firmaBilgisi);
                    FirmaCookieControl firmaCookie = new FirmaCookieControl();
                    firmaCookie.CookieSil();
                    TempData["Mesaj"] = "Kaydınız başarılı bir şekilde oluşturuldu. Artık firmanıza servis, şoför ve şoförlere servis atamaları yapabilirsiniz.";
                    TempData["btn-renk"] = "btn-success";
                    db.SaveChanges();
                    return RedirectToAction("FirmaGiris", "Firma", new { firEmail = firmaBilgisi.firEmail, firParola = firParola, returnUrl = "Anasayfa" });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    TempData["Mesaj"] = "Bir hata olustu. Lutfen tekrar deneyiniz.";
                    TempData["btn-renk"] = "btn-danger";
                    return RedirectToAction("Kayit", "Home");
                }
            }
            else
            {

            }
            TempData["Mesaj"] = "Lütfen gerekli tüm alanları doldurun.";
            TempData["btn-renk"] = "btn-warning";
            return RedirectToAction("Kayit", "Home");
        }
    }
}