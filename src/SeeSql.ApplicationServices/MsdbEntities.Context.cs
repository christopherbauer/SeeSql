﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SeeSql.ApplicationServices
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MsdbEntities : DbContext
    {
        public MsdbEntities()
            : base("name=MsdbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<sysjobstepslog> sysjobstepslogs { get; set; }
        public virtual DbSet<sysschedule> sysschedules { get; set; }
        public virtual DbSet<sysjobactivity> sysjobactivities { get; set; }
        public virtual DbSet<sysjobhistory> sysjobhistories { get; set; }
        public virtual DbSet<sysjob> sysjobs { get; set; }
        public virtual DbSet<sysjobschedule> sysjobschedules { get; set; }
        public virtual DbSet<sysjobserver> sysjobservers { get; set; }
        public virtual DbSet<sysjobstep> sysjobsteps { get; set; }
    }
}
