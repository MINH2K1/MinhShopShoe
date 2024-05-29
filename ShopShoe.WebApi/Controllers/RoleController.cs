using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR;
using ShopShoe.Application.Interface;
using ShopShoe.Application.ViewModel.Crud.Add;
using ShopShoe.Application.ViewModel.Crud.Update;
using ShopShoe.Application.ViewModel.Query;
using ShopShoe.WebApi.Extensions;
using ShopShoe.WebApi.SignalR;

namespace ShopShoe.WebApi.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IHubContext<ShopShoeHub> _hubContext;

        public RoleController(IRoleService roleService,
            IHubContext<ShopShoeHub> hubContext
            )
        {
            _roleService = roleService;
            _hubContext = hubContext;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var model = await _roleService.GetAllAsync();
            return new OkObjectResult(model);
        }

        [HttpGet("GetbyId/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var model = await _roleService.GetById(id);
            return new OkObjectResult(model);
        }

        [HttpGet("GetAllPaging/{keyword}/{page}/{pageSize}")]
        public IActionResult GetAllPaging(string keyword, int page, int pageSize)
        {
            var model = _roleService.GetAllPagingAsync(keyword, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddAsync(AppRoleAddViewModel roleVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
          
                var notificationId = Guid.NewGuid().ToString();

                var announcement = new AnnouncementViewModel()
                {
                    Title = "Role created",
                    DateCreated = DateTime.Now,
                    Content = $"Role {roleVm.Name} has been created",
                    Id = notificationId,
                    UserId = User.GetUserId()
                };
                var announcementUsers = new List<AnnouncementUserViewModel>()
                    {
                            new AnnouncementUserViewModel(){
                                AnnouncementId = notificationId,HasRead = false,
                                UserId = User.GetUserId()}
                        };
                await _roleService.AddAsync(announcement, announcementUsers, roleVm);

                await _hubContext.Clients.All.SendAsync("ReceiveMessage", announcement);
            return new OkObjectResult(roleVm);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync(AppRoleUpdateViewModel roleVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            await _roleService.UpdateAsync(roleVm);
            return new OkObjectResult(roleVm);
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            await _roleService.DeleteAsync(id);
            return new OkObjectResult(id);
        }


        [HttpPost("ListAllFunction")]
        public IActionResult ListAllFunction(Guid roleId)
        {
            var functions = _roleService.GetListFunctionWithRole(roleId);
            return new OkObjectResult(functions);
        }

        [HttpPost("SavePermission")]
        public IActionResult SavePermission(List<PermissionViewModel> listPermmission, Guid roleId)
        {
            _roleService.SavePermission(listPermmission, roleId);
            return new OkResult();
        }
    }
}
