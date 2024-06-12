using Medical.Data.Entities;
using Medical.Helper;
using Medical.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace Medical.Data
{
    public static class Seeder
    {
        public static async Task Seed(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager, IConfiguration configuration)
        {
            if (userManager.Users.Any())
            {
                return;
            }

            // Seed users
            var users = new[]
            {
                new { UserName = "admin", Email = "admin@example.com", Name = "Admin User", NationalId = "11111111111111", PhoneNumber = "1234567890", Role = UserRole.Admin,Password = "Admin123!" },
                new { UserName = "doctor", Email = "doctor@example.com", Name = "Doctor User", NationalId = "22222222222222", PhoneNumber = "1234567891", Role = UserRole.Doctor, Password = "Doctor123!" },
                new { UserName = "patient", Email = "patient@example.com", Name = "Patient User", NationalId = "33333333333333", PhoneNumber = "1234567892", Role = UserRole.Patient, Password = "Patient123!" }
            };

            foreach (var userInfo in users)
            {
                if (await userManager.FindByNameAsync(userInfo.UserName) == null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = userInfo.UserName,
                        Email = userInfo.Email,
                        Name = userInfo.Name,
                        NationalId = userInfo.NationalId,
                        PhoneNumber = userInfo.PhoneNumber,
                        UserRole = userInfo.Role,
                    };

                    var result = await userManager.CreateAsync(user, userInfo.Password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, userInfo.Role.ToString());

                        // Generate and save QR code
                        var qrCodeImageUrl = GenerateQrCodeImage(user.Id, configuration["baseUrl"]);
                        var path = configuration["images:upload"];
                        user.QR = DocumentSettings.SaveFile(user.Name, qrCodeImageUrl, path);
                        await userManager.UpdateAsync(user);
                    }
                }
            }
        }

        private static byte[] GenerateQrCodeImage(int userId, string baseUrl)
        {
            // Generate QR code
            string url = $"{baseUrl}/Account/Profile/{userId}";
            using QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            using BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData);
            byte[] qrCodeImageBytes = qrCode.GetGraphic(20);

            // Return the byte array of the QR code image
            return qrCodeImageBytes;
        }


    }
}
