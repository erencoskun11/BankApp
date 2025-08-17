using BankApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using BankAppDomain.Views;
using BankAppDomain.Entities;

namespace BankApp.Infrastructure.Data
{
    public class BankDbContext : DbContext
    {
        public BankDbContext(DbContextOptions<BankDbContext> options)
            : base(options)
        {
        }
        //view classlari
        public DbSet<PersonalFinancialInfoView> PersonalFinancialInfoViews { get; set; }

        public DbSet<CustomerAccountCardView> CustomerAccountCardView { get; set; }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<CardType> CardTypes { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.NationalId)
                .IsUnique();

            modelBuilder.Entity<Account>()
                .HasIndex(a => a.AccountNumber);

            modelBuilder.Entity<Account>()
                .HasIndex(a => a.IBAN)
                .IsUnique();

            modelBuilder.Entity<Card>()
                .HasIndex(c => c.CardNumber)
                .IsUnique();

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Accounts)
                .WithOne(a => a.Customer)
                .HasForeignKey(a => a.CustomerId);

            modelBuilder.Entity<Account>()
                .HasMany(x=>x.Cards)
                .WithOne(c => c.Account)
                .HasForeignKey(c => c.AccountId);

            modelBuilder.Entity<Account>()
                .HasMany(a => a.Transactions)
                .WithOne(t => t.Account)
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Card>()
                .HasMany(c => c.Transactions)
                .WithOne(t => t.Card)
                .HasForeignKey(t => t.CardId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.TransactionType)
                .WithMany(tt => tt.Transactions)
                .HasForeignKey(t => t.TransactionTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Card>()
                .HasOne(c => c.CardType)
                .WithMany(ct => ct.Cards)
                .HasForeignKey(c => c.CardTypeId);

            modelBuilder.Entity<Customer>()
    .Property(c => c.RiskLimit)
    .HasPrecision(18, 2); // örnek

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasPrecision(18, 2);





            modelBuilder.Entity<CardType>().HasData(
                new CardType { Id = 1, Name = "Bank Card" },
                new CardType { Id = 2, Name = "Credit Card" }
            );

            modelBuilder.Entity<CustomerAccountCardView>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("vw_CustomerAccountCard");
                entity.Property(e => e.CustomerId).HasColumnName("CustomerId");
                entity.Property(e => e.FullName).HasColumnName("FullName");
                entity.Property(e => e.NationalId).HasColumnName("NationalId");
                entity.Property(e => e.AccountId).HasColumnName("AccountId");
                entity.Property(e => e.AccountNumber).HasColumnName("AccountNumber");
                entity.Property(e => e.CardId).HasColumnName("CardId");
                entity.Property(e => e.CardNumber).HasColumnName("CardNumber");
            });
            modelBuilder.Entity<PersonalFinancialInfoView>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("vw_PersonalFinancialInfo");
                entity.Property(e => e.CustomerId).HasColumnName("CustomerId");
                entity.Property(e => e.FullName).HasColumnName("FullName");
                entity.Property(e => e.MaskedNationalId).HasColumnName("MaskedNationalId");
                entity.Property(e => e.AccountCount).HasColumnName("AccountCount");
                entity.Property(e => e.MaskedAccountNumbers).HasColumnName("MaskedAccountNumbers");
            });
        }
    }
}
