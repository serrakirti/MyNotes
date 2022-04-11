﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MyNotes.CoreLayer;
using MyNotes.DataAccessLayer;

namespace MyNotes.BusinessLayer.Abstract
{
    public abstract class ManagerBase<T> :IRepository<T> where T:class
    {
        private Repository<T> repo = new Repository<T>();
        
        public List<T> List()
        {
            return repo.List();
        }

        public List<T> List(Expression<Func<T, bool>> predicate)
        {
            return repo.List(predicate);
        }

        public IQueryable<T> QList()
        {
            return repo.QList();
        }

        public int Insert(T entity)
        {
            return repo.Insert(entity);
        }

        public int Update(T entity)
        {
            return repo.Update(entity);
        }

        public int Delete(T entity)
        {
            return repo.Delete(entity);
        }

        public int Save()
        {
            return repo.Save();
        }

        public T Find(Expression<Func<T, bool>> predicate)
        {
            return repo.Find(predicate);
        }
    }
}
