using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using REACTBIGBANG.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace REACTBIGBANG.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly HospitalContext _context;

        private const string DoctorsRole = "Doctor";
        private const string PatientsRole = "Patients";
        private const string AdminRole = "Admin";
        public TokenController(IConfiguration config, HospitalContext context)
        {
            _configuration = config;
            _context = context;
        }

        [HttpPost("Doctor")]
        public async Task<IActionResult> Post(Doctor _userData)
        {
            if (_userData != null && _userData.Doctor_name != null && _userData.Doctor_Password != null)
            {
                var user = await GetUser(_userData.Doctor_name, _userData.Doctor_Password);

                if (user != null)
                {
                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("Doctor_id", user.Doctor_id.ToString()),
                        new Claim("Doctor_name", user.Doctor_name),
                        new Claim("Doctor_Password",user.Doctor_Password),
                        new Claim(ClaimTypes.Role, DoctorsRole)

                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:ValidIssuer"],
                        _configuration["Jwt:ValidAudience"],
                        claims,
                        expires: DateTime.UtcNow.AddDays(1),
                        signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        private async Task<Doctor> GetUser(string name, string password)
        {
            return await _context.doctors.FirstOrDefaultAsync(x => x.Doctor_name == name && x.Doctor_Password == password);
               
        }

        [HttpPost("Patients")]
        public async Task<IActionResult> Post(Patient _userData)
        {
            if (_userData != null && _userData.Patient_name != null && _userData.Patient_Password != null)
            {
                var user = await GetUsers(_userData.Patient_name, _userData.Patient_Password);

                if (user != null)
                {
                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("Patient_id", user.Patient_id.ToString()),
                        new Claim("Patient_name", user.Patient_name),
                        new Claim("Patient_Password",user.Patient_Password),
                        new Claim(ClaimTypes.Role, PatientsRole)

                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:ValidIssuer"],
                        _configuration["Jwt:ValidAudience"],
                        claims,
                        expires: DateTime.UtcNow.AddDays(1),
                        signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        private async Task<Patient> GetUsers(string name, string password)
        {
            return await _context.patients.FirstOrDefaultAsync(x => x.Patient_name == name && x.Patient_Password == password);

        }
        [HttpPost("Admin")]
        public async Task<IActionResult> PostStaff(Admin staffData)
        {
            if (staffData != null && !string.IsNullOrEmpty(staffData.Admin_name) && !string.IsNullOrEmpty(staffData.Admin_password))
            {
                if (staffData.Admin_name == "Pavithra" && staffData.Admin_password == "Pavi@123")
                {
                    var claims = new[]
                    {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("AdminId", "1"), // Set the admin ID accordingly
                new Claim("Admin_name", staffData.Admin_name),
                new Claim("Admin_password", staffData.Admin_password),
                new Claim(ClaimTypes.Role, AdminRole)
            };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:ValidIssuer"],
                        _configuration["Jwt:ValidAudience"],
                        claims,
                        expires: DateTime.UtcNow.AddDays(1),
                        signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }


        private async Task<Admin> GetStaff(string adminName, string adminPassword)
        {
            return await _context.admins.FirstOrDefaultAsync(s => s.Admin_name == adminName && s.Admin_password == adminPassword);
        }
    }
}
