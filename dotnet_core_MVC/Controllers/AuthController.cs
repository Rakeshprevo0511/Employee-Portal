using dotnet_core_MVC.Data;
using dotnet_core_MVC.DTO;
using dotnet_core_MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;

namespace dotnet_core_MVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly TokenService _tokenService;
        public AuthController(ApplicationDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _context.Employee
                .FirstOrDefaultAsync(e => e.Username == model.Username && e.Password == model.Password);

            if (user == null)
                return Unauthorized(new { message = "Invalid username or password" });

            // Step 1: Generate OTP
            string otp = new Random().Next(100000, 999999).ToString(); // 6-digit OTP
            var expiryTime = DateTime.UtcNow.AddMinutes(5); // OTP expires in 5 minutes
            var existingOtps = await _context.OtpRecords
            .Where(o => o.EmpId == user.Id && !o.IsUsed)
            .ToListAsync();

            foreach (var otpu in existingOtps)
            {
                otpu.IsUsed = true;
            }

            await _context.SaveChangesAsync();

            // Step 2: Save OTP to OtpRecord Table
            var otpRecord = new OtpRecord
            {
                EmpId = user.Id,
                Email = model.Username, // Assuming username is email
                OTP = otp,
                ExpiryTime = expiryTime,
                IsUsed = false
            };

            _context.OtpRecords.Add(otpRecord);
            await _context.SaveChangesAsync();

            // Step 3: Send OTP via Email
            SendOtpEmail("rakeshnagpure71@gmail.com", otp);

            return Ok(new { message = "OTP sent successfully. Please verify.", empId = user.Id });
        }
        private void SendOtpEmail(string email, string otp)
        {
            try
            {
                var mail = new MailMessage();
                mail.From = new MailAddress("rakeshnagpure71@gmail.com");
                mail.To.Add(email);
                mail.Subject = "Your OTP Code";
                mail.Body = $"Your OTP Code is: {otp}";

                var smtp = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new System.Net.NetworkCredential("rakeshnagpure71@gmail.com", "qedi okrr fman kzqd"),
                    EnableSsl = true
                };

                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email sending failed: {ex.Message}");
            }
        }
        [AllowAnonymous]
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpModel model)
        {
            var otpRecord = await _context.OtpRecords
                .Where(o => o.EmpId == model.EmpId && o.OTP == model.OTP && !o.IsUsed && o.ExpiryTime > DateTime.UtcNow)
                .FirstOrDefaultAsync();

            if (otpRecord == null)
                return Unauthorized(new { message = "Invalid or expired OTP" });

            // Mark OTP as used
            otpRecord.IsUsed = true;
            await _context.SaveChangesAsync();

            // Generate JWT Token
            var user = await _context.Employee.FindAsync(model.EmpId);
            var token = _tokenService.GenerateJwtToken(user.EmpName);

            return Ok(new { token, user });
        }
    }


}
