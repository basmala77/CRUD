using CRUD_Operations.Data;
using Microsoft.EntityFrameworkCore;
using RepositoryPatternWithUOW.Core;
using RepositoryPatternWithUOW.Core.Conts;
using RepositoryPatternWithUOW.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.EF.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected ApplicationDbContext _context;
        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<PaginatedResult<Product>> GetProductsAsync(
       int pageNumber, int pageSize, string sortBy = null, string filter = null)
        {
            // Base query
            var query = _context.Products.AsQueryable();

            // Apply filtering
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(p => p.Name.Contains(filter) || p.Description.Contains(filter));
            }

            // Apply sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                query = sortBy switch
                {
                    "name" => query.OrderBy(p => p.Name),
                    "price" => query.OrderBy(p => p.Price),
                    "quantity" => query.OrderBy(p => p.Quantity),
                    _ => query.OrderBy(p => p.Id) // Default sort
                };
            }

            // Get total count for pagination
            var totalCount = await query.CountAsync();

            // Apply pagination
            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<Product>
            {
                Data = data,
                TotalCount = totalCount,
                PageSize = pageSize,
                PageNumber = pageNumber
            };
        }

        public void Attach(T entity)
        {
            _context.Set<T>().Attach(entity);
        }

        public int Count()
        {
           return _context.Set<T>().Count();    
        }

        public int Count(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Count(expression);
        }

        public T Create(T entity)
        {
            _context.Set<T>().Add(entity);
            return entity;
        }

        public int Delete(T entity , int id)
        {
            _context.Remove(entity);
            return id;
        }

        public T Find(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().SingleOrDefault(expression);
        }

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> expression, int? take, int? skip, Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending)
        {
            IQueryable<T> query = _context.Set<T>();
            query.Where(expression);

            if(take.HasValue)
                query=query.Take(take.Value); 
            if(skip.HasValue)
                query=query.Skip(skip.Value);
            if(orderBy != null) 
            {
                if(orderByDirection == OrderBy.Ascending) query=query.OrderBy(orderBy);
                else query=query.OrderByDescending(orderBy);
            }
            return query.ToList();
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }
        public IEnumerable<T> GetValues()
        { return _context.Set<T>().ToList(); }

        public T Update(T entity)
        {
           _context.Set<T>().Update(entity);
            return entity;
        }
    }
}
