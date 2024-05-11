using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopShoe.Infastruction.Repository.Interface
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}
