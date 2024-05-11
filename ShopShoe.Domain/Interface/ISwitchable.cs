using ShopShoe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopShoe.Domain.Interface
{
    public interface ISwitchable
    {
        Status Status { set; get; }
    }
}
