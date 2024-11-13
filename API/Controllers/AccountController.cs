using System.Security.Claims;
using System.Text.Json;
using API.Dtos;
using API.Services;
using Application.Core;
using Application.Users.Queries.GetUserDetails;
using Application.Users.Queries.GetUsers;
using Domain.Entities;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly TokenService _tokenService;
        private IMediator _mediator;
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;
        protected IMediator Mediator =>
            _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        protected ActionResult HandleResult<T>(Result<T> result)
        {
            if (result == null)
                return NotFound();
            if (result.IsSuccess && result.Value != null)
                return Ok(result.Value);
            if (result.IsSuccess && result.Value == null)
                return NotFound();
            return BadRequest(result.Error);
        }

        public AccountController(
            UserManager<User> userManager,
            TokenService tokenService,
            IConfiguration config
        )
        {
            _config = config;
            _tokenService = tokenService;
            _userManager = userManager;
            _httpClient = new HttpClient
            {
                BaseAddress = new System.Uri("https://graph.facebook.com/")
            };
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
                return Unauthorized();

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return BadRequest("Invalid email or password.");
            }

            return await CreateUserObject(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.Username))
            {
                return BadRequest("Username is already taken");
            }

            if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
            {
                return BadRequest("Email is already taken");
            }

            var user = new User
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Username,
                Contact = registerDto.Contact,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, registerDto.Role);
                return await CreateUserObject(user);
            }

            return BadRequest(result.Errors);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));

            return await CreateUserObject(user);
        }

        private async Task<ActionResult<UserDto>> CreateUserObject(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();

            var userDto = new UserDto
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Token = await _tokenService.CreateToken(user),
                Username = user.UserName,
                Role = role
            };

            return Ok(userDto);
        }

        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUser()
        {
            return HandleResult(await Mediator.Send(new GetUsersQuery.Query()));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllUserById(string id)
        {
            return HandleResult(await Mediator.Send(new GetUserDetailsQuery.Query { Id = id }));
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotDto forgotDto)
        {
            var user = await _userManager.FindByEmailAsync(forgotDto.Email);
            if (user == null)
            {
                return Ok(
                    new
                    {
                        message = "If a matching account exists, an email will be sent with instructions to reset your password."
                    }
                );
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetUrl = Url.Action(
                "ResetPassword",
                "Account",
                new { token, email = user.Email },
                Request.Scheme
            );

            var emailService = new EmailServices(_config);
            await emailService.SendPasswordResetEmail(user.Email, resetUrl);

            return Ok(user.Email);
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
            {
                return BadRequest("Invalid request.");
            }

            var result = await _userManager.ResetPasswordAsync(
                user,
                resetPasswordDto.Token,
                resetPasswordDto.NewPassword
            );
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { message = "Password has been reset successfully." });
        }

        [AllowAnonymous]
        [HttpPost("fb-login")]
        public async Task<ActionResult<UserDto>> FacebookLogin(string accessToken)
        {
            var fbVerifyKeys = _config["Facebook:AppId"] + "|" + _config["Facebook:ApiSecret"];

            var verifyTokenResponse = await _httpClient.GetAsync(
                $"debug_token?input_token={accessToken}&access_token={fbVerifyKeys}"
            );

            if (!verifyTokenResponse.IsSuccessStatusCode)
                return Unauthorized();

            var fbUrl = $"me?access_token={accessToken}&fields=name,email";

            var fbInfo = await _httpClient.GetFromJsonAsync<FacebookDto>(fbUrl);

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == fbInfo.Email);

            if (user != null)
                return await CreateUserObject(user);

            user = new User
            {
                DisplayName = fbInfo.Name,
                Email = fbInfo.Email,
                UserName = fbInfo.Email,
            };

            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
                return BadRequest("Problem creating user account.");

            await _userManager.AddToRoleAsync(user, "User");

            return await CreateUserObject(user);
        }

        [AllowAnonymous]
        [HttpPost("google-callback")]
        public async Task<ActionResult<UserDto>> GoogleCallback(GoogleDto loginDto)
        {
            var googleToken = loginDto.Token;

            var validPayload = await GoogleJsonWebSignature.ValidateAsync(googleToken);
            if (validPayload == null)
            {
                return BadRequest("Invalid Google token.");
            }

            var email = validPayload.Email;

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new User
                {
                    Email = email,
                    UserName = email,
                    DisplayName = validPayload.Name,
                };

                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    return BadRequest("User creation failed.");
                }

                await _userManager.AddToRoleAsync(user, "User");
            }

            return await CreateUserObject(user);
        }
    }
}
