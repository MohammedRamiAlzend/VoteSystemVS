using Application.Features.Auth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Auth
{
    public class OtpService : IOtpService
    {
        public Task<string> GenerateOtpAsync(string userId)
        {
            // Generate a 6-digit OTP
            Random random = new Random();
            string otp = random.Next(100000, 999999).ToString();

            // In a real application, you would save this OTP to a database
            // along with the userId and an expiry time.
            // For this example, we'll just return it.

            return Task.FromResult(otp);
        }

        public Task<bool> ValidateOtpAsync(string userId, string otp)
        {
            // In a real application, you would retrieve the stored OTP for the userId
            // and compare it with the provided OTP, also checking for expiry.
            // For this example, we'll just assume it's valid if it's not empty.

            return Task.FromResult(!string.IsNullOrEmpty(otp));
        }
    }

}
