using CRUD_Operations.Data;
using RepositoryPatternWithUOW.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core
{
    public interface IUnitOfWork : IDisposable  
    {
        //IBaseRepository<Product> Products { get; }
        IProductRepository Products { get; }
        int Complete();
    }
}
