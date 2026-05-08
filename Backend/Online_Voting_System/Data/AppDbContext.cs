using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Online_Voting_System.Domain.Entity;

namespace Online_Voting_System.Data
{
    public class AppDbContext : IdentityDbContext<Voter>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Election> Elections { get; set; }
        public DbSet<Party> Parties { get; set; }
        public DbSet<Vote> Votes { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // (Cascade) default by EF Core
            //builder.Entity<Vote>()
            //    .HasOne(c => c.Candidates)
            //    .WithMany()
            //    .HasForeignKey(f => f.CandidateId)
            //    .OnDelete(DeleteBehavior.Cascade);

            // Prevent automatic delete behaviour on vote if election is deleted
            builder.Entity<Vote>()
                .HasOne(e => e.Elections)
                .WithMany()
                .HasForeignKey(f => f.ElectionId)
                .OnDelete(DeleteBehavior.NoAction);
        }

    }
}
