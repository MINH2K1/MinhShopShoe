﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopShoe.Domain.Abtraction;

namespace ShopShoe.Domain.Entities
{
    [Table("Permissions")]
    public class Permission : DomainEntity<int>
    {

        [Required]
        public Guid RoleId { get; set; }


        public bool CanCreate { set; get; }
        public bool CanRead { set; get; }

        public bool CanUpdate { set; get; }
        public bool CanDelete { set; get; }




        [ForeignKey("RoleId")]
        public virtual AppRole AppRole { get; set; }




        [MaxLength(128)]
        [Required]
        public string FunctionId { get; set; }

     
        public virtual Function Function { get; set; }
    }
}
