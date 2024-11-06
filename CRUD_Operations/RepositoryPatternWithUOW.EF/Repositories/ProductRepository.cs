using CRUD_Operations.Data;
using RepositoryPatternWithUOW.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.EF.Repositories
{
    public class ProductRepository : BaseRepository<Product>,IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context) : base(context) { }

        public IEnumerable<Product> SpecialMethod()
        {
            throw new NotImplementedException();
        }
    }
}
