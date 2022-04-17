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
        public ServisTakipEntities db = new();
        

        public ActionResult Cikis(FirmaGirisModel LoginModel)
        {
            FirmaCookieControl firmaCookie = new();
            firmaCookie.CookieSil();
            Session.RemoveAll();
            return RedirectToAction("FirmaGiris", "Firma");
        }
        public ActionResult FirmaGiris(FirmaGirisModel LoginModel)
        {
            FirmaCookieControl firCookie = new();
            
            if (firCookie.CookieGetir() != null)
            {
                    HttpCookie cookie = firCookie.CookieGetir();
                    string firEmail = cookie["firEmail"].ToString();
                    string firParola = cookie["firParola"].ToString();
                    var firma = db.Firmalar.Where(x => x.firEmail == firEmail && x.firParola == firParola).FirstOrDefault();
                    if (firma != null)
                    {
                        firCookie.CookieSil();
                        Firmalar firmaBilgisi = new()
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
                        Firmalar firmaBilgisi = new()
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
                ViewModel vm = new()
                {
                    Firmalar = db.Firmalar.Where(x => x.firId == firId).ToList(),
                    FirmaSoforleri = db.FirmaSoforleri.Where(x => x.firId == firId).ToList(),
                    Ilceler = db.Ilceler.ToList(),
                    Iller = db.Iller.ToList(),
                    Mudurler = db.Mudurler.ToList(),
                    OkulCinsleri = db.OkulCinsleri.ToList(),
                    OkulTurleri = db.OkulTurleri.ToList(),
                    Okullar = db.Okullar/*.Where(x=>x.OkulServisleri.Where(y=>y.Araclar.FirmaAraclari.Where(z=>z.firId==firId).Count()>0).Count() > 0)*/.ToList(),
                    OkulServisleri = db.OkulServisleri.ToList(),
                    Araclar = db.Araclar.ToList(),
                    AracSoforleri = db.AracSoforleri.ToList(),
                    Soforler = db.Soforler.ToList(),
                    FirmaAraclari = db.FirmaAraclari.ToList()
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
                ViewModel vm = new()
                {
                    Firmalar = db.Firmalar.Where(x => x.firId == firId).ToList(),
                    FirmaSoforleri = db.FirmaSoforleri.Where(x => x.firId == firId).ToList(),
                    Ilceler = db.Ilceler.ToList(),
                    Iller = db.Iller.ToList(),
                    Mudurler = db.Mudurler.ToList(),
                    OkulCinsleri = db.OkulCinsleri.ToList(),
                    OkulTurleri = db.OkulTurleri.ToList(),
                    Okullar = db.Okullar/*.Where(x=>x.OkulServisleri.Where(y=>y.Araclar.FirmaAraclari.Where(z=>z.firId==firId).Count()>0).Count() > 0)*/.ToList(),
                    OkulServisleri = db.OkulServisleri.ToList(),
                    Araclar = db.Araclar.ToList(),
                    AracSoforleri = db.AracSoforleri.ToList(),
                    Soforler = db.Soforler.ToList(),
                    FirmaAraclari = db.FirmaAraclari.ToList()
                };
                return View(vm);
            }
            
            return null;
        }

        [Attributes.FirmaRoleControl]
        public ActionResult Araclar()
        {
            int firId = Convert.ToInt32(Session["firId"]);
            if (true == Convert.ToBoolean(Session["firmaLogin"]))
            {
                ViewModel vm = new()
                {
                    Firmalar = db.Firmalar.Where(x => x.firId == firId).ToList(),
                    FirmaSoforleri = db.FirmaSoforleri.Where(x => x.firId == firId).ToList(),
                    Ilceler = db.Ilceler.ToList(),
                    Iller = db.Iller.ToList(),
                    OkulCinsleri = db.OkulCinsleri.ToList(),
                    OkulTurleri = db.OkulTurleri.ToList(),
                    Okullar = db.Okullar.ToList(),
                    OkulServisleri = db.OkulServisleri.ToList(),
                    Araclar = db.Araclar.ToList(),
                    AracSoforleri = db.AracSoforleri.ToList(),
                    Soforler = db.Soforler.ToList(),
                    FirmaAraclari = db.FirmaAraclari.Where(x=>x.firId==firId).ToList()
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
                ViewModel vm = new()
                {
                    Firmalar = db.Firmalar.Where(x=>x.firId==firId).ToList(),
                    FirmaSoforleri = db.FirmaSoforleri.Where(x=>x.firId == firId).ToList(),
                    Ilceler = db.Ilceler.ToList(),
                    Iller = db.Iller.ToList(),
                    Mudurler = db.Mudurler.ToList(),
                    OkulCinsleri = db.OkulCinsleri.ToList(),
                    OkulTurleri = db.OkulTurleri.ToList(),
                    Okullar = db.Okullar.Where(x=>x.okulId!=0)/*.Where(x=>x.OkulServisleri.Where(y=>y.Araclar.FirmaAraclari.Where(z=>z.firId==firId).Count()>0).Count() > 0)*/.ToList(),
                    OkulServisleri = db.OkulServisleri.ToList(),
                    Araclar = db.Araclar.ToList(),
                    AracSoforleri = db.AracSoforleri.ToList(),
                    Soforler = db.Soforler.ToList(),
                    FirmaAraclari = db.FirmaAraclari.ToList()
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
                ViewModel vm = new()
                {
                    Firmalar = db.Firmalar.Where(x => x.firId == firId).ToList(),
                    FirmaSoforleri = db.FirmaSoforleri.Where(x => x.firId == firId).ToList(),
                    Ilceler = db.Ilceler.ToList(),
                    Iller = db.Iller.ToList(),
                    Mudurler = db.Mudurler.ToList(),
                    OkulCinsleri = db.OkulCinsleri.ToList(),
                    OkulTurleri = db.OkulTurleri.ToList(),
                    Okullar = db.Okullar/*.Where(x=>x.OkulServisleri.Where(y=>y.Araclar.FirmaAraclari.Where(z=>z.firId==firId).Count()>0).Count() > 0)*/.ToList(),
                    OkulServisleri = db.OkulServisleri.ToList(),
                    Araclar = db.Araclar.ToList(),
                    AracSoforleri = db.AracSoforleri.ToList(),
                    Soforler = db.Soforler.ToList(),
                    FirmaAraclari = db.FirmaAraclari.ToList()
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

        #region // Araclar
        [HttpPost]
        [Attributes.FirmaRoleControl]
        public ActionResult ServisEkle(Araclar servisBilgisi, int firId, int sofId, int? yedekId)
        {
            if (firId == (int)Session["firId"])
            {
                //if (ModelState.IsValid)
                //{
                    try
                    {
                        if(db.Araclar.Where(x=>x.plaka == servisBilgisi.plaka).Count() > 0)
                        {
                            TempData["Mesaj"] = "Bu plaka daha önce kullanılmış.";
                            return RedirectToAction("Araclar", "Firma");
                        }
                        if (db.Araclar.Where(x => x.sicilNo == servisBilgisi.sicilNo).Count() > 0)
                        {
                            TempData["Mesaj"] = "Bu sicil no daha önce kullanılmış.";
                            return RedirectToAction("Araclar", "Firma");
                        }
                        Random rand = new();
                        servisBilgisi.authCode = (servisBilgisi.aracId + "_" + servisBilgisi.plaka + "_" + rand.Next(100000, 999999)).ToSHA(Crypto.SHA_Type.SHA256);
                        var yeniServis = db.Araclar.Add(servisBilgisi);
                        db.AracSoforleri.RemoveRange(db.AracSoforleri.Where(x => x.aracId == yeniServis.aracId));
                    db.FirmaAraclari.Add(new FirmaAraclari { firId = firId, aracId = yeniServis.aracId });
                        db.AracSoforleri.Add(new AracSoforleri { aracId = yeniServis.aracId, sofId = sofId, yedekMi = false });
                        if (yedekId.HasValue)
                            db.AracSoforleri.Add(new AracSoforleri { aracId = yeniServis.aracId, sofId = yedekId.Value, yedekMi = true });

                        db.SaveChanges();
                        TempData["Mesaj"] = "İşlem Başarılı.";
                        return RedirectToAction("Araclar", "Firma");
                        //return Json(0, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception e)
                    {
                        Console.Write(e);
                        TempData["Mesaj"] = "Bir hata oluştu. Lütfen tekrar deneyiniz.";
                        return RedirectToAction("Araclar", "Firma");
                        //return Json(1, JsonRequestBehavior.AllowGet);
                    }

                /*}
                else
                {
                    TempData["Mesaj"] = "Lütfen gerekli tüm alanları doldurunuz.";
                    return RedirectToAction("Araclar", "Firma");
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
        public ActionResult ServisGuncelle(Araclar servisBilgisi, int firId)
        {
            if (firId == (int)Session["firId"])
            {
                try
                {
                    Random rand = new();
                    var servis = db.Araclar.Where(x => x.aracId == servisBilgisi.aracId).First();
                    servis.plaka = servisBilgisi.plaka;
                    servis.sonMuayeneTarih = servisBilgisi.sonMuayeneTarih;
                    servis.iseGirisTarih = servis.iseGirisTarih;
                    servis.sicilNo = servisBilgisi.sicilNo;
                    servis.aktifMi = servisBilgisi.aktifMi;
                    servis.authCode = (servisBilgisi.aracId + "_" + servisBilgisi.plaka + "_" + rand.Next(100000,999999)).ToSHA(Crypto.SHA_Type.SHA256);
                    db.SaveChanges();
                    TempData["Mesaj"] = "İşlem Başarılı.";
                    return RedirectToAction("Araclar", "Firma");
                    //return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    TempData["Mesaj"] = "Bir hata oluştu. Lütfen tekrar deneyiniz.";
                    return RedirectToAction("Araclar", "Firma");
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
        public ActionResult ServisSoforleriGuncelle(int sofId, int? yedekId, int firId, int aracId)
        {
            if (firId == (int)Session["firId"])
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        
                        db.AracSoforleri.RemoveRange(db.AracSoforleri.Where(x => x.aracId == aracId));
                        db.AracSoforleri.Add(new AracSoforleri { aracId = aracId, sofId = sofId, yedekMi = false });
                        if (yedekId.HasValue)
                        {
                            if (sofId == yedekId.Value)
                            {
                                TempData["Mesaj"] = "Asil ve Yedek Şoför Aynı Kişi Olamaz. Lütfen liste içerisinden YEDEK ŞOFÖR YOK seçeneğini seçiniz.";
                                return RedirectToAction("Araclar", "Firma");
                            }
                            db.AracSoforleri.Add(new AracSoforleri { aracId = aracId, sofId = yedekId.Value, yedekMi = true });
                        }

                        db.SaveChanges();
                        TempData["Mesaj"] = "İşlem Başarılı.";
                        return RedirectToAction("Araclar", "Firma");
                        //return Json(0, JsonRequestBehavior.AllowGet);
                    }
                    catch(Exception e)
                    {
                        Console.Write(e);
                        TempData["Mesaj"] = "Bir hata oluştu. Lütfen tekrar deneyiniz.";
                        return RedirectToAction("Araclar", "Firma");
                        //return Json(1, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    TempData["Mesaj"] = "Lütfen gerekli tüm alanları doldurunuz.";
                    return RedirectToAction("Araclar", "Firma");
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
        public ActionResult ServisSil(int aracId, int firId)
        {
            if (firId == (int)Session["firId"])
            {
                try
                {
                    db.FirmaAraclari.RemoveRange(db.FirmaAraclari.Where(d => d.aracId == aracId));
                    db.ServistekiOgrenciler.RemoveRange(db.ServistekiOgrenciler.Where(d => d.OkulServisleri.aracId == aracId));
                    db.AracSoforleri.RemoveRange(db.AracSoforleri.Where(d => d.aracId == aracId));
                    db.OkulServisleri.RemoveRange(db.OkulServisleri.Where(d => d.aracId == aracId));
                    db.Araclar.RemoveRange(db.Araclar.Where(d => d.aracId == aracId));
                    db.SaveChanges();
                    TempData["Mesaj"] = "İşlem Başarılı.";
                    return RedirectToAction("Araclar", "Firma");
                    //return Json(0, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    Console.Write(e);
                    TempData["Mesaj"] = "Bir hata oluştu. Lütfen tekrar deneyiniz.";
                    return RedirectToAction("Araclar", "Firma");
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
        public ActionResult OkulServisSil(int okulAracId, int firId)
        {
            if (firId == (int)Session["firId"])
            {
                try
                {
                    db.OkulServisleri.RemoveRange(db.OkulServisleri.Where(d => d.okulAracId == okulAracId));
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
                    foreach(var firmaServisi in db.FirmaAraclari.Where(x => x.firId == firId))
                    {
                        foreach(var servis in db.Araclar.Where(x=>x.aracId == firmaServisi.aracId))
                        {
                            db.OkulServisleri.RemoveRange(db.OkulServisleri.Where(d => d.okulId == okulId && d.aracId==servis.aracId));
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
