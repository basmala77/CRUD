using CRUD_Operations.Data;
using RepositoryPatternWithUOW.Core.Conts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.Interface
{
    public interface IBaseRepository <T> where T : class
    {
        T GetById(int id);
        IEnumerable<T> GetValues();
        T Find(Expression<Func<T, bool>> expression);
        IEnumerable<T> FindAll(Expression<Func<T, bool>> expression,int? take,int?
            skip, Expression<Func<T, object>> orderBy = null,string orderByDirection = OrderBy.Ascending);

        T Create(T entity);
        T Update(T entity);
        int Delete(T entity,int id);
        void Attach(T entity);
        int Count();
        int Count(Expression<Func<T, bool>> expression);
        Task<PaginatedResult<Product>> GetProductsAsync(
        int pageNumber, int pageSize, string sortBy = null, string filter = null);
    }
    public class PaginatedResult<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
