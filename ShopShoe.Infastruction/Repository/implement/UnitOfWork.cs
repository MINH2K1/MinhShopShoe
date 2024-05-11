using ShopShoe.Infastruction.DataEf;
using ShopShoe.Infastruction.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopShoe.Infastruction.Repository.implement
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ShopShoeDbContext _dbContext;

        public UnitOfWork(ShopShoeDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Commit()
        {
            _dbContext.SaveChanges();
        }

    }
}
