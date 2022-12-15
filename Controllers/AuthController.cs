using JwtWebApiTutorial.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Songify;
using Songify.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace JwtWebApiTutorial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _context;
        public static User user = new User();
        public static List<User> users = new List<User>();
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public AuthController(IConfiguration configuration, IUserService userService, DataContext context)
        {
            _context = context;
            _configuration = configuration;
            _userService = userService;
        }

    //    [HttpPost]
    //    public async Task<ActionResult<List<User>>> AddCharacter(User character)
    //    {
    //        _context.User.Add(character);
    //        await _context.SaveChangesAsync();

    //        return Ok(await _context.User.ToListAsync());
    //    }

    //    [HttpGet]
    //    public async Task<ActionResult<List<User>>> GetAllCharacters()
    //    {
    //        return Ok(await _context.RpgCharacters.ToListAsync());
    //    }

    //    [HttpGet("{id}")]
    //    public async Task<ActionResult<User>> GetCharacter(int id)
    //    {
    //        var character = await _context.RpgCharacters.FindAsync(id);
    //        if (character == null)
    //        {
    //            return BadRequest("Character not found.");
    //        }
    //        return Ok(character);
    //    }
    //}

    [HttpGet, Authorize]
        public ActionResult<string> GetMe()
        {
            var userName = _userService.GetMyName();
            return Ok(userName);
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.Username = request.Username;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            users.Add(user);
            _context.user.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            var validUser = false;
            {
                validUser = _context.user.Any(user => user.Username == request.Username);
            }
            if (!validUser)
            {
                return BadRequest("User not found or wrong password.");
            }
            VerifyPasswordHash(request.Password, users[0].PasswordHash, users[0].PasswordSalt);
            //FIX THIS PLEASE
            //var validPassword = false;
            //bool checkPassword = false;
            //{
            //    validPassword = _context.user.Any(user => user.checkPassword == VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt));
            //}
            //if (!validPassword)
            //{
            //    return BadRequest("Wrong password.");
            //}
            //foreach (User user in users) 
            //{
            //    if (user.Username == request.Username && VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            //    {

            //        break;
            //    }
            //}

            //if (checkUsername)
            //{
            //    return BadRequest("User not found.");
            //}

            //if (checkPassword)
            //{
            //    return BadRequest("Wrong password.");
            //}

            string token = CreateToken(user);

            var refreshToken = GenerateRefreshToken();
            SetRefreshToken(refreshToken);

            return Ok(token);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (!user.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token.");
            }
            else if(user.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token expired.");
            }

            string token = CreateToken(user);
            var newRefreshToken = GenerateRefreshToken();
            SetRefreshToken(newRefreshToken);

            return Ok(token);
        }

        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };

            return refreshToken;
        }

        private void SetRefreshToken(RefreshToken newRefreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                Console.WriteLine(computedHash.SequenceEqual(passwordHash));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
