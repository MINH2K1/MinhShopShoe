using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopShoe.Application.Interface;
using ShopShoe.Application.ViewModel.Crud.Add;
using ShopShoe.Application.ViewModel.Crud.Update;
using ShopShoe.Application.ViewModel.Query;
using ShopShoe.Domain.Entities;
using ShopShoe.Infastruction.Repository.Interface;
using ShopShoe.Utilities.Dto;
using static ShopShoe.Infastruction.Repository.Interface.IRepository;

namespace ShopShoe.Application.Implement
{
    public class RoleService : IRoleService
    {
        private RoleManager<AppRole> _roleManager;
        private IRepository<AnnouncementUser, int> _announUserRepository;
        private IRepository<Announcement, Guid> _announRepository;
        private IMapper _mapper;
        private IUnitOfWork _unitOfWork;
        private IRepository<Function, string> _functionRepository;
        private IRepository<Permission, int> _permissionRepository;

        public RoleService(
         RoleManager<AppRole> roleManager,
         IRepository<AnnouncementUser, int> announUserRepository,
         IRepository<Announcement, Guid> announRepository,
         IMapper mapper,
         IUnitOfWork unitOfWork,
         IRepository<Function, string> functionRepository,
         IRepository<Permission, int> permissionRepository
         )
        {
            _roleManager = roleManager;
            _announUserRepository = announUserRepository;
            _announRepository = announRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _functionRepository = functionRepository;
            _permissionRepository = permissionRepository;

        }

        public async Task<bool> AddAsync(AnnouncementViewModel announcementVm,
            List<AnnouncementUserViewModel> announcementUsers, AppRoleAddViewModel roleVm)
        {
            var role = new AppRole()
            {
                Name = roleVm.Name,
                Description = roleVm.Description
            };
            var result = await _roleManager.CreateAsync(role);
            var announcement = _mapper.Map<AnnouncementViewModel,
                                  Announcement>(announcementVm);
            _announRepository.Add(announcement);
            foreach (var userVm in announcementUsers)
            {
                var user = _mapper.Map<AnnouncementUserViewModel,
                                    AnnouncementUser>(userVm);
                _announUserRepository.Add(user);
            }
            _unitOfWork.Commit();
            return result.Succeeded;

        }

        public async Task UpdateAsync(AppRoleUpdateViewModel roleVm)
        {
            var role = await _roleManager.FindByIdAsync(roleVm.Id.ToString());
            role.Description = roleVm.Description;
            role.Name = roleVm.Name;
            await _roleManager.UpdateAsync(role);
        }

        public Task<bool> CheckPermission(string functionId, string action, string[] roles)
        {
            var functions = _functionRepository.FindAll();
            var permissions = _permissionRepository.FindAll();
            var query = from f in functions
                        join p in permissions on f.Id equals p.FunctionId
                        join r in _roleManager.Roles on p.RoleId equals r.Id
                        where roles.Contains(r.Name) && f.Id == functionId
                        && ((p.CanCreate && action == "Create")
                        || (p.CanUpdate && action == "Update")
                        || (p.CanDelete && action == "Delete")
                        || (p.CanRead && action == "Read"))
                        select p;
            return query.AnyAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
                var role = await _roleManager.FindByIdAsync(id.ToString());
                await _roleManager.DeleteAsync(role);
        }

        public async Task<List<AppRoleViewModel>> GetAllAsync()
        {
             return await _mapper.ProjectTo<AppRoleViewModel>(_roleManager.Roles).ToListAsync();
        }

        public PagedResult<AppRoleViewModel> GetAllPagingAsync(string keyword, int page, int pageSize)
        {
                var query = _roleManager.Roles;
                if (!string.IsNullOrEmpty(keyword))
                    query = query.Where(x => x.Name.Contains(keyword)
                    || x.Description.Contains(keyword));

                int totalRow = query.Count();
                query = query.Skip((page - 1) * pageSize)
                   .Take(pageSize);

                var data = _mapper.ProjectTo<AppRoleViewModel>(query).ToList();
                var paginationSet = new PagedResult<AppRoleViewModel>()
                {
                    Results = data,
                    CurrentPage = page,
                    RowCount = totalRow,
                    PageSize = pageSize
                };
                return paginationSet;
        }

        public async Task<AppRoleViewModel> GetById(Guid id)
        {
                var role = await _roleManager.FindByIdAsync(id.ToString());
                return _mapper.Map<AppRole, AppRoleViewModel>(role);
        }

        public List<PermissionViewModel> GetListFunctionWithRole(Guid roleId)
        {    
                var functions = _functionRepository.FindAll();
                var permissions = _permissionRepository.FindAll();

                var query = from f in functions
                            join p in permissions on f.Id equals p.FunctionId into fp
                            from p in fp.DefaultIfEmpty()
                            where p != null && p.RoleId == roleId
                            select new PermissionViewModel()
                            {
                                RoleId = roleId,
                                FunctionId = f.Id,
                                CanCreate = p != null ? p.CanCreate : false,
                                CanDelete = p != null ? p.CanDelete : false,
                                CanRead = p != null ? p.CanRead : false,
                                CanUpdate = p != null ? p.CanUpdate : false
                            };
                return query.ToList();
        }

        public void SavePermission(List<PermissionViewModel> permissionVms, Guid roleId)
        {
                var permissions = _mapper.Map<List<PermissionViewModel>, List<Permission>>(permissionVms);
                var oldPermission = _permissionRepository.FindAll().Where(x => x.RoleId == roleId).ToList();
                if (oldPermission.Count > 0)
                {
                    _permissionRepository.RemoveMultiple(oldPermission);
                }
                foreach (var permission in permissions)
                {
                    _permissionRepository.Add(permission);
                }
                _unitOfWork.Commit();
            }
          
        }
    }

