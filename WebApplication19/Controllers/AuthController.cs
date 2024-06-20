using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication19.Services;
using System;
using System.Threading.Tasks;
using WebApplication19.Models;

namespace WebApplication19.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            var registeredUser = await _userService.Register(user.Username, user.Password);
            if (registeredUser == null)
            {
                return BadRequest("User registration failed");
            }
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            var authenticatedUser = await _userService.Authenticate(user.Username, user.Password);
            if (authenticatedUser == null)
            {
                return Unauthorized();
            }

            var accessToken = _tokenService.GenerateJwtToken(authenticatedUser);
            var refreshToken = Guid.NewGuid().ToString();
            await _userService.SaveRefreshToken(authenticatedUser.Username, refreshToken);

            return Ok(new
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }

        [HttpPost("refresh")]
        [Authorize]
        public async Task<IActionResult> Refresh([FromBody] string refreshToken)
        {
            var user = await _userService.GetByUsername(User.Identity.Name);
            if (user == null || user.RefreshToken != refreshToken)
            {
                return Unauthorized();
            }

            var newAccessToken = _tokenService.GenerateJwtToken(user);
            var newRefreshToken = Guid.NewGuid().ToString();
            await _userService.SaveRefreshToken(user.Username, newRefreshToken);

            return Ok(new
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }
    }
}
