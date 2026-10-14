using Microsoft.EntityFrameworkCore;
using SharedLibrary.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace SharedLibrary.Data
{
   public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }
        public DbSet<Ticket> Ticket { get; set; }

        public DbSet<ApplicationUser> AppUser { get; set; }

        public DbSet<Comment> Comment { get; set; }

        //Configure the relationshps
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           

            modelBuilder.Entity<Ticket>()
                .HasOne(u => u.User)
                .WithMany(t => t.CreatedTickets)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Ticket>()
                .HasOne(u => u.Agent)
                .WithMany(t => t.AssignedTickets)
                .HasForeignKey(u => u.AgentId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Comment>()
                .HasOne(a => a.Agent)
                .WithMany(c => c.Comments)
                .HasForeignKey(f => f.AgentId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Comment>()
                .HasOne(a => a.Ticket)
                .WithOne(c => c.Comment)
                .HasForeignKey<Comment>(c => c.TicketId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
