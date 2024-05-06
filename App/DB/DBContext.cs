using App.Models.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

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

public class UserManager(UserManager<UserEntity> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<UserEntity> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<UserEntity>> logger, IAuthenticationSchemeProvider schemes, IUserConfirmation<UserEntity> confirmation) : SignInManager<UserEntity>(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
{

    public override Task SignInAsync(UserEntity user, bool isPersistent, string? authenticationMethod = null)
    {
        return base.SignInAsync(user, isPersistent, authenticationMethod);
    }

}