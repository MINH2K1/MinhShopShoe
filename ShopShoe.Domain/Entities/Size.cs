using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopShoe.Domain.Abtraction;

namespace ShopShoe.Domain.Entities
{
    [Table("Sizes")]
    public class Size : DomainEntity<int>
    {

        [MaxLength(250)]
        public string Name
        {
            get; set;
        }
    }
}
