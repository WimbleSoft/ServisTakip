using Rework;
using ServisTakip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ServisTakip.Controllers
{
    public class OkulController : Controller
    {
        public ServisTakipEntities db = new ServisTakipEntities();

        [Attributes.OkulRoleControl]
        public async Task<bool> Email(EmailFormModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var body = "<p>{0} ({1})</p><p>{2}</p>";
                    var message = new MailMessage();
                    message.To.Add(new MailAddress(model.toEmail));  // replace with valid value 
                    message.From = new MailAddress("sau_bitirme2017@outlook.com");  // replace with valid value
                    message.Subject = model.subject;
                    message.Body = string.Format(body, model.fromName, model.fromEmail, model.message);
                    message.IsBodyHtml = true;

                    using (var smtp = new SmtpClient())
                    {
                        var credential = new NetworkCredential
                        {
                            UserName = "sau_bitirme2017@outlook.com",  // replace with valid value
                            Password = "YMT.2017"  // replace with valid value
                        };
                        smtp.Credentials = credential;
                        //smtp.Host = "smtp.gmail.com";
                        //smtp.Port = 465;
                        smtp.Host="smtp-mail.outlook.com";
                        smtp.Port = 587;
                        smtp.EnableSsl = true;
                        await smtp.SendMailAsync(message);
                    }
                    return true;
                }
                catch (Exception e)
                {
                    Console.Write(e);
                    return false;
                }
            }
            return false;
        }

        #region // Sayfa Çağırımları
        public ActionResult Cikis(MudurGirisModel LoginModel)
        {
            MudurCookieControl mudurCookie = new MudurCookieControl();
            mudurCookie.CookieSil();
            Session.RemoveAll();
            return RedirectToAction("OkulGiris", "Okul");
        }
        public ActionResult OkulGiris(MudurGirisModel LoginModel)
        {
            MudurCookieControl mudurCookie = new MudurCookieControl();
            

            if (mudurCookie.CookieGetir() != null)
            {
                HttpCookie cookie = mudurCookie.CookieGetir();
                string mudurEmail = cookie["mudurEmail"].ToString();
                string mudurParola = cookie["mudurParola"].ToString();
                var mudur = db.Mudurler.Where(x => x.mudurEmail == mudurEmail && x.mudurParola == mudurParola).FirstOrDefault();
                if (mudur != null)
                {
                    mudurCookie.CookieSil();
                    Mudurler mudurBilgisi = new Mudurler
                    {
                        mudurAd = mudur.mudurAd,
                        mudurSoyad = mudur.mudurSoyad,
                        mudurEmail = mudur.mudurEmail,
                        mudurParola = mudur.mudurParola
                    };
                    mudurCookie.CookieKaydet(mudurBilgisi);
                    Session["okulId"] = mudur.Okullar.okulId;
                    Session["mudurAd"] = mudur.mudurAd;
                    Session["mudurSoyad"] = mudur.mudurSoyad;
                    Session["girenid"] = mudur.mudurId;
                    Session["mudurId"] = mudur.mudurId;
                    Session["mudurEmail"] = mudur.mudurEmail;
                    Session["okulLogin"] = true;
                    Session["GirilenYer"] = "Okul";

                    if (LoginModel.returnUrl != null)
                    {//Eğer çıkış yap butonundan buraya getirilmediyse
                        return RedirectToAction(LoginModel.returnUrl.Split('&')[0], "Okul");
                    }
                    else
                    {//Eğer çıkış yap butonundan buraya getirildiyse
                        return RedirectToAction("Anasayfa", "Okul");
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
                    string mudurParola = LoginModel.mudurParola.ToSHA(Crypto.SHA_Type.SHA256);
                    var mudur = db.Mudurler.Where(x => x.mudurEmail == LoginModel.mudurEmail && x.mudurParola == mudurParola).FirstOrDefault();
                    if (mudur != null)
                    {
                        mudurCookie.CookieSil();
                        Mudurler mudurBilgisi = new Mudurler
                        {
                            mudurAd = mudur.mudurAd,
                            mudurSoyad = mudur.mudurSoyad,
                            mudurEmail = mudur.mudurEmail,
                            mudurParola = mudur.mudurParola
                        };
                        mudurCookie.CookieKaydet(mudurBilgisi);
                        Session["okulId"] = mudur.Okullar.okulId;
                        Session["mudurAd"] = mudur.mudurAd;
                        Session["mudurSoyad"] = mudur.mudurSoyad;
                        Session["girenid"] = mudur.mudurId;
                        Session["mudurId"] = mudur.mudurId;
                        Session["mudurEmail"] = mudur.mudurEmail;
                        Session["okulLogin"] = true;
                        Session["GirilenYer"] = "Okul";

                        if (LoginModel.returnUrl != null)
                        {//Eğer çıkış yap butonundan buraya getirilmediyse
                            return RedirectToAction(LoginModel.returnUrl.Split('&')[0], "Okul");
                        }
                        else
                        {//Eğer çıkış yap butonundan buraya getirildiyse
                            return RedirectToAction("Anasayfa", "Okul");
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

        [Attributes.OkulRoleControl]
        public ActionResult Anasayfa()
        {
            int okulId = (int)Session["okulId"];
            if (true == (bool)Session["okulLogin"])
            {
                ViewModel vm = new ViewModel
                {
                    Duraklar = db.Duraklar.ToList(),
                    //Faturalar = db.Faturalar.Where(x=>x.Araclar.FirmaAraclari.Where(y=>y.firId==firId).Count()>0).ToList(),
                    Firmalar = db.Firmalar.ToList(),
                    FirmaSoforleri = db.FirmaSoforleri.ToList(),
                    Ilceler = db.Ilceler.ToList(),
                    Iller = db.Iller.ToList(),
                    IndiBindiler = db.IndiBindiler.ToList(),
                    Mudurler = db.Mudurler.ToList(),
                    //Odemeler = db.Odemeler.Where(x=>x.Faturalar.Araclar.FirmaAraclari.Where(y=>y.firId == firId).Count()>0).ToList(),
                    Ogrenciler = db.Ogrenciler.ToList(),
                    OgrenciVelileri = db.OgrenciVelileri.ToList(),
                    OkulCinsleri = db.OkulCinsleri.ToList(),
                    OkulTurleri = db.OkulTurleri.ToList(),
                    Okullar = db.Okullar.Where(x => x.okulId == okulId).ToList(),
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
            return null;
        }

        [Attributes.OkulRoleControl]
        public ActionResult OkulServisleri()
        {
            int okulId = (int)Session["okulId"];
            if (true == (bool)Session["okulLogin"])
            {
                ViewModel vm = new ViewModel
                {
                    Duraklar = db.Duraklar.ToList(),
                    //Faturalar = db.Faturalar.Where(x=>x.Araclar.FirmaAraclari.Where(y=>y.firId==firId).Count()>0).ToList(),
                    Firmalar = db.Firmalar.ToList(),
                    FirmaSoforleri = db.FirmaSoforleri.ToList(),
                    Ilceler = db.Ilceler.ToList(),
                    Iller = db.Iller.ToList(),
                    IndiBindiler = db.IndiBindiler.ToList(),
                    Mudurler = db.Mudurler.Where(x => x.okulId == okulId).ToList(),
                    //Odemeler = db.Odemeler.Where(x=>x.Faturalar.Araclar.FirmaAraclari.Where(y=>y.firId == firId).Count()>0).ToList(),
                    Ogrenciler = db.Ogrenciler.Where(x => x.okulId == okulId).ToList(),
                    OgrenciVelileri = db.OgrenciVelileri.ToList(),
                    OkulCinsleri = db.OkulCinsleri.ToList(),
                    OkulTurleri = db.OkulTurleri.ToList(),
                    Okullar = db.Okullar.Where(x => x.okulId == okulId).ToList(),
                    OkulServisleri = db.OkulServisleri.Where(x => x.okulId == okulId).ToList(),
                    Rotalar = db.Rotalar.ToList(),
                    Araclar = db.Araclar.ToList(),
                    ServistekiOgrenciler = db.ServistekiOgrenciler.ToList(),
                    AracSoforleri = db.AracSoforleri.ToList(),
                    Soforler = db.Soforler.ToList(),
                    FirmaAraclari = db.FirmaAraclari.ToList(),
                    Veliler = db.Veliler.ToList()
                };
                ViewBag.okulLatitude = db.Okullar.Find(okulId).latitude;
                ViewBag.okulLongitude = db.Okullar.Find(okulId).longitude;
                return View(vm);
            }
            return null;
        }

        [Attributes.OkulRoleControl]
        public ActionResult Ogrenciler()
        {
            int okulId = (int)Session["okulId"];
            if (true == (bool)Session["okulLogin"])
            {
                ViewModel vm = new ViewModel
                {
                    Duraklar = db.Duraklar.ToList(),
                    //Faturalar = db.Faturalar.Where(x=>x.Araclar.FirmaAraclari.Where(y=>y.firId==firId).Count()>0).ToList(),
                    Firmalar = db.Firmalar.ToList(),
                    FirmaSoforleri = db.FirmaSoforleri.ToList(),
                    Ilceler = db.Ilceler.ToList(),
                    Iller = db.Iller.ToList(),
                    IndiBindiler = db.IndiBindiler.ToList(),
                    Mudurler = db.Mudurler.Where(x => x.okulId == okulId).ToList(),
                    //Odemeler = db.Odemeler.Where(x=>x.Faturalar.Araclar.FirmaAraclari.Where(y=>y.firId == firId).Count()>0).ToList(),
                    Ogrenciler = db.Ogrenciler.Where(x => x.okulId == okulId).ToList(),
                    OgrenciVelileri = db.OgrenciVelileri.ToList(),
                    OkulCinsleri = db.OkulCinsleri.ToList(),
                    OkulTurleri = db.OkulTurleri.ToList(),
                    Okullar = db.Okullar.Where(x => x.okulId == okulId).ToList(),
                    OkulServisleri = db.OkulServisleri.Where(x => x.okulId == okulId).ToList(),
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
            return null;
        }

        [Attributes.OkulRoleControl]
        public ActionResult OgrenciDetay(int ogrId)
        {
            int okulId = (int)Session["okulId"];
            if (true == (bool)Session["okulLogin"])
            {
                if (db.Ogrenciler.Find(ogrId) != null)
                {
                    ViewModel vm = new ViewModel
                    {
                        Duraklar = db.Duraklar.Where(x => x.ogrId == ogrId).ToList(),
                        //Faturalar = db.Faturalar.Where(x=>x.Araclar.FirmaAraclari.Where(y=>y.firId==firId).Count()>0).ToList(),
                        Firmalar = db.Firmalar.ToList(),
                        FirmaSoforleri = db.FirmaSoforleri.ToList(),
                        Ilceler = db.Ilceler.ToList(),
                        Iller = db.Iller.ToList(),
                        IndiBindiler = db.IndiBindiler.ToList(),
                        Mudurler = db.Mudurler.Where(x => x.okulId == okulId).ToList(),
                        Ogrenciler = db.Ogrenciler.Where(x => x.okulId == okulId).ToList(),
                        OgrenciVelileri = db.OgrenciVelileri.Where(x=>x.ogrId==ogrId).ToList(),
                        OkulCinsleri = db.OkulCinsleri.ToList(),
                        OkulTurleri = db.OkulTurleri.ToList(),
                        Okullar = db.Okullar.Where(x => x.okulId == okulId).ToList(),
                        OkulServisleri = db.OkulServisleri.Where(x => x.okulId == okulId).ToList(),
                        Rotalar = db.Rotalar.ToList(),
                        Araclar = db.Araclar.ToList(),
                        ServistekiOgrenciler = db.ServistekiOgrenciler.ToList(),
                        AracSoforleri = db.AracSoforleri.ToList(),
                        Soforler = db.Soforler.ToList(),
                        FirmaAraclari = db.FirmaAraclari.ToList(),
                        Veliler = db.Veliler.ToList()
                    };
                    ViewBag.ogrId = ogrId;
                    ViewBag.okulLatitude = db.Okullar.Find(okulId).latitude;
                    ViewBag.okulLongitude = db.Okullar.Find(okulId).longitude;
                    ViewBag.servisLatitude = db.ServistekiOgrenciler.Where(x=>x.ogrId==ogrId).First().OkulServisleri.Araclar.latitude;
                    ViewBag.servisLongitude = db.ServistekiOgrenciler.Where(x => x.ogrId == ogrId).First().OkulServisleri.Araclar.longitude;
                    ViewBag.aracId = db.ServistekiOgrenciler.Where(x => x.ogrId == ogrId).First().OkulServisleri.aracId;
                    return View(vm);
                }
                else
                {
                    TempData["Mesaj"] = "Bir hata oluştu."; TempData["Btn-Renk"] = "btn-danger";
                    return RedirectToAction("OkulGiris", "Okul");
                }
                
            }
            return RedirectToAction("OkulGiris","Okul");
        }

        [Attributes.OkulRoleControl]
        public ActionResult Veliler()
        {
            int okulId = (int)Session["okulId"];
            if (true == (bool)Session["okulLogin"])
            {
                ViewModel vm = new ViewModel
                {
                    Duraklar = db.Duraklar.ToList(),
                    //Faturalar = db.Faturalar.Where(x=>x.Araclar.FirmaAraclari.Where(y=>y.firId==firId).Count()>0).ToList(),
                    Firmalar = db.Firmalar.ToList(),
                    FirmaSoforleri = db.FirmaSoforleri.ToList(),
                    Ilceler = db.Ilceler.ToList(),
                    Iller = db.Iller.ToList(),
                    IndiBindiler = db.IndiBindiler.ToList(),
                    Mudurler = db.Mudurler.Where(x => x.okulId == okulId).ToList(),
                    //Odemeler = db.Odemeler.Where(x=>x.Faturalar.Araclar.FirmaAraclari.Where(y=>y.firId == firId).Count()>0).ToList(),
                    Ogrenciler = db.Ogrenciler.Where(x => x.okulId == okulId).ToList(),
                    OgrenciVelileri = db.OgrenciVelileri.ToList(),
                    OkulCinsleri = db.OkulCinsleri.ToList(),
                    OkulTurleri = db.OkulTurleri.ToList(),
                    Okullar = db.Okullar.Where(x => x.okulId == okulId).ToList(),
                    OkulServisleri = db.OkulServisleri.Where(x => x.okulId == okulId).ToList(),
                    Rotalar = db.Rotalar.ToList(),
                    Araclar = db.Araclar.ToList(),
                    ServistekiOgrenciler = db.ServistekiOgrenciler.ToList(),
                    AracSoforleri = db.AracSoforleri.ToList(),
                    Soforler = db.Soforler.ToList(),
                    FirmaAraclari = db.FirmaAraclari.ToList(),
                    Veliler = db.Veliler.ToList()
                };
                ViewBag.okulId = okulId;
                return View(vm);
            }
            return null;
        }

        [Attributes.OkulRoleControl]
        public ActionResult AracSoforleri()
        {
            int okulId = (int)Session["okulId"];
            if (true == (bool)Session["okulLogin"])
            {
                ViewModel vm = new ViewModel
                {
                    Duraklar = db.Duraklar.ToList(),
                    //Faturalar = db.Faturalar.Where(x=>x.Araclar.FirmaAraclari.Where(y=>y.firId==firId).Count()>0).ToList(),
                    Firmalar = db.Firmalar.ToList(),
                    FirmaSoforleri = db.FirmaSoforleri.ToList(),
                    Ilceler = db.Ilceler.ToList(),
                    Iller = db.Iller.ToList(),
                    IndiBindiler = db.IndiBindiler.ToList(),
                    Mudurler = db.Mudurler.ToList(),
                    //Odemeler = db.Odemeler.Where(x=>x.Faturalar.Araclar.FirmaAraclari.Where(y=>y.firId == firId).Count()>0).ToList(),
                    Ogrenciler = db.Ogrenciler.ToList(),
                    OgrenciVelileri = db.OgrenciVelileri.ToList(),
                    OkulCinsleri = db.OkulCinsleri.ToList(),
                    OkulTurleri = db.OkulTurleri.ToList(),
                    Okullar = db.Okullar.Where(x => x.okulId == okulId).ToList(),
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

            return null;
        }
        
        #endregion

        #region // Okul Servisleri
        [HttpPost]
        [Attributes.OkulRoleControl]
        public ActionResult OkulServisEkle(OkulServisleri okulServisleri, string authCode)
        {
            if (okulServisleri.okulId == (int)Session["okulId"])
            {
                try
                {
                    if (db.Araclar.Where(x => x.aracId == okulServisleri.aracId && x.authCode == authCode).Count() > 0)
                    {
                        db.OkulServisleri.Add(okulServisleri);
                        db.SaveChanges();
                        TempData["Mesaj"] = "İşlem Başarılı."; TempData["Btn-Renk"] = "btn-info";
                        return RedirectToAction("OkulServisleri", "Okul");
                        //return Json(0, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        TempData["Mesaj"] = "Ne yazık ki, bu servise ait doğrulama kodunu yanlış girdiniz."; TempData["Btn-Renk"] = "btn-warning";
                        return RedirectToAction("OkulServisleri", "Okul");
                        //return Json(3, JsonRequestBehavior.AllowGet);
                    }

                }
                catch (Exception e)
                {
                    Console.Write(e);
                    TempData["Mesaj"] = "Bir hata oluştu. Lütfen tekrar deneyiniz."; TempData["Btn-Renk"] = "btn-danger";
                    return RedirectToAction("OkulServisleri", "Okul");
                    //return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                TempData["Mesaj"] = "Bu fonksiyonu çalıştırma yetkiniz bulunmamaktadır.";
                return RedirectToAction("OkulGiris", "Okul");
                //return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Attributes.OkulRoleControl]
        public ActionResult OkulServisSil(int okulAracId, int okulId)
        {
            if (okulId == (int)Session["okulId"])
            {
                try
                {
                    db.OkulServisleri.RemoveRange(db.OkulServisleri.Where(d => d.okulAracId == okulAracId));
                    db.SaveChanges();
                    TempData["Mesaj"] = "İşlem Başarılı."; TempData["Btn-Renk"] = "btn-info";
                    return RedirectToAction("OkulServisleri", "Okul");
                    //return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    Console.Write(e);
                    TempData["Mesaj"] = "Bir hata oluştu. Lütfen tekrar deneyiniz."; TempData["Btn-Renk"] = "btn-danger";
                    return RedirectToAction("OkulServisleri", "Okul");
                    //return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                TempData["Mesaj"] = "Bu fonksiyonu çalıştırma yetkiniz bulunmamaktadır. Lütfen yeniden giriş yapınız.";
                return RedirectToAction("OkulGiris", "Okul");
                //return Json(-1, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        [Attributes.OkulRoleControl]
        public JsonResult ServisKonumGetir(int okulId, int aracId)
        {
            if (okulId == (int)Session["okulId"])
            {
                try
                {
                    if (db.Araclar.Where(x=>x.aracId==aracId).Count()> 0)
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

        private class LatLng
        {
            public string lat { get; set; }
            public string lng { get; set; }
        }
        #endregion

        #region // Öğrenciler
        [Attributes.OkulRoleControl]
        [HttpPost]
        public ActionResult OgrenciEkle(Ogrenciler ogrenciBilgi)
        {
            if (ogrenciBilgi.okulId == (int)Session["okulId"])
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        db.Ogrenciler.Add(ogrenciBilgi);
                        db.SaveChanges();
                        TempData["Mesaj"] = "İşlem Başarılı";  TempData["Btn-Renk"] = "btn-success";
                        return RedirectToAction("Ogrenciler", "Okul");
                        //return Json(0, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception e)
                    {
                        Console.Write(e);
                        TempData["Mesaj"] = "Bir hata oluştu";  TempData["Btn-Renk"] = "btn-danger";;
                        return RedirectToAction("Ogrenciler", "Okul");
                        //return Json(1, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    TempData["Mesaj"] = "Lütfen tüm alanları doldurunuz";  TempData["Btn-Renk"] = "btn-warning";
                    return RedirectToAction("Ogrenciler", "Okul");
                    //return Json(2, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                TempData["Mesaj"] = "Bu fonksiyonu çalıştırma yetkiniz bulunmamaktadır. Lütfen yeniden giriş yapınız.";
                return RedirectToAction("OkulGiris", "Okul");
                //return Json(-1, JsonRequestBehavior.AllowGet);

            }

        }

        [Attributes.OkulRoleControl]
        [HttpPost]
        public ActionResult OgrenciGuncelle(Ogrenciler ogrenciBilgi)
        {
            if (ogrenciBilgi.okulId == (int)Session["okulId"])
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var ogrenci = db.Ogrenciler.Find(ogrenciBilgi.ogrId);
                        ogrenci.ogrAd = ogrenciBilgi.ogrAd;
                        ogrenci.ogrSoyad = ogrenciBilgi.ogrSoyad;
                        ogrenci.ogrTCno = ogrenciBilgi.ogrTCno;
                        ogrenci.okulId = ogrenciBilgi.okulId;
                        ogrenci.geoX = ogrenciBilgi.geoX;
                        ogrenci.geoY = ogrenciBilgi.geoY;
                        ogrenci.ilce_id = ogrenciBilgi.ilce_id;
                        ogrenci.ogrAdres = ogrenciBilgi.ogrAdres;
                        ogrenci.ogrenciNo = ogrenciBilgi.ogrenciNo;
                        ogrenci.ogrGsm = ogrenciBilgi.ogrGsm;
                        db.SaveChanges();
                        TempData["Mesaj"] = "İşlem Başarılı";  TempData["Btn-Renk"] = "btn-success";
                        return RedirectToAction("Ogrenciler", "Okul");
                        //return Json(0, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception e)
                    {
                        Console.Write(e);
                        TempData["Mesaj"] = "Bir hata oluştu";  TempData["Btn-Renk"] = "btn-danger";;
                        return RedirectToAction("Ogrenciler", "Okul");
                        //return Json(1, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    TempData["Mesaj"] = "Lütfen tüm alanları doldurunuz";  TempData["Btn-Renk"] = "btn-warning";
                    return RedirectToAction("Ogrenciler", "Okul");
                    //return Json(2, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                TempData["Mesaj"] = "Bu fonksiyonu çalıştırma yetkiniz bulunmamaktadır. Lütfen yeniden giriş yapınız.";
                return RedirectToAction("OkulGiris", "Okul");
                //return Json(-1, JsonRequestBehavior.AllowGet);

            }
        }

        [Attributes.OkulRoleControl]
        [HttpGet]
        public ActionResult OgrenciSil(int ogrId, int okulId)
        {
            if (okulId == (int)Session["okulId"])
            {
                try
                {
                    db.Ogrenciler.RemoveRange(db.Ogrenciler.Where(x => x.ogrId == ogrId));
                    db.SaveChanges();
                    TempData["Mesaj"] = "İşlem Başarılı";  TempData["Btn-Renk"] = "btn-success";
                    return RedirectToAction("Ogrenciler", "Okul");
                    //return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    Console.Write(e);
                    TempData["Mesaj"] = "Bir hata oluştu";  TempData["Btn-Renk"] = "btn-danger";;
                    return RedirectToAction("Ogrenciler", "Okul");
                    //return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                TempData["Mesaj"] = "Bu fonksiyonu çalıştırma yetkiniz bulunmamaktadır. Lütfen yeniden giriş yapınız.";
                return RedirectToAction("OkulGiris", "Okul");
                //return Json(-1, JsonRequestBehavior.AllowGet);

            }
        }
        #endregion

        #region // Öğrenci Velileri
        
        [Attributes.OkulRoleControl]
        [HttpPost]
        public async Task<ActionResult> OgrenciVelisiEkle(Veliler veliBilgi, int ogrId, int okulId)
        {
            if (okulId == (int)Session["okulId"])
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        try
                        {
                            if(db.Veliler.Where(x=>x.veliEmail==veliBilgi.veliEmail).Count()>0)
                            {
                                await OgrenciVelisiAta(veliBilgi, ogrId, okulId);
                                TempData["Mesaj"] = veliBilgi.veliEmail + " email adresine kayıtlı başka bir Veli bulunduğu için, giriş yapmış olduğunuz öğrenci var olan Veli'ye atanmıştır."; TempData["Btn-Renk"] = "btn-warning";
                                return RedirectToAction("Ogrenciler", "Okul");
                            }
                            veliBilgi.veliParola = (new Random().Next(100000, 10000000) + "_" + veliBilgi.veliEmail + "_" + veliBilgi.veliGsm).ToSHA(Crypto.SHA_Type.SHA1).ToSHA(Crypto.SHA_Type.SHA256);
                            var veli = db.Veliler.Add(veliBilgi);
                            db.OgrenciVelileri.Add(new OgrenciVelileri { veliId = veli.veliId, ogrId = ogrId });
                            TempData["Mesaj"] = "İşlem Başarılı";  TempData["Btn-Renk"] = "btn-success";
                        }
                        catch(Exception e)
                        {
                            Console.Write(e);
                            TempData["Mesaj"] = "Bir hata oluştu";  TempData["Btn-Renk"] = "btn-danger";;
                            return RedirectToAction("Ogrenciler", "Okul");
                        }

                        #region // mail mesajı oluştur ÖĞRENCİ ADINA VELİ OLARAK ATANMANIZ GERÇEKLEŞTİ
                        var okul = db.Okullar.Find(okulId);
                        var ogrenci = db.Ogrenciler.Find(ogrId);
                        string subject = ogrenci.ogrTCno + " / " + ogrenci.ogrAd + " " + ogrenci.ogrSoyad + " adlı öğrencinin Servis Takip Sistemi'nde takip edilme bilgisi.";
                        string mailMesaji =
                            "Merhaba " + veliBilgi.veliAd + " " + veliBilgi.veliSoyad +
                            "<br>" +
                                "Velisi bulunduğunuz, " + ogrenci.ogrAd +" "+ ogrenci.ogrSoyad +
                                " adına Veli olarak atanmanız gerçekleşmiştir." +
                            "<br>" +
                                " Servis Takip Sistemi - Veli paneli için geçici giriş bilgileriniz aşağıdaki gibidir." +
                            "<br>" +
                                "Giriş Email:" + veliBilgi.veliEmail +
                            "<br>" +
                                "Giriş Geçici Parola:" + (new Random().Next(100000, 10000000) + "_" + veliBilgi.veliEmail + "_" + veliBilgi.veliGsm).ToSHA(Crypto.SHA_Type.SHA1) +
                            "<br>" +
                                "Giriş Linki: " +
                                    "<a href='http://localhost/Veli/VeliGiris?" +
                                    "veliEmail=" + veliBilgi.veliEmail +
                                    "&veliParola=" + (new Random().Next(100000, 10000000) + "_" + veliBilgi.veliEmail + "_" + veliBilgi.veliGsm).ToSHA(Crypto.SHA_Type.SHA1) +
                                    "'>Otomatik giriş yapmak için buraya tıklayın.</a>" +
                            "<br>" +
                                "Lütfen giriş yaptıktan sonra ayarlar bölümünden geçerli şifrenizi değiştiriniz."+
                            "<br>" +
                                "Eğer bu mailin size yanlışlıkla geldiğini düşünüyorsanız lütfen " + okul.okulMail + " adresine mail yoluyla ulaşınız."
                            ;

                        EmailFormModel emailForm = new EmailFormModel
                        {
                            fromEmail = okul.okulMail,
                            fromName = okul.okulAdi,
                            message = mailMesaji,
                            subject = subject,
                            toEmail = veliBilgi.veliEmail,
                            toName = veliBilgi.veliAd + " " + veliBilgi.veliSoyad
                        };
                        #endregion


                        var EmailGonderildiMi = await Email(emailForm);
                        if(EmailGonderildiMi==true)
                        {
                            db.SaveChanges();
                            TempData["Mesaj"] = "İşlem Başarılı";  TempData["Btn-Renk"] = "btn-success";
                            return RedirectToAction("Ogrenciler", "Okul");
                            //return Json(0, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            TempData["Mesaj"] = "Mail gönderme başarısız. Veritabanı işlemi kaydedilmedi. Lütfen sistem yöneticileriyle irtibata geçiniz."; TempData["Btn-Renk"] = "btn-danger";
                            return RedirectToAction("Ogrenciler", "Okul");
                            //return Json(-1, JsonRequestBehavior.AllowGet);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.Write(e);
                        TempData["Mesaj"] = "Bir hata oluştu";  TempData["Btn-Renk"] = "btn-danger";;
                        return RedirectToAction("Ogrenciler", "Okul");
                        //return Json(1, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    TempData["Mesaj"] = "Lütfen tüm alanları doldurunuz";  TempData["Btn-Renk"] = "btn-warning";
                    return RedirectToAction("Ogrenciler", "Okul");
                    //return Json(2, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                TempData["Mesaj"] = "Bu fonksiyonu çalıştırma yetkiniz bulunmamaktadır. Lütfen yeniden giriş yapınız.";
                return RedirectToAction("OkulGiris", "Okul");
                //return Json(-1, JsonRequestBehavior.AllowGet);

            }

        }

        [Attributes.OkulRoleControl]
        [HttpPost]
        public async Task<ActionResult> OgrenciVelisiAta(Veliler veliBilgi, int ogrId, int okulId)
        {
            if (okulId == (int)Session["okulId"])
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var veli = db.Veliler.Where(x => x.veliEmail == veliBilgi.veliEmail);
                        if (veli.Count() > 0)
                        {
                            try
                            {

                                if (veli != null)
                                {
                                    var ogrenciVelisi = db.OgrenciVelileri.Where(x => x.ogrId == ogrId && x.Veliler.veliEmail == veliBilgi.veliEmail);
                                    if (ogrenciVelisi.Count() > 0)
                                    {
                                        TempData["Mesaj"] = veliBilgi.veliEmail + " email adresine sahip veli, daha önce bu öğrenciye Veli olarak atanmış."; TempData["Btn-Renk"] = "btn-warning";
                                        return RedirectToAction("Ogrenciler", "Okul");
                                    }
                                    else
                                    {
                                        db.OgrenciVelileri.Add(new OgrenciVelileri { veliId = veli.First().veliId, ogrId = ogrId });
                                        TempData["Mesaj"] = "İşlem Başarılı"; TempData["Btn-Renk"] = "btn-success";
                                    }
                                }
                                else
                                {
                                    TempData["Mesaj"] = "Bir hata oluştu"; TempData["Btn-Renk"] = "btn-danger"; ;
                                    return RedirectToAction("Ogrenciler", "Okul");
                                }
                            }
                            catch (Exception e)
                            {
                                Console.Write(e);
                                TempData["Mesaj"] = "Bir hata oluştu"; TempData["Btn-Renk"] = "btn-danger"; ;
                                return RedirectToAction("Ogrenciler", "Okul");
                            }

                            #region // mail mesajı oluştur ÖĞRENCİ ADINA VELİ OLARAK ATANMANIZ GERÇEKLEŞTİ
                            var okul = db.Okullar.Find(okulId);
                            var ogrenci = db.Ogrenciler.Find(ogrId);
                            string subject = ogrenci.ogrTCno + " / " + ogrenci.ogrAd + " " + ogrenci.ogrSoyad + " adlı öğrencinin Servis Takip Sistemi'nde takip edilme bilgisi.";
                            string mailMesaji =
                                "Sayın " + veli.First().veliAd + " " + veli.First().veliSoyad +
                                "<br>" +
                                    "Velisi bulunduğunuz okulumuzun, " + ogrenci.ogrAd + " " + ogrenci.ogrSoyad + " | " + ogrenci.ogrTCno +
                                    " bilgili öğrencimize Veli olarak atanmanız tarafımızdan gerçekleşmiştir." +
                                "<br>" +
                                    "Artık, Servis Takip Sistemi - Veli panelinden, ilgili öğrencimizin bilgilerine erişebileceksiniz." +
                                "<br>" +
                                    "Giriş Linki: " +
                                        "<a href='http://localhost/Veli/VeliGiris'>Giriş sayfasına yönlendirilmek için buraya tıklayın.</a>" +
                                "<br>" +
                                    "Eğer bu mailin size yanlışlıkla geldiğini düşünüyorsanız lütfen " + okul.okulMail + " adresine mail yoluyla ulaşınız."
                                ;

                            EmailFormModel emailForm = new EmailFormModel
                            {
                                fromEmail = okul.okulMail,
                                fromName = okul.okulAdi,
                                message = mailMesaji,
                                subject = subject,
                                toEmail = veli.First().veliEmail,
                                toName = veli.First().veliAd + " " + veli.First().veliSoyad
                            };
                            #endregion


                            var EmailGonderildiMi = await Email(emailForm);
                            if (EmailGonderildiMi == true)
                            {
                                db.SaveChanges();
                                TempData["Mesaj"] = "İşlem Başarılı"; TempData["Btn-Renk"] = "btn-success";
                                return RedirectToAction("Ogrenciler", "Okul");
                                //return Json(0, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                TempData["Mesaj"] = "Mail gönderme başarısız. Veritabanı işlemi kaydedilmedi. Lütfen sistem yöneticileriyle irtibata geçiniz."; TempData["Btn-Renk"] = "btn-danger";
                                return RedirectToAction("Ogrenciler", "Okul");
                                //return Json(-1, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            TempData["Mesaj"] = "Bu Emaile sahip bir veli bulunmamaktadır"; TempData["Btn-Renk"] = "btn-warning";
                            return RedirectToAction("Ogrenciler", "Okul");
                        }
                       
                    }
                    catch (Exception e)
                    {
                        Console.Write(e);
                        TempData["Mesaj"] = "Bir hata oluştu"; TempData["Btn-Renk"] = "btn-danger"; ;
                        return RedirectToAction("Ogrenciler", "Okul");
                        //return Json(1, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    TempData["Mesaj"] = "Lütfen tüm alanları doldurunuz"; TempData["Btn-Renk"] = "btn-warning";
                    return RedirectToAction("Ogrenciler", "Okul");
                    //return Json(2, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                TempData["Mesaj"] = "Bu fonksiyonu çalıştırma yetkiniz bulunmamaktadır. Lütfen yeniden giriş yapınız.";
                return RedirectToAction("OkulGiris", "Okul");
                //return Json(-1, JsonRequestBehavior.AllowGet);

            }

        }


        [Attributes.OkulRoleControl]
        [HttpGet]
        public async Task<ActionResult> OgrenciVelisiSil(int ogrVeliId, int okulId)
        {
            if (okulId == (int)Session["okulId"])
            {
                try
                {
                    var okul = db.Okullar.Find(okulId);
                    var veli = db.OgrenciVelileri.Find(ogrVeliId).Veliler;
                    var ogrenci = db.OgrenciVelileri.Find(ogrVeliId).Ogrenciler;
                    try
                    {
                        db.OgrenciVelileri.RemoveRange(db.OgrenciVelileri.Where(x => x.ogrVeliId == ogrVeliId));
                        TempData["Mesaj"] = "İşlem Başarılı";  TempData["Btn-Renk"] = "btn-success";
                    }
                    catch(Exception e)
                    {
                        Console.Write(e);
                        TempData["Mesaj"] = "Bir hata oluştu.";
                        return RedirectToAction("Ogrenciler", "Okul");
                        //return Json(1, JsonRequestBehavior.AllowGet);
                    }
                    #region // mail mesajı oluştur ÖĞRENCİYE AİT YETKİ VE SORUMLULUKLARINIZ FESHEDİLDİ
                    

                    string subject = "Servis Takip Sistemi'ndeki ,"+ ogrenci.ogrAd + " "+ ogrenci.ogrSoyad + " öğrenciniz hk.";
                    string mailMesaji =
                        "Sayın " + veli.veliAd + " " + veli.veliSoyad +
                        "<br>" +
                            "Servis Takip Sistemi'ndeki ," + ogrenci.ogrAd + " " + ogrenci.ogrSoyad + " üzerindeki yetki ve sorumluluklarınız " + okul.okulAdi + " tarafından feshedildi." +
                        "<br>" +
                            "Artık, " + ogrenci.ogrAd + " " + ogrenci.ogrSoyad + " üzerinde herhangi bir değişiklik yapmanız mümkün olmayacaktır." +
                        "<br>" +
                            "Eğer bu mailin size yanlışlıkla geldiğini düşünüyorsanız lütfen " + okul.okulMail + " adresine mail yoluyla ulaşınız."
                        ;

                    EmailFormModel emailForm = new EmailFormModel
                    {
                        fromEmail = okul.okulMail,
                        fromName = okul.okulAdi,
                        message = mailMesaji,
                        subject = subject,
                        toEmail = veli.veliEmail,
                        toName = veli.veliAd + " " + veli.veliSoyad
                    };
                    #endregion

                    var EmailGonderildiMi = await Email(emailForm);
                    if (EmailGonderildiMi == true)
                    {
                        db.SaveChanges();
                        TempData["Mesaj"] = "İşlem Başarılı";  TempData["Btn-Renk"] = "btn-success";
                        return RedirectToAction("Ogrenciler", "Okul");
                        //return Json(0, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        TempData["Mesaj"] = "Mail gönderme başarısız. Veritabanı işlemi kaydedilmedi. Lütfen sistem yöneticileriyle irtibata geçiniz."; TempData["Btn-Renk"] = "btn-danger";
                        return RedirectToAction("Ogrenciler", "Okul");
                        //return Json(1, JsonRequestBehavior.AllowGet);

                    }

                }
                catch (Exception e)
                {
                    Console.Write(e);
                    TempData["Mesaj"] = "Bir hata oluştu";  TempData["Btn-Renk"] = "btn-danger";;
                    return RedirectToAction("Ogrenciler", "Okul");
                    //return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                TempData["Mesaj"] = "Bu fonksiyonu çalıştırma yetkiniz bulunmamaktadır. Lütfen yeniden giriş yapınız.";
                return RedirectToAction("OkulGiris", "Okul");
                //return Json(-1, JsonRequestBehavior.AllowGet);

            }
        }
        #endregion

        #region // Veliler
        [Attributes.OkulRoleControl]
        [HttpPost]
        public async Task<ActionResult> VeliEkle(Veliler veliBilgi, int okulId)
        {
            if (okulId == (int)Session["okulId"])
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        try
                        {
                            veliBilgi.veliParola = (new Random().Next(100000, 10000000) + "_" + veliBilgi.veliEmail + "_" + veliBilgi.veliGsm).ToSHA(Crypto.SHA_Type.SHA1).ToSHA(Crypto.SHA_Type.SHA256);
                            var veli = db.Veliler.Add(veliBilgi);
                            TempData["Mesaj"] = "İşlem Başarılı";  TempData["Btn-Renk"] = "btn-success";
                        }
                        catch (Exception e)
                        {
                            Console.Write(e);
                            TempData["Mesaj"] = "Bir hata oluştu";  TempData["Btn-Renk"] = "btn-danger";;
                            return RedirectToAction("Veliler", "Okul");
                        }

                        #region // mail mesajı oluştur VELİ ATANMANIZ GERÇEKLEŞTİ HENÜZ Bİ ÖĞRENCİ ATANMADI
                        var okul = db.Okullar.Find(okulId);
                        string subject = "Servis Takip Sistemi'ne " + okul.okulAdi + " tarafından üye edildiniz.";
                        string mailMesaji =
                            "Merhaba " + veliBilgi.veliAd + " " + veliBilgi.veliSoyad +
                            "<br>" +
                                "Okulumuz " + okul.okulAdi + " okuluna Veli olarak atanmanız müdürümüz tarafından gerçekleştirilmiştir."+
                            "<br>" +
                                "Ancak henüz size atanmış bir öğrencimiz bulunmamaktadır." +
                            "<br>"+
                                "Bir öğrencimizi sizin veliliğinize atadığımızda mail ile bilgilendirileceksiniz." +
                            "<br>" +
                                "Servis Takip Sistemi - Veli paneli için geçici giriş bilgileriniz aşağıdaki gibidir." +
                            "<br>" +
                                "Giriş Email: " + veliBilgi.veliEmail +
                            "<br>" +
                                "Giriş Geçici Parola: " + (new Random().Next(100000, 10000000) + "_" + veliBilgi.veliEmail + "_" + veliBilgi.veliGsm).ToSHA(Crypto.SHA_Type.SHA1) +
                            "<br>" +
                                "Giriş Linki: " +
                                    "<a href='http://localhost/Veli/VeliGiris?" +
                                    "veliEmail=" + veliBilgi.veliEmail +
                                    "&veliParola=" + (new Random().Next(100000, 10000000) + "_" + veliBilgi.veliEmail + "_" + veliBilgi.veliGsm).ToSHA(Crypto.SHA_Type.SHA1) +
                                    "'>Otomatik giriş yapmak için buraya tıklayın.</a>" +
                            "<br>" +
                                "Lütfen giriş yaptıktan sonra ayarlar bölümünden geçerli şifrenizi değiştiriniz." +
                            "<br>" +
                                "Eğer bu mailin size yanlışlıkla geldiğini düşünüyorsanız lütfen " + okul.okulMail + " adresine mail yoluyla ulaşınız."
                            ;

                        EmailFormModel emailForm = new EmailFormModel
                        {
                            fromEmail = okul.okulMail,
                            fromName = okul.okulAdi,
                            message = mailMesaji,
                            subject = subject,
                            toEmail = veliBilgi.veliEmail,
                            toName = veliBilgi.veliAd + " " + veliBilgi.veliSoyad
                        };
                        #endregion

                        var EmailGonderildiMi = await Email(emailForm);
                        if (EmailGonderildiMi == true)
                        {
                            db.SaveChanges();
                            TempData["Mesaj"] = "İşlem Başarılı";  TempData["Btn-Renk"] = "btn-success";
                            return RedirectToAction("Veliler", "Okul");
                            //return Json(0, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            TempData["Mesaj"] = "Mail gönderme başarısız. Veritabanı işlemi kaydedilmedi. Lütfen sistem yöneticileriyle irtibata geçiniz."; TempData["Btn-Renk"] = "btn-danger";
                            return RedirectToAction("Veliler", "Okul");
                            //return Json(-1, JsonRequestBehavior.AllowGet);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.Write(e);
                        TempData["Mesaj"] = "Bir hata oluştu";  TempData["Btn-Renk"] = "btn-danger";;
                        return RedirectToAction("Veliler", "Okul");
                        //return Json(1, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    TempData["Mesaj"] = "Lütfen tüm alanları doldurunuz";  TempData["Btn-Renk"] = "btn-warning";
                    return RedirectToAction("Veliler", "Okul");
                    //return Json(2, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                TempData["Mesaj"] = "Bu fonksiyonu çalıştırma yetkiniz bulunmamaktadır. Lütfen yeniden giriş yapınız.";
                return RedirectToAction("OkulGiris", "Okul");
                //return Json(-1, JsonRequestBehavior.AllowGet);

            }

        }

        [Attributes.OkulRoleControl]
        [HttpGet]
        public async Task<ActionResult> VeliSil(int veliId, int okulId)
        {
            if (okulId == (int)Session["okulId"])
            {
                try
                {
                    var veli = db.Veliler.Find(veliId);
                    try
                    {
                        db.OgrenciVelileri.RemoveRange(db.OgrenciVelileri.Where(x => x.veliId == veliId));
                        db.Veliler.RemoveRange(db.Veliler.Where(x => x.veliId == veliId));
                        TempData["Mesaj"] = "İşlem Başarılı";  TempData["Btn-Renk"] = "btn-success";
                    }
                    catch (Exception e)
                    {
                        Console.Write(e);
                        TempData["Mesaj"] = "Bir hata oluştu:";
                        return RedirectToAction("Veliler", "Okul");
                        //return Json(1, JsonRequestBehavior.AllowGet);
                    }
                    #region // mail mesajı oluştur
                    var okul = db.Okullar.Find(okulId);
                    string subject = "Servis Takip Sistemi'ndeki 'Veli' üyeliğiniz, " + okul.okulAdi + " tarafından feshedildi.";
                    string mailMesaji =
                        "Merhaba " + veli.veliAd + " " + veli.veliSoyad +
                        "<br>" +
                            okul.okulAdi + " okulumuzla olan 'Veli' üyeliğiniz ve yetkiniz bulunan öğrencilerimiz üzerindeki yetkiniz müdürümüz tarafından feshedilmiştir." +
                        "<br>" +
                            "Artık, Servis Takip Sistemi - Veli paneline girişiniz mümkün olmayacaktır." +
                        "<br>" +
                            "Eğer bu mailin size yanlışlıkla geldiğini düşünüyorsanız lütfen " + okul.okulMail + " adresine mail yoluyla ulaşınız."
                        ;

                    EmailFormModel emailForm = new EmailFormModel
                    {
                        fromEmail = okul.okulMail,
                        fromName = okul.okulAdi,
                        message = mailMesaji,
                        subject = subject,
                        toEmail = veli.veliEmail,
                        toName = veli.veliAd + " " + veli.veliSoyad
                    };
                    #endregion

                    var EmailGonderildiMi = await Email(emailForm);
                    if (EmailGonderildiMi == true)
                    {
                        db.SaveChanges();
                        TempData["Mesaj"] = "İşlem Başarılı";  TempData["Btn-Renk"] = "btn-success";
                        return RedirectToAction("Veliler", "Okul");
                        //return Json(0, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        TempData["Mesaj"] = "Mail gönderme başarısız. Veritabanı işlemi kaydedilmedi. Lütfen sistem yöneticileriyle irtibata geçiniz."; TempData["Btn-Renk"] = "btn-danger";
                        return RedirectToAction("Veliler", "Okul");

                    }
                    
                    
                }
                catch (Exception e)
                {
                    Console.Write(e);
                    TempData["Mesaj"] = "Bir hata oluştu";  TempData["Btn-Renk"] = "btn-danger";;
                    return RedirectToAction("Veliler", "Okul");
                    //return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                TempData["Mesaj"] = "Bu fonksiyonu çalıştırma yetkiniz bulunmamaktadır. Lütfen yeniden giriş yapınız.";
                return RedirectToAction("OkulGiris", "Okul");
                //return Json(-1, JsonRequestBehavior.AllowGet);

            }
        }
        #endregion
    }
}