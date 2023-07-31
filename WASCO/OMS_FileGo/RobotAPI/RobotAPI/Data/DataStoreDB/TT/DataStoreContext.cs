using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using RobotAPI.Data;

namespace RobotAPI.Data.DataStoreDB.TT {
    public partial class DataStoreContext : DbContext {
        //public DataStoreContext()
        //{
        //} 
        //public DataStoreContext(DbContextOptions<DataStoreContext> options)
        //    : base(options)
        //{
        //} 
        public virtual DbSet<accident_data> accident_data { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseNpgsql("Host=119.59.126.76;Database=DataStore;Username=gauser;Password=gooddevxox");
        //    }
        //} 
        //private readonly IConfiguration Configuration;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            //optionsBuilder.UseSqlServer(Configuration["GAEntities:ConnectionString"]);
            optionsBuilder.UseNpgsql(Globals.DataStoreConn);
        }
        public DataStoreContext() { }

        public DataStoreContext(DbContextOptions<DataStoreContext> options)
            : base(options) {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
