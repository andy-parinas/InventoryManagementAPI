using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using InventoryManagementAPI.Data;
using InventoryManagementAPI.Dto;
using InventoryManagementAPI.Helpers;
using InventoryManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace InventoryManagementAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IAuthRepository _authRepo;
        private readonly IConfiguration _config; 

        public AuthController(IAuthRepository authRepo, IConfiguration config)
        {
            _authRepo = authRepo;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto register, 
            [FromQuery] RegisterParams registerParams)
        {
            if (await _authRepo.UserExist(register.Username))
                ModelState.AddModelError("Username", "Username Already Exist");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            User newUser = new User
            {
                Username = register.Username,
                FirstName = register.FirstName,
                LastName = register.LastName,
                Phone = register.Phone,
                Email = register.Email
            };

            User createdUser = await _authRepo.Register(newUser, register.Password);

            return StatusCode(201);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto login)
        {
            User user = await _authRepo.Login(login.Username, login.Password);

            if (user == null)
                return Unauthorized();

            //GENERATE TOKEN
            var key = Encoding.ASCII.GetBytes(_config.GetSection("AppSettings:Token").Value);
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                                            SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            var authUser = new
            {
                id = user.Id,
                name = user.FirstName
            };

            return Ok(new
            {
                user = authUser,
                token = tokenString
            });
        }
    }
}