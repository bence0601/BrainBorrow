using BrainBorrowAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BrainBorrowAPI.Data
{
    public class UserContext : IdentityUserContext<IdentityUser>
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
            {
            }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }


    }
}
