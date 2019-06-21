using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServisTakip.Models
{
    public class VeliGirisModel
    {
        [Required]
        [Display(Name = "veliEmail")]
        public string veliEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "veliParola")]
        public string veliParola { get; set; }

        public string returnUrl { get; set; }

    }
    public class SoforGirisModel
    {
        [Required]
        [Display(Name = "sofEmail")]
        public string sofEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "sofParola")]
        public string sofParola { get; set; }

        public string returnUrl { get; set; }

        public string plaka { get; set; }

    }
    public class MudurGirisModel
    {
        [Required]
        [Display(Name = "mudurEmail")]
        public string mudurEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "mudurParola")]
        public string mudurParola { get; set; }
        public string returnUrl { get; set; }

    }
    public class FirmaGirisModel
    {
        [Required]
        [Display(Name = "firEmail")]
        public string firEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "firParola")]
        public string firParola { get; set; }
        public string returnUrl { get; set; }

    }
}