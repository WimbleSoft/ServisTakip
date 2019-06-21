using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServisTakip.Models
{
    public class ViewModel
    {
        public List<OkulServisleri> OkulServisleri { get; set; }
        public List<Mudurler> Mudurler { get; set; }
        public List<Okullar> Okullar { get; set; }
        public List<OkulCinsleri> OkulCinsleri { get; set; }
        public List<OkulTurleri> OkulTurleri { get; set; }
        public List<Ogrenciler> Ogrenciler { get; set; }
        public List<Rotalar> Rotalar { get; set; }
        public List<Duraklar> Duraklar { get; set; }
        public List<ServistekiOgrenciler> ServistekiOgrenciler { get; set; }
        public List<IndiBindiler> IndiBindiler { get; set; }
        public List<Servisler> Servisler { get; set; }
        public List<Soforler> Soforler { get; set; }
        public List<FirmaSoforleri> FirmaSoforleri { get; set; }
        public List<OgrenciVelileri> OgrenciVelileri { get; set; }
        public List<Veliler> Veliler { get; set; }
        public List<Firmalar> Firmalar { get; set; }
        public List<Faturalar> Faturalar { get; set; }
        public List<Odemeler> Odemeler { get; set; }
        public List<Iller> Iller { get; set; }
        public List<Ilceler> Ilceler { get; set; }
        public List<ServisSoforleri> ServisSoforleri { get; set; }
        public List<FirmaServisleri> FirmaServisleri { get; set; }
    }
}