using arTWander.Models;
using Microsoft.EntityFrameworkCore;

namespace arTWander.Data
{
    public static class SeedDataExtentions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    RoleId = 1,
                    Name = "admin",
                    Active = true
                },
                new Role
                {
                    RoleId = 2,
                    Name = "common",
                    Active = true
                });

            modelBuilder.Entity<User>().HasData(
                 new User
                 {
                     UserId = 1,
                     Email = "a975409@gmail.com",
                     Password = "F5297FBE0319A5C2B3DF2FEC7176D688",
                     RoleId = 1
                 }
                );
        }
    }
}
