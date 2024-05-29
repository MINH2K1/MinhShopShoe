using ShopShoe.Application.ViewModel.Crud.Add;
using ShopShoe.Application.ViewModel.Crud.Update;
using ShopShoe.Application.ViewModel.Query;
using ShopShoe.Utilities.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopShoe.Application.Interface
{
    public interface IRoleService
    {
        Task<bool> AddAsync(AnnouncementViewModel announcement,
            List<AnnouncementUserViewModel> announcementUsers,
            AppRoleAddViewModel userVm);

        Task UpdateAsync(AppRoleUpdateViewModel userVm);

        Task DeleteAsync(Guid id);

        Task<List<AppRoleViewModel>> GetAllAsync();

        PagedResult<AppRoleViewModel> GetAllPagingAsync(string keyword, int page, int pageSize);

        Task<AppRoleViewModel> GetById(Guid id);

        List<PermissionViewModel> GetListFunctionWithRole(Guid roleId);

        void SavePermission(List<PermissionViewModel> permissions, Guid roleId);

        Task<bool> CheckPermission(string functionId, string action, string[] roles);
    }
}
