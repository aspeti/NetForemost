using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Category> Categories { get; }
        IRepository<User> User { get; }
        IRepository<BlogEntry> BlogEntry { get; }
        Task<int> CompleteAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDBContext _context;
        public IRepository<Category> Categories { get; private set; }
        public IRepository<User> User { get; private set; }
        public IRepository<BlogEntry> BlogEntry { get; private set; }

        public UnitOfWork(AppDBContext context)
        {
            _context = context;
            Categories = new Repository<Category>(_context);
            User = new Repository<User>(_context);
            BlogEntry = new Repository<BlogEntry>(_context);
        }

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}
