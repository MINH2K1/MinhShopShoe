﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShopShoe.Infastruction.Repository.Interface
{
    public interface IRepository
    {
        public interface IRepository<TEntity, in TKey> where TEntity : class
        {
            TEntity FindById(TKey id, params Expression<Func<TEntity, object>>[] includeProperties);

            TEntity FindSingle(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);

            IQueryable<TEntity> FindAll(params Expression<Func<TEntity, object>>[] includeProperties);

            IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);

            void Add(TEntity entity);

            void Update(TEntity entity);

            void Remove(TEntity entity);

            void Remove(TKey id);

            void RemoveMultiple(List<TEntity> entities);
        }
    }
}
