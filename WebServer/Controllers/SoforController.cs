using Rework;
using ServisTakip.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServisTakip.Controllers
{
    public class SoforController : Controller
    {
        public ServisTakipEntities db = new ServisTakipEntities();

        public ActionResult Cikis(SoforGirisModel LoginModel)
        {
            SoforCookieControl soforCookie = new SoforCookieControl();
            soforCookie.CookieSil();
            Session.RemoveAll();
            return RedirectToAction("SoforGiris", "Sofor");
        }
        public ActionResult SoforGiris(SoforGirisModel LoginModel)
        {
            SoforCookieControl soforCookie = new SoforCookieControl();
            Session.RemoveAll();

            if (soforCookie.CookieGetir() != null)
            {
                HttpCookie cookie = soforCookie.CookieGetir();
                string sofEmail = cookie["sofEmail"].ToString();
                string sofParola = cookie["sofParola"].ToString();
                string plaka = cookie["plaka"].ToString();
                var sofor = db.Soforler.Where(x => x.sofEmail == sofEmail && x.sofParola == sofParola).FirstOrDefault();
                if (sofor != null)
                {
                    var servisSoforu = db.AracSoforleri.Where(x => x.sofId == sofor.sofId && x.Araclar.plaka == plaka);
                    if (servisSoforu.Count()>0)
                    {
                        soforCookie.CookieSil();
                        Soforler soforBilgisi = new Soforler
                        {
                            sofAd = sofor.sofAd,
                            sofSoyad = sofor.sofSoyad,
                            sofEmail = sofor.sofEmail,
                            sofParola = sofor.sofParola
                        };
                        soforCookie.CookieKaydet(soforBilgisi, plaka);

                        Session["sofAd"] = sofor.sofAd;
                        Session["sofSoyad"] = sofor.sofSoyad;
                        Session["aracId"] = servisSoforu.First().aracId;
                        Session["girenid"] = sofor.sofId;
                        Session["sofId"] = sofor.sofId;
                        Session["sofEmail"] = sofor.sofEmail;
                        Session["soforLogin"] = true;
                        Session["GirilenYer"] = "Sofor";

                        if (LoginModel.returnUrl != null)
                        {//Eğer çıkış yap butonundan buraya getirilmediyse
                            return RedirectToAction(LoginModel.returnUrl.Split('&')[0], "Sofor");
                        }
                        else
                        {//Eğer çıkış yap butonundan buraya getirildiyse
                            return RedirectToAction("Okullar", "Sofor");
                        }
                    }
                    else
                    {
                        TempData["Mesaj"] = "Bu plakaya giriş yetkiniz bulunmamaktadır.";
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
                    string sofParola = LoginModel.sofParola.ToSHA(Crypto.SHA_Type.SHA256);
                    var sofor = db.Soforler.Where(x => x.sofEmail == LoginModel.sofEmail && x.sofParola == sofParola).FirstOrDefault();
                    if (sofor != null)
                    {
                        var servisSoforu = db.AracSoforleri.Where(x => x.sofId == sofor.sofId && x.Araclar.plaka == LoginModel.plaka);
                        if (servisSoforu.Count() > 0)
                        {
                            soforCookie.CookieSil();
                            Soforler soforBilgisi = new Soforler
                            {
                                sofAd = sofor.sofAd,
                                sofSoyad = sofor.sofSoyad,
                                sofEmail = sofor.sofEmail,
                                sofParola = sofor.sofParola
                            };
                            soforCookie.CookieKaydet(soforBilgisi, LoginModel.plaka);
                            Session["sofAd"] = sofor.sofAd;
                            Session["sofSoyad"] = sofor.sofSoyad;
                            Session["girenid"] = sofor.sofId;
                            Session["aracId"] = servisSoforu.First().aracId;
                            Session["sofId"] = sofor.sofId;
                            Session["sofEmail"] = sofor.sofEmail;
                            Session["soforLogin"] = true;
                            Session["GirilenYer"] = "Sofor";

                            if (LoginModel.returnUrl != null)
                            {//Eğer çıkış yap butonundan buraya getirilmediyse
                                return RedirectToAction(LoginModel.returnUrl.Split('&')[0], "Sofor");
                            }
                            else
                            {//Eğer çıkış yap butonundan buraya getirildiyse
                                return RedirectToAction("Okullar", "Sofor");
                            }
                        }
                        else
                        {
                            TempData["Mesaj"] = "Bu plakaya giriş yetkiniz bulunmamaktadır.";
                        }
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

        [Attributes.SoforRoleControl]
        public ActionResult Servis(int? okulAracId, short? okuldanEveEvdenOkula)
        {
            if (okulAracId.HasValue && okuldanEveEvdenOkula.HasValue)
            {
                int sofId = (int)Session["sofId"];
                var okulServisi = db.OkulServisleri.Find(okulAracId.Value);
                int aracId = okulServisi.aracId;
                int okulId = okulServisi.okulId;
                ViewModel vm = new ViewModel
                {
                    Duraklar = db.Duraklar.Where(x => x.Rotalar.OkulServisleri.aracId == aracId).ToList(),
                    Faturalar = db.Faturalar.Where(x => x.okulAracId == okulAracId).ToList(),
                    Firmalar = db.Firmalar.ToList(),
                    FirmaSoforleri = db.FirmaSoforleri.Where(x => x.sofId == sofId).ToList(),
                    Ilceler = db.Ilceler.ToList(),
                    Iller = db.Iller.ToList(),
                    IndiBindiler = db.IndiBindiler.ToList(),
                    Mudurler = db.Mudurler.ToList(),
                    Odemeler = db.Odemeler.Where(x=>x.Faturalar.ServistekiOgrenciler.okulAracId==okulAracId).ToList(),
                    Ogrenciler = db.Ogrenciler.ToList(),
                    OgrenciVelileri = db.OgrenciVelileri.ToList(),
                    OkulCinsleri = db.OkulCinsleri.ToList(),
                    OkulTurleri = db.OkulTurleri.ToList(),
                    Okullar = db.Okullar.Where(x => x.okulId == okulId).ToList(),
                    OkulServisleri = db.OkulServisleri.Where(x => x.aracId == aracId).ToList(),
                    Rotalar = db.Rotalar.Where(x => x.okulAracId == okulAracId.Value).ToList(),
                    Araclar = db.Araclar.Where(x => x.aracId == aracId).ToList(),
                    ServistekiOgrenciler = db.ServistekiOgrenciler.Where(x => x.okulAracId == okulAracId.Value).ToList(),
                    AracSoforleri = db.AracSoforleri.Where(x => x.aracId == aracId).ToList(),
                    Soforler = db.Soforler.Where(x => x.sofId == sofId).ToList(),
                    FirmaAraclari = db.FirmaAraclari.Where(x => x.aracId == aracId).ToList(),
                    Veliler = db.Veliler.ToList()
                };
                ViewBag.authCode = okulServisi.Araclar.authCode;
                ViewBag.okuldanEveEvdenOkula = okuldanEveEvdenOkula;
                ViewBag.sofId = sofId;
                ViewBag.aracId = aracId;
                ViewBag.okulAracId = okulAracId;
                ViewBag.latitude = db.Araclar.Find(aracId).latitude;
                ViewBag.longitude = db.Araclar.Find(aracId).longitude;
                ViewBag.okulLatitude = db.OkulServisleri.Find(okulAracId.Value).Okullar.latitude;
                ViewBag.okulLongitude = db.OkulServisleri.Find(okulAracId.Value).Okullar.longitude;
                return View(vm);
            }
            else
            {
                return RedirectToAction("Okullar", "Sofor");
            }
        }
        [Attributes.SoforRoleControl]
        public ActionResult Okullar()
        {
            int sofId = (int)Session["sofId"];
            int aracId = (int)Session["aracId"];
            ViewModel vm = new ViewModel
            {
                Duraklar = db.Duraklar.ToList(),
                Faturalar = db.Faturalar.ToList(),
                Firmalar = db.Firmalar.ToList(),
                FirmaSoforleri = db.FirmaSoforleri.Where(x => x.sofId == sofId).ToList(),
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
                OkulServisleri = db.OkulServisleri.Where(x => x.aracId == aracId).ToList(),
                Rotalar = db.Rotalar.Where(x => x.OkulServisleri.aracId == aracId).ToList(),
                Araclar = db.Araclar.Where(x => x.aracId == aracId).ToList(),
                ServistekiOgrenciler = db.ServistekiOgrenciler.Where(x => x.OkulServisleri.aracId == aracId).ToList(),
                AracSoforleri = db.AracSoforleri.Where(x => x.aracId == aracId).ToList(),
                Soforler = db.Soforler.Where(x => x.sofId == sofId).ToList(),
                FirmaAraclari = db.FirmaAraclari.Where(x => x.aracId == aracId).ToList(),
                Veliler = db.Veliler.ToList()
            };
            return View(vm);
        }


        #region //Asenkron Sorgular
        [HttpPost]
        public ActionResult ServistekiOgrenciEkle(int okulAracId, int okulId, string ogrAd, string ogrSoyad, double ogrTCno, decimal ucret, TimeSpan alimZamani, TimeSpan teslimZamani, DateTime kayitTarihi, short ogretimTuru, short okuldanEveEvdenOkula)
        {
            try
            {
                if(db.Ogrenciler.Where(x=>x.okulId==okulId && x.ogrTCno==ogrTCno && x.ogrAd==ogrAd && x.ogrSoyad==ogrSoyad).Count() > 0)
                {
                    var ogrenci = db.Ogrenciler.Where(x => x.okulId == okulId && x.ogrTCno == ogrTCno && x.ogrAd == ogrAd && x.ogrSoyad == ogrSoyad).First();
                    if(db.ServistekiOgrenciler.Where(x => x.ogrId == ogrenci.ogrId && x.okulAracId!=okulAracId).Count() > 0)
                    {
                        TempData["Mesaj"] = "Bilgilerini girmiş olduğunuz öğrenci başka bir servisi kullanmaktadır.";
                        TempData["btn-renk"] = "btn-danger";
                    }
                    else
                    {
                        if (db.ServistekiOgrenciler.Where(x => x.ogrId == ogrenci.ogrId && x.okulAracId == okulAracId).Count() == 0)
                        {
                            db.ServistekiOgrenciler.Add(new ServistekiOgrenciler
                            {
                                alimZamani = alimZamani,
                                teslimZamani = teslimZamani,
                                kayitTarihi = kayitTarihi,
                                ogrId = ogrenci.ogrId,
                                ogretimTuru = ogretimTuru,
                                okulAracId = okulAracId,
                                ucret = ucret
                            });
                            db.SaveChanges();
                            TempData["Mesaj"] = "Kayıt İşlemi Başarılı.";
                            TempData["btn-renk"] = "btn-success";
                        }
                        else
                        {
                            TempData["Mesaj"] = "Bu öğrenci zaten servisinizde bulunmaktadır.";
                            TempData["btn-renk"] = "btn-warning";
                        }
                    }
                    
                }
                else
                {
                    TempData["Mesaj"] = "Bu okulda kayıtlı öğrenciler ile girmiş olduğunuz öğrenci bilgileri uyuşmamaktadır.";
                    TempData["btn-renk"] = "btn-warning";
                }
                
               
                //return Json(true, JsonRequestBehavior.AllowGet);
                return RedirectToAction("Servis","Sofor",new { okulAracId = okulAracId, okuldanEveEvdenOkula = okuldanEveEvdenOkula });
            }
            catch
            {
                TempData["Mesaj"] = "Bir hata olustu. Lutfen tekrar deneyiniz.";
                TempData["btn-renk"] = "btn-danger";
                //return Json(TempData["Mesaj"], JsonRequestBehavior.AllowGet);
                return RedirectToAction("Servis", "Sofor", new { okulAracId = okulAracId, okuldanEveEvdenOkula = okuldanEveEvdenOkula });
            }
        }
        [HttpPost]
        public ActionResult ServistekiOgrenciGuncelle(int okulAracId, int servistekiOgrenciId, decimal ucret, TimeSpan alimZamani, TimeSpan teslimZamani, short ogretimTuru, short okuldanEveEvdenOkula)
        {
            try
            {
                if (db.ServistekiOgrenciler.Find(servistekiOgrenciId) != null)
                {
                    var servistekiogrenci = db.ServistekiOgrenciler.Find(servistekiOgrenciId);
                    servistekiogrenci.alimZamani = alimZamani;
                    servistekiogrenci.teslimZamani = teslimZamani;
                    servistekiogrenci.ucret = ucret;
                    servistekiogrenci.ogretimTuru = ogretimTuru;
                    db.SaveChanges();
                }
                else
                {
                    TempData["Mesaj"] = "Düzenlenmeye çalışılan öğrenci daha önce silinmiş veya olmayan bir öğrencinin bilgilerini değiştirmeye çalışıyor olabilirsiniz.";
                    TempData["btn-renk"] = "btn-warning";
                }


                //return Json(true, JsonRequestBehavior.AllowGet);
                return RedirectToAction("Servis", "Sofor", new { okulAracId = okulAracId, okuldanEveEvdenOkula = okuldanEveEvdenOkula });
            }
            catch
            {
                TempData["Mesaj"] = "Bir hata olustu. Lutfen tekrar deneyiniz.";
                TempData["btn-renk"] = "btn-danger";
                //return Json(TempData["Mesaj"], JsonRequestBehavior.AllowGet);
                return RedirectToAction("Servis", "Sofor", new { okulAracId = okulAracId, okuldanEveEvdenOkula = okuldanEveEvdenOkula });
            }
        }
        [HttpGet]
        public ActionResult ServistekiOgrenciSil(int okulAracId, int servistekiOgrenciId, int sofId, short okuldanEveEvdenOkula)
        {
            if(sofId == (int)Session["sofId"])
            {
                try
                {
                    if (db.ServistekiOgrenciler.Find(servistekiOgrenciId) != null)
                    {
                        db.Odemeler.RemoveRange(db.Odemeler.Where(x => x.Faturalar.servistekiOgrenciId == servistekiOgrenciId));
                        db.Faturalar.RemoveRange(db.Faturalar.Where(x => x.servistekiOgrenciId == servistekiOgrenciId));
                        db.IndiBindiler.RemoveRange(db.IndiBindiler.Where(x => x.servistekiOgrenciId == servistekiOgrenciId));
                        db.ServistekiOgrenciler.Remove(db.ServistekiOgrenciler.Find(servistekiOgrenciId));
                        db.SaveChanges();
                        TempData["Mesaj"] = "İşlem Başarılı.";
                        TempData["btn-renk"] = "btn-succes";
                    }
                    else
                    {
                        TempData["Mesaj"] = "Düzenlenmeye çalışılan öğrenci daha önce silinmiş veya olmayan bir öğrencinin bilgilerini değiştirmeye çalışıyor olabilirsiniz.";
                        TempData["btn-renk"] = "btn-warning";
                    }
                    //return Json(true, JsonRequestBehavior.AllowGet);
                    return RedirectToAction("Servis", "Sofor", new { okulAracId = okulAracId, okuldanEveEvdenOkula = okuldanEveEvdenOkula });
                }
                catch
                {
                    TempData["Mesaj"] = "Bir hata olustu. Lutfen tekrar deneyiniz.";
                    TempData["btn-renk"] = "btn-danger";
                    //return Json(TempData["Mesaj"], JsonRequestBehavior.AllowGet);
                    return RedirectToAction("Servis", "Sofor", new { okulAracId = okulAracId, okuldanEveEvdenOkula = okuldanEveEvdenOkula });
                }
            }
            else
            {
                TempData["Mesaj"] = "Bu fonksiyonu çalıştırma yetkiniz bulunmamaktadır.";
                return RedirectToAction("SoforGiris", "Sofor");
            }

        }


        [HttpPost]
        public ActionResult AylikFaturaCikart(int okulAracId, List<int> servistekiOgrIdleri, short okuldanEveEvdenOkula)
        {
            try
            {
                foreach(int servistekiOgrId in servistekiOgrIdleri)
                {
                    if (db.Faturalar.Where(x=>x.kayitTarihi.Month==DateTime.Now.Month && x.kayitTarihi.Year==DateTime.Now.Year && x.servistekiOgrenciId == servistekiOgrId).Count()==0)
                    {//Eğer bu ay fatura eklenmemişse
                        var servistekiOgrenci = db.ServistekiOgrenciler.Find(servistekiOgrId);
                        db.Faturalar.Add(new Faturalar
                        {
                            kayitTarihi=DateTime.Now,
                            servistekiOgrenciId=servistekiOgrId,
                            kullanimDurumu=true,
                            okulAracId= servistekiOgrenci.okulAracId,
                            ogrId = servistekiOgrenci.ogrId,
                            faturaTutari = servistekiOgrenci.ucret
                        });
                        db.SaveChanges();
                        TempData["Mesaj"] = "Fatura Kayıt İşlemleri Başarılı.";
                        TempData["btn-renk"] = "btn-success";
                    }
                    else
                    {
                        TempData["Mesaj"] = "Seçtiğiniz öğrencilerden biri/bazılarına zaten bu ay fatura çıkartılmış. Faturası olmayanlara fatura çıkartıldı.";
                        TempData["btn-renk"] = "btn-warning";
                    }
                }
                return RedirectToAction("Servis", "Sofor", new { okulAracId = okulAracId, okuldanEveEvdenOkula = okuldanEveEvdenOkula });
            }
            catch
            {
                TempData["Mesaj"] = "Bir hata olustu. Lutfen tekrar deneyiniz.";
                TempData["btn-renk"] = "btn-danger";
                //return Json(TempData["Mesaj"], JsonRequestBehavior.AllowGet);
                return RedirectToAction("Servis", "Sofor", new { okulAracId = okulAracId, okuldanEveEvdenOkula = okuldanEveEvdenOkula });
            }
        }
        [HttpPost]
        public ActionResult AylikOdemeAl(int okulAracId, int servistekiOgrenciId, decimal ucret, short okuldanEveEvdenOkula)
        {
            try
            {
                if (ucret > 0)
                {
                    var servistekiOgrencininFaturalari = db.Faturalar.Where(x => x.servistekiOgrenciId == servistekiOgrenciId);
                    if (servistekiOgrencininFaturalari.Count() > 0)
                    {

                        foreach (var servistekiOgrencininFaturasi in servistekiOgrencininFaturalari.OrderByDescending(x => x.faturaId))
                        {
                            decimal odenen = 0;
                            foreach (var odeme in db.Odemeler.Where(x => x.faturaId == servistekiOgrencininFaturasi.faturaId))
                            {
                                odenen += odeme.miktar;
                            }

                            decimal odenmemisMiktar = servistekiOgrencininFaturasi.faturaTutari - odenen;

                            if (ucret >= odenmemisMiktar)
                            {
                                db.Odemeler.Add(new Odemeler
                                {
                                    faturaId = servistekiOgrencininFaturasi.faturaId,
                                    miktar = odenmemisMiktar,
                                    odemeTarihi = DateTime.Now
                                });
                            }
                            ucret -= odenmemisMiktar;

                            if (servistekiOgrencininFaturalari.First().faturaId != servistekiOgrencininFaturasi.faturaId)
                            {
                                if (ucret <= 0)
                                {
                                    TempData["Mesaj"] = "Girilen ücret tüm faturaları ödemeye yetmiyor. Ödeme kadar ücret tüm faturalardan düşüldü.";
                                    TempData["btn-renk"] = "btn-warning";
                                }
                            }
                            else
                            {
                                TempData["Mesaj"] = "Fatura Ödeme İşlemleri Başarılı. Bu öğrencinin tüm faturaları ödendi.";
                                TempData["btn-renk"] = "btn-success";
                            }
                        }
                    }
                    else
                    {
                        TempData["Mesaj"] = "Seçtiğiniz öğrencinin faturası bulunmuyor.";
                        TempData["btn-renk"] = "btn-warning";
                    }
                    db.SaveChanges();
                }
                else
                {
                    TempData["Mesaj"] = "Geçerli bir ücret girmediniz.";
                    TempData["btn-renk"] = "btn-warning";
                }
                return RedirectToAction("Servis", "Sofor", new { okulAracId = okulAracId, okuldanEveEvdenOkula = okuldanEveEvdenOkula });
            }
            catch(Exception e)
            {
                Console.Write(e);
                TempData["Mesaj"] = "Bir hata olustu. Lutfen tekrar deneyiniz.";
                TempData["btn-renk"] = "btn-danger";
                //return Json(TempData["Mesaj"], JsonRequestBehavior.AllowGet);
                return RedirectToAction("Servis", "Sofor", new { okulAracId = okulAracId, okuldanEveEvdenOkula = okuldanEveEvdenOkula });
            }
        }
        [HttpGet]
        public ActionResult ServiseBindir(int okulAracId, int servistekiOgrenciId, short okuldanEveEvdenOkula)
        {
            var date = DateTime.Now.Date;
            try
            {
                
                if (okuldanEveEvdenOkula == 0)
                {//okuldan eve istikamet
                    var indibindi = db.IndiBindiler.Where(x => x.servistekiOgrenciId == servistekiOgrenciId && x.okuldanEveEvdenOkula == 0 && DbFunctions.TruncateTime(x.bindiTarihi) == date);
                    if (indibindi.Count() == 0)
                    {//eğer bugün okuldan eve istikamette biniş yoksa biniş verisi ekle
                        db.IndiBindiler.Add(new IndiBindiler
                        {
                            bindiTarihi = DateTime.Now,
                            okuldanEveEvdenOkula = 0,
                            servistekiOgrenciId = servistekiOgrenciId,
                            indiTarihi = new DateTime(0001, 01, 01, 01, 01, 01).Date
                        });
                    }
                    else
                    {//eğer bugün okuldan eve istikamette biniş zaten varsa var diye rapor ver.
                        TempData["Mesaj"] = "Seçtiğiniz öğrenci zaten okuldan eve istikametinde serviste bulunuyor.";
                        TempData["btn-renk"] = "btn-warning";
                    }
                }
                else if(okuldanEveEvdenOkula == 1)
                {//evden okula istikamet
                    var indibindi = db.IndiBindiler.Where(x => x.servistekiOgrenciId == servistekiOgrenciId && x.okuldanEveEvdenOkula == 1 && DbFunctions.TruncateTime(x.bindiTarihi) == date);
                    if (indibindi.Count() == 0)
                    {//Eğer bugün evden okula istikamette biniş yoksa biniş verisi ekle
                        db.IndiBindiler.Add(new IndiBindiler
                        {
                            bindiTarihi = DateTime.Now,
                            okuldanEveEvdenOkula = 1,
                            servistekiOgrenciId = servistekiOgrenciId,
                            indiTarihi = new DateTime(0001, 01, 01, 01, 01, 01).Date
                        });
                    }
                    else
                    {//Eğer bugün evden okula istikamette biniş zaten varsa var diye rapor ver.
                        TempData["Mesaj"] = "Seçtiğiniz öğrenci zaten evden okula istikametinde serviste bulunuyor.";
                        TempData["btn-renk"] = "btn-warning";
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Servis", "Sofor", new { okulAracId = okulAracId, okuldanEveEvdenOkula = okuldanEveEvdenOkula });
            }
            catch (Exception e)
            {
                Console.Write(e);
                TempData["Mesaj"] = "Bir hata olustu. Lutfen tekrar deneyiniz.";
                TempData["btn-renk"] = "btn-danger";
                //return Json(TempData["Mesaj"], JsonRequestBehavior.AllowGet);
                return RedirectToAction("Servis", "Sofor", new { okulAracId = okulAracId, okuldanEveEvdenOkula = okuldanEveEvdenOkula });
            }
        }
        [HttpGet]
        public ActionResult ServistenIndir(int okulAracId, int servistekiOgrenciId, short okuldanEveEvdenOkula)
        {
            var date = DateTime.Now.Date;
            try
            {
                if(okuldanEveEvdenOkula == 0)
                {//Okuldan eve istikamet
                    var indibindi = db.IndiBindiler.Where(x => x.servistekiOgrenciId == servistekiOgrenciId && x.okuldanEveEvdenOkula == 0 && DbFunctions.TruncateTime(x.bindiTarihi) == date).OrderByDescending(x => x.indiBindiId);
                    if (indibindi.Count() > 0)
                    {//Eğer okuldan eve istikamette biniş tarihi bugün girilmiş ise, girilen indibindi verisini bul ve indi tarihini ekle
                        indibindi.First().indiTarihi = DateTime.Now;
                    }
                    else
                    {//Eğer okuldan eve istikamette biniş tarihi girilmemişse, biniş olmadan iniş verisi girilemeyeceğini bildir.
                        TempData["Mesaj"] = DateTime.Now.ToShortDateString()+" tarihinde okuldan biniş kaydı alınmadan iniş kaydı yapılamaz.";
                        TempData["btn-renk"] = "btn-danger";
                    }
                }
                else if(okuldanEveEvdenOkula == 1)
                {//evden okula istikamet
                    var indibindi = db.IndiBindiler.Where(x => x.servistekiOgrenciId == servistekiOgrenciId && x.okuldanEveEvdenOkula == 1 && DbFunctions.TruncateTime(x.bindiTarihi) == date).OrderByDescending(x => x.indiBindiId);
                    if (indibindi.Count() > 0)
                    {//eğer evden okula istikamette biniş tarihi bugün girilmiş ise, girilen indibindi verisini bul ve indi tarihini ekle
                        
                        indibindi.First().indiTarihi = DateTime.Now;
                    }
                    else
                    {//Eğer evden okula istikamette biniş tarihi girilmemişse, biniş olmadan iniş verisi girilemeyeceğini bildir.
                        TempData["Mesaj"] = DateTime.Now.ToShortDateString() + " tarihinde evden biniş kaydı alınmadan iniş kaydı yapılamaz.";
                        TempData["btn-renk"] = "btn-danger";
                    }
                }
                db.SaveChanges();
                
                return RedirectToAction("Servis", "Sofor", new { okulAracId = okulAracId, okuldanEveEvdenOkula = okuldanEveEvdenOkula });
            }
            catch (Exception e)
            {
                Console.Write(e);
                TempData["Mesaj"] = "Bir hata olustu. Lutfen tekrar deneyiniz.";
                TempData["btn-renk"] = "btn-danger";
                //return Json(TempData["Mesaj"], JsonRequestBehavior.AllowGet);
                return RedirectToAction("Servis", "Sofor", new { okulAracId = okulAracId, okuldanEveEvdenOkula = okuldanEveEvdenOkula });
            }
        }

        [HttpPost]
        [Attributes.SoforRoleControl]
        public JsonResult ServisKonumGetir(int sofId, int aracId)
        {
            if (sofId == Convert.ToInt32(Session["sofId"]))
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
        private class LatLng
        {
            public string lat { get; set; }
            public string lng { get; set; }
        }
        public class SofAndroidGiris
        {
            public string sofEmail { get; set; }
            public string sofParola { get; set; }
            public string plaka { get; set; }
        }

        [HttpPost]
        public JsonResult SoforAndroidGiris(string sofEmail, string sofParola, string plaka)
        {
            if (ModelState.IsValid)
            {
                sofParola = sofParola.ToSHA(Crypto.SHA_Type.SHA256);
                var sofor = db.Soforler.Where(x => x.sofEmail == sofEmail && x.sofParola == sofParola);
                if (sofor.Count()==1)
                {
                    int sofId = sofor.First().sofId;
                    var servisSoforu = db.AracSoforleri.Where(x => x.sofId == sofId && x.Araclar.plaka == plaka);
                    if (servisSoforu.Count()==1)
                    {
                        return Json(servisSoforu.First().Araclar.authCode,JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                        //return Json("Giriş yapıtğınız şoför bilgisinin, plaka ile bir ilişiği bulunmamaktadır.", JsonRequestBehavior.AllowGet);
                    }   
                }
                return Json(false, JsonRequestBehavior.AllowGet);
                //return Json("Email / Parola bilgilerini yanlış girdiniz.", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
                //return Json("Lütfen giriş bilgileriniz eksiksiz giriniz.", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ServisKonumGuncelle(string authCode, int aracId, string latitude, string longitude, int sofId)
        {
            if(sofId == (int)Session["sofId"])
            {
                try
                {
                    var servisSoforu = db.AracSoforleri.Where(x => x.aracId == aracId && x.sofId == sofId);
                    if (servisSoforu.Count() == 1)
                    {
                        var servis = db.Araclar.Where(x => x.aracId == aracId && x.authCode == authCode);
                        if (servis.Count() == 1)
                        {
                            latitude = latitude.Substring(0, 10);
                            longitude = longitude.Substring(0, 10);
                            servis.First().latitude = latitude;
                            servis.First().longitude = longitude;
                            db.SaveChanges();
                            return Json(0, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(2, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(-1, JsonRequestBehavior.AllowGet);
                    }
                    
                }
                catch
                {
                    return Json(1,JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ServisAndroidKonumGuncelle(string authCode, string latitude, string longitude)
        {
            try
            {
                var servis = db.Araclar.Where(x => x.authCode == authCode);
                if (servis.Count() == 1)
                {
                    latitude = latitude.Substring(0, 10);
                    longitude = longitude.Substring(0, 10);
                    servis.First().latitude = latitude;
                    servis.First().longitude = longitude;
                    db.SaveChanges();
                    return Json(0, JsonRequestBehavior.AllowGet);//İŞlem doğru
                }
                else
                {
                    return Json(2, JsonRequestBehavior.AllowGet);//authCode'ye ait servis bulunamadı
                }
            }
            catch(Exception e)
            {
                return Json(e.Message, JsonRequestBehavior.AllowGet);//hata
            }
        }

        #endregion
    }
}