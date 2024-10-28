using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.DTO;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;


namespace BlogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private IConfiguration config;

        public AuthenticationController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            config = configuration;
        }

        /// <summary>
        /// Gets list of users
        /// </summary>       
        [HttpPost("authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> login([FromBody] UserRequest userRequest)
        {
            try
            {
                var user = await _unitOfWork.User.GetAllAsync(e => e.UsrEmail == userRequest.email && e.UsrPassword == userRequest.password);
                if (user == null) return BadRequest(new { message = "Invalid Credentials" });

                string jwtToken = GenerateToken(user.First());

                return Ok(new { token = jwtToken });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        private string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UsrFirstName),
                 new Claim(ClaimTypes.Email, user.UsrEmail)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("JWT:Key").Value));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var SecurityToken = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: credentials
                );
            string Token = new JwtSecurityTokenHandler().WriteToken(SecurityToken);
            return Token;
        }
    }
}
