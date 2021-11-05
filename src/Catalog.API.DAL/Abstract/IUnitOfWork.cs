﻿using System;
using System.Threading.Tasks;

namespace Catalog.API.DAL.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        public IRepository<T> Repository<T>() where T : class;
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
