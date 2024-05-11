﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopShoe.Domain.Interface
{
    public interface IHasOwner<T>
    {
        T OwnerId { set; get; }
    }
}
