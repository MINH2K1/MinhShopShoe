using ShopShoe.Application.ViewModel.Crud.Add;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopShoe.Application.ViewModel.Query
{
    public class PermissionViewModel
    {
        public int Id { get; set; }


        public Guid RoleId { get; set; }

        public string FunctionId { get; set; }

        public bool CanCreate { set; get; }

        public bool CanRead { set; get; }

        public bool CanUpdate { set; get; }

        public bool CanDelete { set; get; }

        public AppRoleAddViewModel AppRole { get; set; }

        public FunctionViewModel Function { get; set; }
    }
}
