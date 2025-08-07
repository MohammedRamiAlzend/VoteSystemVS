using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Admin> Admins { get; set; }
    public DbSet<AttendanceUser> AttendanceUsers { get; set; }
    public DbSet<OTPCode> OTPCodes { get; set; }
    public DbSet<SystemLog> SystemLogs { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Vote> Votes { get; set; }
    public DbSet<VoteQuestion> VoteQuestions { get; set; }
    public DbSet<VoteQuestionOption> VoteQuestionOptions { get; set; }
    public DbSet<VoteSession> VoteSessions { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasOne(u => u.CreatedByAdmin)
            .WithMany(a => a.CreatedUsersByAdmin)
            .HasForeignKey(u => u.CreatedByAdminId);

        modelBuilder.Entity<VoteQuestionOption>()
            .HasOne(o => o.VoteQuestion)
            .WithMany()
            .HasForeignKey(o => o.VoteQuestionId);

        modelBuilder.Entity<Vote>()
            .HasOne(v => v.User)
            .WithMany()
            .HasForeignKey(v => v.UserId);

        modelBuilder.Entity<Vote>()
            .HasOne(v => v.VoteQuestionOption)
            .WithMany()
            .HasForeignKey(v => v.VoteQuestionOptionId);

        modelBuilder.Entity<VoteQuestion>()
            .HasOne(vq => vq.VoteSession)
            .WithMany()
            .HasForeignKey(vq => vq.VoteSessionId);
        
        modelBuilder.Entity<AttendanceUser>()
            .HasOne(au => au.User)
            .WithMany()
            .HasForeignKey(au => au.UserId);

        modelBuilder.Entity<AttendanceUser>()
            .HasOne(au => au.VoteSession)
            .WithMany()
            .HasForeignKey(au => au.VoteSessionId);
        
        modelBuilder.Entity<AttendanceUser>()
            .HasOne(au => au.CreatedByAdmin)
            .WithMany()
            .HasForeignKey(au => au.CreatedByAdminId);
    }
}
