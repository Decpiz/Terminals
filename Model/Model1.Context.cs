﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Terminals.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class HranitelPRO7 : DbContext
    {
        private static HranitelPRO7 instance;
        public HranitelPRO7()
            : base("name=HranitelPRO7")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public static HranitelPRO7 GetContext()
        {
            if (instance == null)
                instance = new HranitelPRO7();
            return instance;
        }

        public virtual DbSet<Celi> Celi { get; set; }
        public virtual DbSet<CherniySpisok> CherniySpisok { get; set; }
        public virtual DbSet<Dolzhnosti> Dolzhnosti { get; set; }
        public virtual DbSet<GrupZajavki> GrupZajavki { get; set; }
        public virtual DbSet<Organizacii> Organizacii { get; set; }
        public virtual DbSet<Pasport> Pasport { get; set; }
        public virtual DbSet<Podrazdelenia> Podrazdelenia { get; set; }
        public virtual DbSet<Polzovateli> Polzovateli { get; set; }
        public virtual DbSet<Posetiteli> Posetiteli { get; set; }
        public virtual DbSet<Sotrudniki> Sotrudniki { get; set; }
        public virtual DbSet<Statusi> Statusi { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Zajavki> Zajavki { get; set; }
    }
}
