using ShopShoe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopShoe.Domain.Abtraction;
using ShopShoe.Domain.Abtraction.Interface;

namespace ShopShoe.Domain.Entities
{
    [Table("Pages")]
    public class Page : DomainEntity<int>, ISwitchable
    {

        [Required]
        [MaxLength(256)]
        public string Name { set; get; }

        [MaxLength(256)]
        [Required]
        public string Alias { set; get; }

        public string Content { set; get; }
        public Status Status { set; get; }
    }
}
