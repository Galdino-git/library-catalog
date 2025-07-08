using Microsoft.EntityFrameworkCore;
using MyLib.Domain.Entities;

namespace MyLib.Infrastructure.Data
{
    public class BookCatalogDbContext(DbContextOptions<BookCatalogDbContext> options) : DbContext(options)
    {
        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt);

                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Author).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Gender).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Publisher).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PublishedYear).IsRequired();

                entity.Property(e => e.ISBN).IsRequired().HasMaxLength(13);
                entity.HasIndex(e => e.ISBN).IsUnique();

                entity.Property(e => e.Synopsis).IsRequired().HasMaxLength(5000);
                entity.Property(e => e.CoverImageUrl).HasMaxLength(2048);
                entity.Property(e => e.CoverImage);

                entity.Property(e => e.RegisteredByUserId);
                entity.HasOne(b => b.RegisteredByUser)
                    .WithMany(u => u.Books)
                    .HasForeignKey(b => b.RegisteredByUserId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt);

                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.Username).IsUnique();

                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(256);

                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Email).IsUnique();

                entity.HasMany(e => e.Books)
                    .WithOne()
                    .HasForeignKey(b => b.RegisteredByUserId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}
