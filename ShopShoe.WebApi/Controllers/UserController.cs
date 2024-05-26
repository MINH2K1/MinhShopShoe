using Microsoft.AspNetCore.Mvc;
using ShopShoe.Application.Implement;
using ShopShoe.Application.Interface;
using ShopShoe.Application.ViewModel.User;
using ShopShoe.Utilities.Dto;

namespace ShopShoe.WebApi.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;

        }
        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var User = await _userService.GetById(id);
            return Ok(User);
        }
        [HttpGet("getall")]
        public async Task<IActionResult> Index()
        {
            var listUser = await _userService.GetAllAsync();
            return Ok(listUser);
        }
        [HttpPost("add")]
        public async Task<IActionResult> Add(AppUserViewModel userViewModel)
        {
            var user = await _userService.AddAsync(userViewModel);
            return Created();
        }
        [HttpDelete("delete-{id}")]
          public  async Task<IActionResult> DeleteAsync(string id)
        {
          await  _userService.DeleteAsync(id);
            return NoContent();
        }
        [HttpGet("GetAllPaging/{keyword}/{page}/{pageSize}")]
        public  PagedResult<AppUserViewModel> GetAllPagingAsync(string keyword, int page, int pageSize)
        {
           var listUser= _userService.GetAllPagingAsync(keyword, page, pageSize);
            return listUser;
        }

    }
}
