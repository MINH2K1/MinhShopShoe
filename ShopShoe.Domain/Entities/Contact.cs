using ShopShoe.Domain.BaseEntity;
using ShopShoe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopShoe.Domain.Entities
{
    [Table("ContactDetails")]
    public class Contact : DomainEntity<string>
    {

        [MaxLength(250)]
        [Required]
        public string Name { set; get; }

        [MaxLength(50)]
        public string Phone { set; get; }

        [MaxLength(250)]
        public string Email { set; get; }

        [MaxLength(250)]
        public string Website { set; get; }

        [MaxLength(250)]
        public string Address { set; get; }

        public string Other { set; get; }

        public double? Lat { set; get; }

        public double? Lng { set; get; }

        public Status Status { set; get; }
    }
}
