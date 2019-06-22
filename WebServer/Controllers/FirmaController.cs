using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using ServisTakip.Models;
using Rework;
using System.Web;

namespace ServisTakip.Controllers
{
    public class FirmaController : Controller
    {
        public ServisTakipEntities db = new ServisTakipEntities();
        

        public ActionResult Cikis(FirmaGirisModel LoginModel)
        {
            FirmaCookieControl firmaCookie = new FirmaCookieControl();
            firmaCookie.CookieSil();
            Session.RemoveAll();
            return RedirectToAction("FirmaGiris", "Firma");
        }
        public ActionResult FirmaGiris(FirmaGirisModel LoginModel)
        {
            FirmaCookieControl firCookie = new FirmaCookieControl();
            
            if (firCookie.CookieGetir() != null)
            {
                    HttpCookie cookie = firCookie.CookieGetir();
                    string firEmail = cookie["firEmail"].ToString();
                    string firParola = cookie["firParola"].ToString();
                    var firma = db.Firmalar.Where(x => x.firEmail == firEmail && x.firParola == firParola).FirstOrDefault();
                    if (firma != null)
                    {
                        firCookie.CookieSil();
                        Firmalar firmaBilgisi = new Firmalar
                        {
                            firAd = firma.firAd,
                            firEmail = firma.firEmail,
                            firParola = firma.firParola
                        };
                        firCookie.CookieKaydet(firmaBilgisi);
                        Session["firAd"] = firma.firAd;
                        Session["girenid"] = firma.firId;
                        Session["firId"] = firma.firId;
                        Session["firEmail"] = firma.firEmail;
                        Session["firmaLogin"] = true;
                        Session["GirilenYer"] = "Firma";

                        if (LoginModel.returnUrl != null)
                        {//Eğer çıkış yap butonundan buraya getirilmediyse
                            return RedirectToAction(LoginModel.returnUrl.Split('&')[0], "Firma");
                        }
                        else
                        {//Eğer çıkış yap butonundan buraya getirildiyse
                            return RedirectToAction("Anasayfa", "Firma");
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
                    string firParola = LoginModel.firParola.ToSHA(Crypto.SHA_Type.SHA256);
                    var firma = db.Firmalar.Where(x => x.firEmail == LoginModel.firEmail && x.firParola == firParola).FirstOrDefault();
                    if (firma != null)
                    {
                        firCookie.CookieSil();
                        Firmalar firmaBilgisi = new Firmalar
                        {
                            firAd = firma.firAd,
                            firEmail = firma.firEmail,
                            firParola = firma.firParola
                        };
                        firCookie.CookieKaydet(firmaBilgisi);
                        Session["firAd"] = firma.firAd;
                        Session["girenid"] = firma.firId;
                        Session["firId"] = firma.firId;
                        Session["firEmail"] = firma.firEmail;
                        Session["firmaLogin"] = true;
                        Session["GirilenYer"] = "Firma";

                        if (LoginModel.returnUrl != null)
                        {//Eğer çıkış yap butonundan buraya getirilmediyse
                            return RedirectToAction(LoginModel.returnUrl.Split('&')[0], "Firma");
                        }
                        else
                        {//Eğer çıkış yap butonundan buraya getirildiyse
                            return RedirectToAction("Anasayfa", "Firma");
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

        [Attributes.FirmaRoleControl]
        public ActionResult Anasayfa()
        {
            int firId = Convert.ToInt32(Session["firId"]);
            if (true == Convert.ToBoolean(Session["firmaLogin"]))
            {
                ViewModel vm = new ViewModel
                {
                    //Duraklar = db.Duraklar.Where(x=>x.Ogrenciler.ServistekiOgrenciler.Servisler.FirmaServisleri.Where(z => z.firId == firId).Count() > 0).ToList(),
                    //Faturalar = db.Faturalar.Where(x=>x.Servisler.FirmaServisleri.Where(y=>y.firId==firId).Count()>0).ToList(),
                    Firmalar = db.Firmalar.Where(x => x.firId == firId).ToList(),
                    FirmaSoforleri = db.FirmaSoforleri.Where(x => x.firId == firId).ToList(),
                    Ilceler = db.Ilceler.ToList(),
                    Iller = db.Iller.ToList(),
                    //IndiBindiler = db.IndiBindiler.Where(x=>x.ServistekiOgrenciler.Servisler.FirmaServisleri.Where(y=>y.firId==firId).Count()>0).ToList(),
                    Mudurler = db.Mudurler.ToList(),
                    //Odemeler = db.Odemeler.Where(x=>x.Faturalar.Servisler.FirmaServisleri.Where(y=>y.firId == firId).Count()>0).ToList(),
                    //Ogrenciler = db.Ogrenciler.Where(y => y.ServistekiOgrenciler.Servisler.FirmaServisleri.Where(z => z.firId == firId).Count() > 0).ToList(),
                    //OgrenciVelileri = db.OgrenciVelileri.Where(y => y.Ogrenciler.ServistekiOgrenciler.Servisler.FirmaServisleri.Where(z=>z.firId == firId).Count()>0).ToList(),
                    OkulCinsleri = db.OkulCinsleri.ToList(),
                    OkulTurleri = db.OkulTurleri.ToList(),
                    Okullar = db.Okullar/*.Where(x=>x.OkulServisleri.Where(y=>y.Servisler.FirmaServisleri.Where(z=>z.firId==firId).Count()>0).Count() > 0)*/.ToList(),
                    OkulServisleri = db.OkulServisleri.ToList(),
                    //Rotalar = db.Rotalar.Where(x => x.Okullar.OkulServisleri.Where(y => y.Servisler.FirmaServisleri.Where(z => z.firId == firId).Count() > 0).Count() > 0).ToList(),
                    Servisler = db.Servisler.ToList(),
                    //ServistekiOgrenciler = db.ServistekiOgrenciler.ToList(),
                    ServisSoforleri = db.ServisSoforleri.ToList(),
                    Soforler = db.Soforler.ToList(),
                    FirmaServisleri = db.FirmaServisleri.ToList()
                    //Veliler = db.Veliler.Where(x=>x.OgrenciVelileri.Where(y=>y.Ogrenciler.ServistekiOgrenciler.Servisler.FirmaServisleri.Where(z => z.firId == firId).Count() > 0).Count()>0).ToList()
                };
                return View(vm);
            }
            return null;
        }

        [Attributes.FirmaRoleControl]
        public ActionResult FirmaSoforleri()
        {
            int firId = (int)Session["firId"];
            if (true == (bool)Session["firmaLogin"])
            {
                ViewModel vm = new ViewModel
                {
                    //Duraklar = db.Duraklar.Where(x=>x.Ogrenciler.ServistekiOgrenciler.Servisler.FirmaServisleri.Where(z => z.firId == firId).Count() > 0).ToList(),
                    //Faturalar = db.Faturalar.Where(x=>x.Servisler.FirmaServisleri.Where(y=>y.firId==firId).Count()>0).ToList(),
                    Firmalar = db.Firmalar.Where(x => x.firId == firId).ToList(),
                    FirmaSoforleri = db.FirmaSoforleri.Where(x => x.firId == firId).ToList(),
                    Ilceler = db.Ilceler.ToList(),
                    Iller = db.Iller.ToList(),
                    //IndiBindiler = db.IndiBindiler.Where(x=>x.ServistekiOgrenciler.Servisler.FirmaServisleri.Where(y=>y.firId==firId).Count()>0).ToList(),
                    Mudurler = db.Mudurler.ToList(),
                    //Odemeler = db.Odemeler.Where(x=>x.Faturalar.Servisler.FirmaServisleri.Where(y=>y.firId == firId).Count()>0).ToList(),
                    //Ogrenciler = db.Ogrenciler.Where(y => y.ServistekiOgrenciler.Servisler.FirmaServisleri.Where(z => z.firId == firId).Count() > 0).ToList(),
                    //OgrenciVelileri = db.OgrenciVelileri.Where(y => y.Ogrenciler.ServistekiOgrenciler.Servisler.FirmaServisleri.Where(z=>z.firId == firId).Count()>0).ToList(),
                    OkulCinsleri = db.OkulCinsleri.ToList(),
                    OkulTurleri = db.OkulTurleri.ToList(),
                    Okullar = db.Okullar/*.Where(x=>x.OkulServisleri.Where(y=>y.Servisler.FirmaServisleri.Where(z=>z.firId==firId).Count()>0).Count() > 0)*/.ToList(),
                    OkulServisleri = db.OkulServisleri.ToList(),
                    //Rotalar = db.Rotalar.Where(x => x.Okullar.OkulServisleri.Where(y => y.Servisler.FirmaServisleri.Where(z => z.firId == firId).Count() > 0).Count() > 0).ToList(),
                    Servisler = db.Servisler.ToList(),
                    //ServistekiOgrenciler = db.ServistekiOgrenciler.ToList(),
                    ServisSoforleri = db.ServisSoforleri.ToList(),
                    Soforler = db.Soforler.ToList(),
                    FirmaServisleri = db.FirmaServisleri.ToList()
                    //Veliler = db.Veliler.Where(x=>x.OgrenciVelileri.Where(y=>y.Ogrenciler.ServistekiOgrenciler.Servisler.FirmaServisleri.Where(z => z.firId == firId).Count() > 0).Count()>0).ToList()
                };
                return View(vm);
            }
            
            return null;
        }

        [Attributes.FirmaRoleControl]
        public ActionResult Servisler()
        {
            int firId = Convert.ToInt32(Session["firId"]);
            if (true == Convert.ToBoolean(Session["firmaLogin"]))
            {
                ViewModel vm = new ViewModel
                {
                    Firmalar = db.Firmalar.Where(x => x.firId == firId).ToList(),
                    FirmaSoforleri = db.FirmaSoforleri.Where(x => x.firId == firId).ToList(),
                    Ilceler = db.Ilceler.ToList(),
                    Iller = db.Iller.ToList(),
                    OkulCinsleri = db.OkulCinsleri.ToList(),
                    OkulTurleri = db.OkulTurleri.ToList(),
                    Okullar = db.Okullar.ToList(),
                    OkulServisleri = db.OkulServisleri.ToList(),
                    Servisler = db.Servisler.ToList(),
                    ServisSoforleri = db.ServisSoforleri.ToList(),
                    Soforler = db.Soforler.ToList(),
                    FirmaServisleri = db.FirmaServisleri.Where(x=>x.firId==firId).ToList()
                };
                return View(vm);
            }

            return null;
        }

        [Attributes.FirmaRoleControl]
        public ActionResult Okullar()
        {
            int firId = (int)Session["firId"];
            if (true == (bool)Session["firmaLogin"])
            {
                ViewModel vm = new ViewModel
                {
                    //Duraklar = db.Duraklar.Where(x=>x.Ogrenciler.ServistekiOgrenciler.Servisler.FirmaServisleri.Where(z => z.firId == firId).Count() > 0).ToList(),
                    //Faturalar = db.Faturalar.Where(x=>x.Servisler.FirmaServisleri.Where(y=>y.firId==firId).Count()>0).ToList(),
                    Firmalar = db.Firmalar.Where(x=>x.firId==firId).ToList(),
                    FirmaSoforleri = db.FirmaSoforleri.Where(x=>x.firId == firId).ToList(),
                    Ilceler = db.Ilceler.ToList(),
                    Iller = db.Iller.ToList(),
                    //IndiBindiler = db.IndiBindiler.Where(x=>x.ServistekiOgrenciler.Servisler.FirmaServisleri.Where(y=>y.firId==firId).Count()>0).ToList(),
                    Mudurler = db.Mudurler.ToList(),
                    //Odemeler = db.Odemeler.Where(x=>x.Faturalar.Servisler.FirmaServisleri.Where(y=>y.firId == firId).Count()>0).ToList(),
                    //Ogrenciler = db.Ogrenciler.Where(y => y.ServistekiOgrenciler.Servisler.FirmaServisleri.Where(z => z.firId == firId).Count() > 0).ToList(),
                    //OgrenciVelileri = db.OgrenciVelileri.Where(y => y.Ogrenciler.ServistekiOgrenciler.Servisler.FirmaServisleri.Where(z=>z.firId == firId).Count()>0).ToList(),
                    OkulCinsleri = db.OkulCinsleri.ToList(),
                    OkulTurleri = db.OkulTurleri.ToList(),
                    Okullar = db.Okullar.Where(x=>x.okulId!=0)/*.Where(x=>x.OkulServisleri.Where(y=>y.Servisler.FirmaServisleri.Where(z=>z.firId==firId).Count()>0).Count() > 0)*/.ToList(),
                    OkulServisleri = db.OkulServisleri.ToList(),
                    //Rotalar = db.Rotalar.Where(x => x.Okullar.OkulServisleri.Where(y => y.Servisler.FirmaServisleri.Where(z => z.firId == firId).Count() > 0).Count() > 0).ToList(),
                    Servisler = db.Servisler.ToList(),
                    //ServistekiOgrenciler = db.ServistekiOgrenciler.ToList(),
                    ServisSoforleri = db.ServisSoforleri.ToList(),
                    Soforler = db.Soforler.ToList(),
                    FirmaServisleri = db.FirmaServisleri.ToList()
                    //Veliler = db.Veliler.Where(x=>x.OgrenciVelileri.Where(y=>y.Ogrenciler.ServistekiOgrenciler.Servisler.FirmaServisleri.Where(z => z.firId == firId).Count() > 0).Count()>0).ToList()
                };
                return View(vm);
            }

            return null;
        }

        [Attributes.FirmaRoleControl]
        public ActionResult Ayarlar()
        {
            int firId = Convert.ToInt32(Session["firId"]);
            if (true == Convert.ToBoolean(Session["firmaLogin"]))
            {
                ViewModel vm = new ViewModel
                {
                    //Duraklar = db.Duraklar.Where(x=>x.Ogrenciler.ServistekiOgrenciler.Servisler.FirmaServisleri.Where(z => z.firId == firId).Count() > 0).ToList(),
                    //Faturalar = db.Faturalar.Where(x=>x.Servisler.FirmaServisleri.Where(y=>y.firId==firId).Count()>0).ToList(),
                    Firmalar = db.Firmalar.Where(x => x.firId == firId).ToList(),
                    FirmaSoforleri = db.FirmaSoforleri.Where(x => x.firId == firId).ToList(),
                    Ilceler = db.Ilceler.ToList(),
                    Iller = db.Iller.ToList(),
                    //IndiBindiler = db.IndiBindiler.Where(x=>x.ServistekiOgrenciler.Servisler.FirmaServisleri.Where(y=>y.firId==firId).Count()>0).ToList(),
                    Mudurler = db.Mudurler.ToList(),
                    //Odemeler = db.Odemeler.Where(x=>x.Faturalar.Servisler.FirmaServisleri.Where(y=>y.firId == firId).Count()>0).ToList(),
                    //Ogrenciler = db.Ogrenciler.Where(y => y.ServistekiOgrenciler.Servisler.FirmaServisleri.Where(z => z.firId == firId).Count() > 0).ToList(),
                    //OgrenciVelileri = db.OgrenciVelileri.Where(y => y.Ogrenciler.ServistekiOgrenciler.Servisler.FirmaServisleri.Where(z=>z.firId == firId).Count()>0).ToList(),
                    OkulCinsleri = db.OkulCinsleri.ToList(),
                    OkulTurleri = db.OkulTurleri.ToList(),
                    Okullar = db.Okullar/*.Where(x=>x.OkulServisleri.Where(y=>y.Servisler.FirmaServisleri.Where(z=>z.firId==firId).Count()>0).Count() > 0)*/.ToList(),
                    OkulServisleri = db.OkulServisleri.ToList(),
                    //Rotalar = db.Rotalar.Where(x => x.Okullar.OkulServisleri.Where(y => y.Servisler.FirmaServisleri.Where(z => z.firId == firId).Count() > 0).Count() > 0).ToList(),
                    Servisler = db.Servisler.ToList(),
                    //ServistekiOgrenciler = db.ServistekiOgrenciler.ToList(),
                    ServisSoforleri = db.ServisSoforleri.ToList(),
                    Soforler = db.Soforler.ToList(),
                    FirmaServisleri = db.FirmaServisleri.ToList()
                    //Veliler = db.Veliler.Where(x=>x.OgrenciVelileri.Where(y=>y.Ogrenciler.ServistekiOgrenciler.Servisler.FirmaServisleri.Where(z => z.firId == firId).Count() > 0).Count()>0).ToList()
                };
                return View(vm);
            }

            return null;
        }

        [HttpPost]
        [Attributes.FirmaRoleControl]
        public ActionResult FirmaGuncelle(Firmalar firmaBilgisi)
        {
            if (firmaBilgisi.firId == (int)Session["firId"])
            {
                try
                {
                    var firma = db.Firmalar.Where(x => x.firId == firmaBilgisi.firId).First();
                    firma.firAd = firmaBilgisi.firAd;
                    firma.firAdres = firmaBilgisi.firAdres;
                    firma.firFax = firmaBilgisi.firFax;
                    firma.firTel = firmaBilgisi.firTel;
                    firma.firYetkiliAdSoyad = firma.firYetkiliAdSoyad;
                    firma.ilce_id = firmaBilgisi.ilce_id;
                    if (firma.firParola.Length > 0)
                    {
                        firma.firParola = firmaBilgisi.firParola.ToSHA(Crypto.SHA_Type.SHA256);
                    }
                    db.SaveChanges();
                    TempData["Mesaj"] = "İşlem Başarılı.";
                    return RedirectToAction("Ayarlar", "Firma");
                    //return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    Console.Write(e);
                    TempData["Mesaj"] = "Bir hata oluştu. Lütfen tekrar deneyiniz.";
                    return RedirectToAction("Ayarlar", "Firma");
                    //return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                TempData["Mesaj"] = "Bu fonksiyonu çalıştırma yetkiniz bulunmamaktadır.";
                return RedirectToAction("FirmaGiris", "Firma");
                //return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }

        #region // Firma Soforleri
        [HttpPost]
        [Attributes.FirmaRoleControl]
        public ActionResult FirmaSoforEkle(FirmaSoforleri firmaSoforleri)
        {
            if (firmaSoforleri.firId == (int)Session["firId"])
            {
                try
                {
                    db.FirmaSoforleri.Add(firmaSoforleri);
                    db.SaveChanges();
                    TempData["Mesaj"] = "İşlem Başarılı.";
                    return RedirectToAction("FirmaSoforleri", "Firma");
                    //return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    Console.Write(e);
                    TempData["Mesaj"] = "Bir hata oluştu. Lütfen tekrar deneyiniz.";
                    return RedirectToAction("FirmaSoforleri", "Firma");
                    //return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                TempData["Mesaj"] = "Bu fonksiyonu çalıştırma yetkiniz bulunmamaktadır.";
                return RedirectToAction("FirmaGiris", "Firma");
                //return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Attributes.FirmaRoleControl]
        public ActionResult FirmaSoforSil(int firSoforId, int firId)
        {
            if(firId == (int)Session["firId"])
            {
                try
                {
                    db.FirmaSoforleri.RemoveRange(db.FirmaSoforleri.Where(d => d.firSoforId == firSoforId));
                    db.SaveChanges();
                    TempData["Mesaj"] = "İşlem Başarılı.";
                    return RedirectToAction("FirmaSoforleri", "Firma");
                    //return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    Console.Write(e);
                    TempData["Mesaj"] = "Bir hata oluştu. Lütfen tekrar deneyiniz.";
                    return RedirectToAction("FirmaSoforleri", "Firma");
                    //return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                TempData["Mesaj"] = "Bu fonksiyonu çalıştırma yetkiniz bulunmamaktadır. Lütfen yeniden giriş yapınız.";
                return RedirectToAction("FirmaGiris", "Firma");
                //return Json(-1, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region // Servisler
        [HttpPost]
        [Attributes.FirmaRoleControl]
        public ActionResult ServisEkle(Servisler servisBilgisi, int firId, int sofId, int? yedekId)
        {
            if (firId == (int)Session["firId"])
            {
                //if (ModelState.IsValid)
                //{
                    try
                    {
                        if(db.Servisler.Where(x=>x.plaka == servisBilgisi.plaka).Count() > 0)
                        {
                            TempData["Mesaj"] = "Bu plaka daha önce kullanılmış.";
                            return RedirectToAction("Servisler", "Firma");
                        }
                        if (db.Servisler.Where(x => x.sicilNo == servisBilgisi.sicilNo).Count() > 0)
                        {
                            TempData["Mesaj"] = "Bu sicil no daha önce kullanılmış.";
                            return RedirectToAction("Servisler", "Firma");
                        }
                        Random rand = new Random();
                        servisBilgisi.authCode = (servisBilgisi.servisId + "_" + servisBilgisi.plaka + "_" + rand.Next(100000, 999999)).ToSHA(Crypto.SHA_Type.SHA256);
                        var yeniServis = db.Servisler.Add(servisBilgisi);
                        db.ServisSoforleri.RemoveRange(db.ServisSoforleri.Where(x => x.servisId == yeniServis.servisId));
                    db.FirmaServisleri.Add(new FirmaServisleri { firId = firId, servisId = yeniServis.servisId });
                        db.ServisSoforleri.Add(new ServisSoforleri { servisId = yeniServis.servisId, sofId = sofId, yedekMi = false });
                        if (yedekId.HasValue)
                            db.ServisSoforleri.Add(new ServisSoforleri { servisId = yeniServis.servisId, sofId = yedekId.Value, yedekMi = true });

                        db.SaveChanges();
                        TempData["Mesaj"] = "İşlem Başarılı.";
                        return RedirectToAction("Servisler", "Firma");
                        //return Json(0, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception e)
                    {
                        Console.Write(e);
                        TempData["Mesaj"] = "Bir hata oluştu. Lütfen tekrar deneyiniz.";
                        return RedirectToAction("Servisler", "Firma");
                        //return Json(1, JsonRequestBehavior.AllowGet);
                    }

                /*}
                else
                {
                    TempData["Mesaj"] = "Lütfen gerekli tüm alanları doldurunuz.";
                    return RedirectToAction("Servisler", "Firma");
                }*/
            }
            else
            {
                TempData["Mesaj"] = "Bu fonksiyonu çalıştırma yetkiniz bulunmamaktadır.";
                return RedirectToAction("FirmaGiris", "Firma");
                //return Json(-1, JsonRequestBehavior.AllowGet);
            }
            
        }

        [HttpPost]
        [Attributes.FirmaRoleControl]
        public ActionResult ServisGuncelle(Servisler servisBilgisi, int firId)
        {
            if (firId == (int)Session["firId"])
            {
                try
                {
                    Random rand = new Random();
                    var servis = db.Servisler.Where(x => x.servisId == servisBilgisi.servisId).First();
                    servis.plaka = servisBilgisi.plaka;
                    servis.sonMuayeneTarih = servisBilgisi.sonMuayeneTarih;
                    servis.iseGirisTarih = servis.iseGirisTarih;
                    servis.sicilNo = servisBilgisi.sicilNo;
                    servis.aktifMi = servisBilgisi.aktifMi;
                    servis.authCode = (servisBilgisi.servisId + "_" + servisBilgisi.plaka + "_" + rand.Next(100000,999999)).ToSHA(Crypto.SHA_Type.SHA256);
                    db.SaveChanges();
                    TempData["Mesaj"] = "İşlem Başarılı.";
                    return RedirectToAction("Servisler", "Firma");
                    //return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    TempData["Mesaj"] = "Bir hata oluştu. Lütfen tekrar deneyiniz.";
                    return RedirectToAction("Servisler", "Firma");
                    //return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                TempData["Mesaj"] = "Bu fonksiyonu çalıştırma yetkiniz bulunmamaktadır.";
                return RedirectToAction("FirmaGiris", "Firma");
                //return Json(-1, JsonRequestBehavior.AllowGet);
            }
            
        }

        [HttpPost]
        [Attributes.FirmaRoleControl]
        public ActionResult ServisSoforleriGuncelle(int sofId, int? yedekId, int firId, int servisId)
        {
            if (firId == (int)Session["firId"])
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        
                        db.ServisSoforleri.RemoveRange(db.ServisSoforleri.Where(x => x.servisId == servisId));
                        db.ServisSoforleri.Add(new ServisSoforleri { servisId = servisId, sofId = sofId, yedekMi = false });
                        if (yedekId.HasValue)
                        {
                            if (sofId == yedekId.Value)
                            {
                                TempData["Mesaj"] = "Asil ve Yedek Şoför Aynı Kişi Olamaz. Lütfen liste içerisinden YEDEK ŞOFÖR YOK seçeneğini seçiniz.";
                                return RedirectToAction("Servisler", "Firma");
                            }
                            db.ServisSoforleri.Add(new ServisSoforleri { servisId = servisId, sofId = yedekId.Value, yedekMi = true });
                        }

                        db.SaveChanges();
                        TempData["Mesaj"] = "İşlem Başarılı.";
                        return RedirectToAction("Servisler", "Firma");
                        //return Json(0, JsonRequestBehavior.AllowGet);
                    }
                    catch(Exception e)
                    {
                        Console.Write(e);
                        TempData["Mesaj"] = "Bir hata oluştu. Lütfen tekrar deneyiniz.";
                        return RedirectToAction("Servisler", "Firma");
                        //return Json(1, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    TempData["Mesaj"] = "Lütfen gerekli tüm alanları doldurunuz.";
                    return RedirectToAction("Servisler", "Firma");
                }
            }
            else
            {
                TempData["Mesaj"] = "Bu fonksiyonu çalıştırma yetkiniz bulunmamaktadır.";
                return RedirectToAction("FirmaGiris", "Firma");
                //return Json(-1, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        [Attributes.FirmaRoleControl]
        public ActionResult ServisSil(int servisId, int firId)
        {
            if (firId == (int)Session["firId"])
            {
                try
                {
                    db.FirmaServisleri.RemoveRange(db.FirmaServisleri.Where(d => d.servisId == servisId));
                    db.ServistekiOgrenciler.RemoveRange(db.ServistekiOgrenciler.Where(d => d.OkulServisleri.servisId == servisId));
                    db.ServisSoforleri.RemoveRange(db.ServisSoforleri.Where(d => d.servisId == servisId));
                    db.OkulServisleri.RemoveRange(db.OkulServisleri.Where(d => d.servisId == servisId));
                    db.Servisler.RemoveRange(db.Servisler.Where(d => d.servisId == servisId));
                    db.SaveChanges();
                    TempData["Mesaj"] = "İşlem Başarılı.";
                    return RedirectToAction("Servisler", "Firma");
                    //return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    Console.Write(e);
                    TempData["Mesaj"] = "Bir hata oluştu. Lütfen tekrar deneyiniz.";
                    return RedirectToAction("Servisler", "Firma");
                    //return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                TempData["Mesaj"] = "Bu fonksiyonu çalıştırma yetkiniz bulunmamaktadır.";
                return RedirectToAction("FirmaGiris", "Firma");
                //return Json(-1, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region // Okul Servisleri
        [HttpPost]
        [Attributes.FirmaRoleControl]
        public ActionResult OkulServisEkle(OkulServisleri okulServisleri, int firId)
        {
            if (firId == (int)Session["firId"])
            {
                try
                {
                    db.OkulServisleri.Add(okulServisleri);
                    db.SaveChanges();
                    TempData["Mesaj"] = "İşlem Başarılı.";
                    return RedirectToAction("Okullar", "Firma");
                    //return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    Console.Write(e);
                    TempData["Mesaj"] = "Bir hata oluştu. Lütfen tekrar deneyiniz.";
                    return RedirectToAction("Okullar", "Firma");
                    //return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                TempData["Mesaj"] = "Bu fonksiyonu çalıştırma yetkiniz bulunmamaktadır.";
                return RedirectToAction("FirmaGiris", "Firma");
                //return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        [Attributes.FirmaRoleControl]
        public ActionResult OkulServisSil(int okulServisId, int firId)
        {
            if (firId == (int)Session["firId"])
            {
                try
                {
                    db.OkulServisleri.RemoveRange(db.OkulServisleri.Where(d => d.okulServisId == okulServisId));
                    db.SaveChanges();
                    TempData["Mesaj"] = "İşlem Başarılı.";
                    return RedirectToAction("Okullar", "Firma");
                    //return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    Console.Write(e);
                    TempData["Mesaj"] = "Bir hata oluştu. Lütfen tekrar deneyiniz.";
                    return RedirectToAction("Okullar", "Firma");
                    //return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                TempData["Mesaj"] = "Bu fonksiyonu çalıştırma yetkiniz bulunmamaktadır. Lütfen yeniden giriş yapınız.";
                return RedirectToAction("FirmaGiris", "Firma");
                //return Json(-1, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        [Attributes.FirmaRoleControl]
        public ActionResult OkulSil(int okulId, int firId)
        {
            if (firId == (int)Session["firId"])
            {
                try
                {
                    foreach(var firmaServisi in db.FirmaServisleri.Where(x => x.firId == firId))
                    {
                        foreach(var servis in db.Servisler.Where(x=>x.servisId == firmaServisi.servisId))
                        {
                            db.OkulServisleri.RemoveRange(db.OkulServisleri.Where(d => d.okulId == okulId && d.servisId==servis.servisId));
                        }
                    }

                    
                    db.SaveChanges();
                    TempData["Mesaj"] = "İşlem Başarılı.";
                    return RedirectToAction("Okullar", "Firma");
                    //return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    Console.Write(e);
                    TempData["Mesaj"] = "Bir hata oluştu. Lütfen tekrar deneyiniz.";
                    return RedirectToAction("Okullar", "Firma");
                    //return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                TempData["Mesaj"] = "Bu fonksiyonu çalıştırma yetkiniz bulunmamaktadır. Lütfen yeniden giriş yapınız.";
                return RedirectToAction("FirmaGiris", "Firma");
                //return Json(-1, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

    }

}
