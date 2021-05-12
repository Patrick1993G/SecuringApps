
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace SecuringApps_WebApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ShoppingCart.Application.ViewModels.AssignmentViewModel> AssignmentViewModel { get; set; }
        public DbSet<ShoppingCart.Application.ViewModels.StudentAssignmentViewModel> StudentAssignmentViewModel { get; set; }
        public DbSet<ShoppingCart.Application.ViewModels.CommentViewModel> CommentViewModel { get; set; }
    }
}
