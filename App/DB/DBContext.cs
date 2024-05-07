using App.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace App.DB;

public class DBContext : IdentityDbContext<UserEntity, IdentityRole<Guid>, Guid>
{
    public DbSet<VideoEntity> Videos { get; set; } = null!;
    public DbSet<CommentEntity> Comments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<CommentEntity>().HasOne(c => c.User).WithMany(u => u.Comments).HasForeignKey(c => c.UserId);
        modelBuilder.Entity<CommentEntity>().HasOne(c => c.Video).WithMany(u => u.Comments).HasForeignKey(c => c.VideoId);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlite("Data Source=db.db");
    }
}
