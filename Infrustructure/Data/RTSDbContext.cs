using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Emit;


namespace Infrastructure.Data
{
    public class RTSDbContext : DbContext
    {
        private string connectionString { get; set; }

        public RTSDbContext()
        {
        }

        public RTSDbContext(DbContextOptions<RTSDbContext> options) : base(options)
        {
            var sqlServerOptionsExtension =
                options.FindExtension<SqlServerOptionsExtension>();
            if (sqlServerOptionsExtension != null)
            {
                connectionString = sqlServerOptionsExtension.ConnectionString;
            }
        }


        public virtual DbSet<InvoiceDocument> InvoiceDocument { get; set; }
        public virtual DbSet<InDependentCreditNote> InDependentCreditNote { get; set; }
        public virtual DbSet<DependentCreditNote> DependentCreditNote { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<InvoiceDocument>(b =>
            {
                b.ToTable("RTS_InvoiceDocument");
                b.HasKey(u => new
                {
                    u.InvoiceNumber
                });

                b.HasIndex(u => new
                {
                    u.InvoiceNumber,
                }).HasName("RTS_InvoiceDocument_IDX1").IsUnique();

                b.Property(u => u.InvoiceNumber).IsRequired();
                b.Property(u => u.TotalAmount).IsRequired();
                b.Property(u => u.ExternalInvoiceNumber).IsRequired();
                b.Property(p => p.TotalAmount).HasColumnType("decimal(18, 2)");
                
            });
           

            builder.Entity<InDependentCreditNote>(b =>
            {
                b.ToTable("RTS_InDependentCreditNote");
                b.HasKey(u => new
                {
                    u.CreditNumber
                });

                b.HasIndex(u => new
                {
                    u.CreditNumber,
                }).HasName("RTS_InDependentCreditNote_IDX01").IsUnique();

                b.Property(u => u.ExternalCreditNumber).IsRequired().HasMaxLength(10);
                b.Property(p => p.TotalAmount).HasColumnType("decimal(18, 2)");
                b.Property(u => u.CreditNumber).IsRequired().HasMaxLength(10);
            });


            builder.Entity<DependentCreditNote>(b =>
            {
                b.ToTable("RTS_DependentCreditNote");
                b.HasKey(u => new
                {
                    u.CreditNumber
                });

                b.HasIndex(u => new
                {
                    u.CreditNumber,
                }).HasName("RTSCreditNumber_IDX01").IsUnique();

                b.Property(u => u.ExternalCreditNumber).IsRequired();
                b.Property(u => u.TotalAmount).IsRequired();
                b.Property(p => p.TotalAmount).HasColumnType("decimal(18, 2)");
                b.Property(u => u.CreditNumber).HasMaxLength(10).IsRequired();
            });


            //builder.Entity<DependentCreditNote>()
            //    .HasOne(dc => dc.ParentInvoiceNumber)
            //   .WithMany(id => id.DependentCreditNote)
            //    .HasForeignKey(dc => dc.InvoiceDocument);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);

            // optionsBuilder.UseSqlServer("Data Source=DESKTOP-73KNC10\\SQLEXPRESS;Database=RTSBD;User ID=sa;Password=123456");
        }
    }
}