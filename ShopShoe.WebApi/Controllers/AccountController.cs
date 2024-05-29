using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using ShopShoe.Application.Interface;
using ShopShoe.Domain.Entities;
using ShopShoe.WebApi.Model;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Azure.Core;
using ShopShoe.Application.Implement;

namespace ShopShoe.WebApi.Controllers
{
    [ApiController]
    public class AccountController:Controller
    {
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userInManager;

        public AccountController(ITokenService tokenService,
            SignInManager<AppUser> signInManager,
            UserManager<AppUser> userInManager)
        {
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userInManager = userInManager;
        }

        [HttpPost, Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
        {
            if (loginViewModel is null)
            {
                return BadRequest("Invalid client request");
            }
            // login
            var user = await _signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, loginViewModel.Remember, false);
           
            if (!user.Succeeded)
                return Unauthorized();

            // Packed user in to claim
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, loginViewModel.UserName),
            new Claim(ClaimTypes.Role, "Manager")
        };
            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var result = _userInManager.FindByNameAsync(loginViewModel.UserName);
            // update refresh token into database
            result.Result.RefreshToken = refreshToken;

            await _userInManager.UpdateAsync(result.Result);
            //
            return Ok(
          new  {
                Token = accessToken,
                RefreshToken = refreshToken
            });
       
        }
        [HttpGet("Logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        /// <summary>
        /// login with google
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet("google-register")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin()
        {
            Console.WriteLine("xxxx");
            // Chuyển hướng đến nhà cung cấp dịch vụ xác thực
            var properties = new AuthenticationProperties { RedirectUri = Url.Action(nameof(GoogleResponse) )};
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

   
        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse(string returnUrl="/")
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (authenticateResult.Succeeded)
            {
                var emailClaim = authenticateResult.Principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var nameClaim = authenticateResult.Principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                var existingUser = await _userInManager.FindByEmailAsync(emailClaim);
                var jwt = _tokenService.GenerateAccessToken(authenticateResult.Principal.Claims);
                if (existingUser != null)
                {
                    var refreshToken = _tokenService.GenerateRefreshToken();
                    existingUser.RefreshToken = refreshToken;

                    // User already exists, sign in the user
                    await _signInManager.SignInAsync(existingUser, isPersistent: false);
                    await _userInManager.UpdateAsync(existingUser);
                    return Ok(new
                    {
                        token = jwt,
                        freshToken = refreshToken
                    });
                }
                else
                {
                    var refreshToken = _tokenService.GenerateRefreshToken();
                    // User does not exist, create a new account
                    var newUser = new AppUser { UserName = nameClaim, Email = emailClaim,RefreshToken= refreshToken };
                    var result = await _userInManager.CreateAsync(newUser);

                    if (result.Succeeded)
                    {
                        // Sign in the newly created user
                        await _signInManager.SignInAsync(newUser, isPersistent: false);
                        return Ok(new
                        {
                            token = jwt,
                            freshToken = refreshToken
                        });
                    }
                    else
                    {
                        // Failed to create a new user
                        return BadRequest("Failed to create a new user.");
                    }
                }
            }
            else
            {
                // Authentication failed
                return BadRequest("Authentication failed.");
            }
        }
    }       
}
