﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopShoe.Domain.Entities
{
    [Table("AppRoles")]
   public class AppRole : IdentityRole<Guid>
    {
        [MaxLength(250)]
        public string? Description { get; set; }
        public   AppRole() { }
       public AppRole(string name, string de)
        {
            Name = Name;
            Description= de;
        }
       

    }
}
