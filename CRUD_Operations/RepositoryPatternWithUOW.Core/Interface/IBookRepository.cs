using CRUD_Operations.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.Interface
{
    public interface IProductRepository : IBaseRepository<Product>
    {
         IEnumerable<Product> SpecialMethod();
    }
}
