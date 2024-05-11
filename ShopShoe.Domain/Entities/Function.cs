using ShopShoe.Domain.BaseEntity;
using ShopShoe.Domain.Enums;
using ShopShoe.Domain.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopShoe.Domain.Entities
{
    [Table("Functions")]
    public class Function : DomainEntity<string>, ISwitchable, ISortable
    {

        [Required]
        [MaxLength(128)]
        public string Name { set; get; }

        [Required]
        [MaxLength(250)]
        public string URL { set; get; }


        [MaxLength(128)]
        public string ParentId { set; get; }

        public string IconCss { get; set; }
        public int SortOrder { set; get; }
        public Status Status { set; get; }
    }
}
