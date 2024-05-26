using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopShoe.Application.Interface;
using ShopShoe.Application.ViewModel.Query;
using ShopShoe.Domain.Entities;

namespace ShopShoe.WebApi.Controllers
{
    [ApiController]
    public class TokenController:Controller
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        public TokenController(ITokenService tokenService, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
             _tokenService = tokenService;
        }
        [HttpPost("RefreshToken")]
        
        public IActionResult freshToken(TokenApiViewModel tokenApiViewModel)
        {
            if (tokenApiViewModel is null)
                return BadRequest();

            string accessToken = tokenApiViewModel.AccessToken;
            string refreshToken = tokenApiViewModel.RefreshToken;

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);

            // get Name from pricible
            var username = principal.Identity.Name;//this is mapped to the Name claim by default

            //Get UserName from database 
            var user = _userManager.FindByNameAsync(username);
            // check freshtoken
            if (user is null || user.Result.RefreshToken != refreshToken || user.Result.RefreshTokenExpiryTime <= DateTime.Now)
                return BadRequest("Invalid client request");
            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            user.Result.RefreshToken = newRefreshToken;
            _userManager.UpdateAsync(user.Result);

            return Ok(new 
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }
        [HttpPost, Authorize]
        [Route("revoke")]
        public IActionResult Revoke()
        {
            var username = User.Identity.Name;
            var user = _userManager.FindByNameAsync(username);
            if (user == null) return BadRequest();
              else  user.Result.RefreshToken = null;
            _userManager.UpdateAsync(user.Result);
            return NoContent();
        }
    }
}
