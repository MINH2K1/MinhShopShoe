﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopShoe.Domain.Abtraction.Interface
{
    public class IHasSoftDelete
    {
        bool IsDeleted { set; get; }
    }
}
