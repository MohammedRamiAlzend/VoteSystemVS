using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Data;


public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // مجموعات الكيانات
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<VotingSession> VotingSessions { get; set; }
    public DbSet<VotingOption> VotingOptions { get; set; }
    public DbSet<Vote> Votes { get; set; }
    public DbSet<VerificationCode> VerificationCodes { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<SystemSetting> SystemSettings { get; set; }
    public DbSet<Association> Associations { get; set; }
    public DbSet<VotingItem> VotingItems { get; set; }
    public DbSet<VotingResult> VotingResults { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // تكوين جميع الكيانات لاستخدام Guid كمعرّف رئيسي
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            modelBuilder.Entity(entityType.Name)
                .Property<Guid>("Id")
                .IsRequired();

            modelBuilder.Entity(entityType.Name)
                .HasKey("Id");
        }

        // تكوين العلاقة بين User و Role (Many-to-Many)
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Association to User Relationship
        modelBuilder.Entity<User>()
            .HasOne(u => u.Association)
            .WithMany(a => a.Members)
            .HasForeignKey(u => u.AssociationId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.AssignedByUser)
            .WithMany()
            .HasForeignKey(ur => ur.AssignedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // تكوين جلسة التصويت
        modelBuilder.Entity<VotingSession>()
            .HasOne(vs => vs.CreatedByUser)
            .WithMany(u => u.CreatedVotingSessions)
            .HasForeignKey(vs => vs.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<VotingSession>()
            .HasOne(vs => vs.ModifiedByUser)
            .WithMany(u => u.ModifiedVotingSessions)
            .HasForeignKey(vs => vs.ModifiedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        // Association to VotingSession Relationship
        modelBuilder.Entity<VotingSession>()
            .HasOne(vs => vs.Association)
            .WithMany(a => a.GeneralAssemblies)
            .HasForeignKey(vs => vs.AssociationId)
            .OnDelete(DeleteBehavior.Restrict);

        // تكوين خيار التصويت (VotingOption related to VotingItem)
        modelBuilder.Entity<VotingOption>()
            .HasOne(vo => vo.VotingItem)
            .WithMany(vi => vi.VotingOptions)
            .HasForeignKey(vo => vo.VotingItemId)
            .OnDelete(DeleteBehavior.Cascade);

        // تكوين جلسة التصويت (VotingSession related to VotingItem)
        modelBuilder.Entity<VotingItem>()
            .HasOne(vi => vi.VotingSession)
            .WithMany(vs => vs.VotingItems)
            .HasForeignKey(vi => vi.VotingSessionId)
            .OnDelete(DeleteBehavior.Cascade);

        // تكوين الصوت (Vote related to VotingItem)
        modelBuilder.Entity<Vote>()
            .HasOne(v => v.VotingItem)
            .WithMany(vi => vi.Votes)
            .HasForeignKey(v => v.VotingItemId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Vote>()
            .HasOne(v => v.User)
            .WithMany(u => u.Votes)
            .HasForeignKey(v => v.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Vote>()
            .HasOne(v => v.VotingOption)
            .WithMany(vo => vo.Votes)
            .HasForeignKey(v => v.OptionId)
            .OnDelete(DeleteBehavior.SetNull);

        // تكوين Result (One-to-One with VotingItem)
        modelBuilder.Entity<VotingResult>()
            .HasOne(r => r.VotingItem)
            .WithOne(vi => vi.Result)
            .HasForeignKey<VotingResult>(r => r.VotingItemId)
            .OnDelete(DeleteBehavior.Cascade);

        // تكوين رمز التحقق
        modelBuilder.Entity<VerificationCode>()
            .HasOne(vc => vc.User)
            .WithMany(u => u.VerificationCodes)
            .HasForeignKey(vc => vc.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<VerificationCode>()
            .HasOne(vc => vc.VotingSession)
            .WithMany(vs => vs.VerificationCodes)
            .HasForeignKey(vc => vc.VotingId)
            .OnDelete(DeleteBehavior.Cascade);

        // تكوين السجل
        modelBuilder.Entity<AuditLog>()
            .HasOne(al => al.User)
            .WithMany(u => u.AuditLogs)
            .HasForeignKey(al => al.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        // تكوين إعدادات النظام
        modelBuilder.Entity<SystemSetting>()
            .HasOne(ss => ss.LastModifiedByUser)
            .WithMany(u => u.ModifiedSettings)
            .HasForeignKey(ss => ss.LastModifiedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        // تكوين الفهارس الفريدة
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.MemberId)
            .IsUnique();

        modelBuilder.Entity<Role>()
            .HasIndex(r => r.RoleName)
            .IsUnique();

        modelBuilder.Entity<SystemSetting>()
            .HasIndex(ss => ss.SettingName)
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }

}
