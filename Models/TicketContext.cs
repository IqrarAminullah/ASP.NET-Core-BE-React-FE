using Microsoft.EntityFrameworkCore;

namespace TestApi.Models
{
    public class TicketContext : DbContext
    {
        public TicketContext(DbContextOptions<TicketContext> options) : base(options) {}
        public virtual DbSet<Ticket> Tickets {get;set;}
        public virtual DbSet<User> Users{get;set;}
        public virtual DbSet<Category> Categories{get;set;}
        public virtual DbSet<UserAuth> UserAuths{get;set;}
        
        protected override void OnModelCreating(ModelBuilder modelBuilder){
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.assignee)
                .WithMany(u => u.assignedTickets)
                .HasForeignKey(t => t.assigneeId);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.owner)
                .WithMany(u => u.ownedTickets)
                .HasForeignKey(t => t.ownerId);


        }
    }
}