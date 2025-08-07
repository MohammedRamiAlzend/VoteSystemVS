using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Domain.Entities;

namespace Infrastructure.Data
{
    public static class DbSeeder
    {
        public static void SeedAdmins(AppDbContext context)
        {
            // Example admins
            var admins = new[]
            {
                new { UserName = "rami", Password = "123" },
                new { UserName = "admin1", Password = "admin1pass" },
                new { UserName = "admin2", Password = "admin2pass" }
            };

            foreach (var admin in admins)
            {
                if (!context.Admins.Any(a => a.UserName == admin.UserName))
                {
                    context.Admins.Add(new Admin
                    {
                        UserName = admin.UserName,
                        HashedPassword = HashPassword(admin.Password)
                    });
                }
            }
            context.SaveChanges();
        }

        private static string HashPassword(string password)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
