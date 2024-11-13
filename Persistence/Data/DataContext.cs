using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions options)
            : base(options) { }

        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<PerformanceFeedback> PerformanceFeedbacks { get; set; }
        public DbSet<MaintenancedRecord> MaintenancedRecords { get; set; }
        public DbSet<RentalHistory> RentalHistories { get; set; }
        public DbSet<SparePart> SpareParts { get; set; }
        public DbSet<RentalRequest> RentalRequests { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Images> Images { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionDetail> TransactionDetails { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<SparePartFeedback> SparePartFeedbacks { get; set; }
        public DbSet<SparePartImage> sparePartImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Equipment>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<PerformanceFeedback>().HasQueryFilter(pf => !pf.IsDeleted);
            modelBuilder.Entity<MaintenancedRecord>().HasQueryFilter(mr => !mr.IsDeleted);
            modelBuilder.Entity<RentalHistory>().HasQueryFilter(rh => !rh.IsDeleted);
            modelBuilder.Entity<SparePart>().HasQueryFilter(sp => !sp.IsDeleted);
            modelBuilder.Entity<RentalRequest>().HasQueryFilter(rr => !rr.IsDeleted);
            modelBuilder.Entity<Payment>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<Images>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<Transaction>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<TransactionDetail>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<SparePartFeedback>().HasQueryFilter(sf => !sf.IsDeleted);
            modelBuilder.Entity<SparePartImage>().HasQueryFilter(si => !si.IsDeleted);

            modelBuilder
                .Entity<PerformanceFeedback>()
                .HasOne(pf => pf.User)
                .WithMany(u => u.PerformanceFeedbacks)
                .HasForeignKey(pf => pf.UserId);

            modelBuilder
                .Entity<RentalHistory>()
                .HasOne(rh => rh.Renter)
                .WithMany(u => u.RentalHistories)
                .HasForeignKey(rh => rh.RenterId);

            modelBuilder
                .Entity<Equipment>()
                .HasMany(e => e.PerformanceFeedbacks)
                .WithOne(pf => pf.Equipment)
                .HasForeignKey(pf => pf.EquipmentId);

            modelBuilder
                .Entity<Equipment>()
                .HasMany(e => e.MaintenancedRecords)
                .WithOne(mr => mr.Equipment)
                .HasForeignKey(mr => mr.EquipmentId);

            modelBuilder
                .Entity<Equipment>()
                .HasMany(e => e.RentalHistories)
                .WithOne(rh => rh.Equipment)
                .HasForeignKey(rh => rh.EquipmentId);

            modelBuilder
                .Entity<Equipment>()
                .HasMany(e => e.SpareParts)
                .WithOne(sp => sp.Equipment)
                .HasForeignKey(sp => sp.EquipmentId);

            modelBuilder
                .Entity<RentalRequest>()
                .HasOne(rr => rr.User)
                .WithMany(u => u.RentalRequests)
                .HasForeignKey(rr => rr.UserId);

            modelBuilder
                .Entity<RentalRequest>()
                .HasOne(rr => rr.Equipment)
                .WithMany(e => e.RentalRequests)
                .HasForeignKey(rr => rr.EquipmentId);

            modelBuilder
                .Entity<Images>()
                .HasOne(p => p.Equipment)
                .WithOne(i => i.Images)
                .HasForeignKey<Images>(p => p.EquipmentId);

            modelBuilder
                .Entity<Transaction>()
                .HasMany(t => t.TransactionDetails)
                .WithOne(td => td.Transactions)
                .HasForeignKey(td => td.TransactionId);

            modelBuilder
                .Entity<TransactionDetail>()
                .HasOne(td => td.SparePart)
                .WithMany(sp => sp.TransactionDetails)
                .HasForeignKey(td => td.SparePartId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<TransactionDetail>()
                .HasOne(td => td.Equipment)
                .WithMany(e => e.TransactionDetails)
                .HasForeignKey(td => td.EquipmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<TransactionDetail>()
                .HasOne(td => td.Transactions)
                .WithMany(e => e.TransactionDetails)
                .HasForeignKey(td => td.TransactionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
