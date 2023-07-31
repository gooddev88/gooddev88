using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
 


namespace Robot.Data.DATASTOREDB.TT {
    public partial class DataStoreContext : DbContext {    
        public virtual DbSet<data_accidents> data_accidents { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseNpgsql(Globals.DataStoreConn);
        }
        public DataStoreContext() { }
        public DataStoreContext(DbContextOptions<DataStoreContext> options)
            : base(options) {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<data_accidents>(entity => {
                entity.ToTable("data_accidents");
                entity.Property(e => e.id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn(); 
                entity.Property(e => e.accidentid)
                    .HasMaxLength(255)
                    .HasColumnName("accidentid"); 
            }); 
            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
