using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopShoe.Domain.Enums
{
    public enum BillStatus
    {
        [Description("New bill")]
        New,
        [Description("In Progress")]
        InProgress,
        [Description("Returned")]
        Returned,
        [Description("Cancelled")]
        Cancelled,
        [Description("Completed")]
        Completed
    }
}
