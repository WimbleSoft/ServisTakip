﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServisTakip
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ServisSistemEntities : DbContext
    {
        public ServisSistemEntities()
            : base("name=ServisSistemEntities")
        {

            Database.Connection.ConnectionString = Database.Connection.ConnectionString.Replace("yyyyy", "yyyyy");
            Database.Connection.ConnectionString = Database.Connection.ConnectionString.Replace("xxxxx", "yyyyy");
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Duraklar> Duraklar { get; set; }
        public virtual DbSet<Faturalar> Faturalar { get; set; }
        public virtual DbSet<Firmalar> Firmalar { get; set; }
        public virtual DbSet<FirmaServisleri> FirmaServisleri { get; set; }
        public virtual DbSet<FirmaSoforleri> FirmaSoforleri { get; set; }
        public virtual DbSet<Ilceler> Ilceler { get; set; }
        public virtual DbSet<Iller> Iller { get; set; }
        public virtual DbSet<IndiBindiler> IndiBindiler { get; set; }
        public virtual DbSet<Mudurler> Mudurler { get; set; }
        public virtual DbSet<Odemeler> Odemeler { get; set; }
        public virtual DbSet<Ogrenciler> Ogrenciler { get; set; }
        public virtual DbSet<OgrenciVelileri> OgrenciVelileri { get; set; }
        public virtual DbSet<OkulCinsleri> OkulCinsleri { get; set; }
        public virtual DbSet<Okullar> Okullar { get; set; }
        public virtual DbSet<OkulServisleri> OkulServisleri { get; set; }
        public virtual DbSet<OkulTurleri> OkulTurleri { get; set; }
        public virtual DbSet<Rotalar> Rotalar { get; set; }
        public virtual DbSet<Servisler> Servisler { get; set; }
        public virtual DbSet<ServisSoforleri> ServisSoforleri { get; set; }
        public virtual DbSet<ServistekiOgrenciler> ServistekiOgrenciler { get; set; }
        public virtual DbSet<Soforler> Soforler { get; set; }
        public virtual DbSet<Veliler> Veliler { get; set; }
    }
}