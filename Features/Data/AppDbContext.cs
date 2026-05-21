using LibraryManagementSystem.Features.Data.Models;
using Microsoft.EntityFrameworkCore;


namespace LibraryManagementSystem.Features.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Borrow> Borrows { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Member> Members { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable(t => t.HasCheckConstraint("CK_Book_Quantity_NonNegative", "Quantity >= 0"));
            });
        }

    }
}
