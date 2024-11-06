using CRUD_Operations.Data;
using CRUD_Operations.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryPatternWithUOW.Core.Interface;
using RepositoryPatternWithUOW.Core;
using RepositoryPatternWithUOW.EF.Repositories;

namespace CRUD_Operations.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        //private readonly ApplicationDbContext _dbContext;
        //private readonly IBaseRepository<Product> _productrepository;
        private readonly IProductRepository _repository;    
        private readonly IUnitOfWork _unitOfWork;
        public ProductsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        [Route("")]
        public ActionResult<IEnumerable<Product>> Get() 
        {
            //var record = _dbContext.Products.ToList();
            return Ok(_unitOfWork.Products.GetValues());  
        }
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(int id)
        {

            //var record = await _dbContext.Products.FindAsync(id);
            return Ok(_unitOfWork.Products.GetById(id));
        }
        [HttpGet]
        [Route("GetByName")]
        public IActionResult GetByName(string name) 
        {
            return Ok(_unitOfWork.Products.Find(n => n.Name == name));
        }
        [HttpGet]
        [Route("GetOrderded")]
        public IActionResult GetOrdered(string description)
        {
            return Ok(_unitOfWork.Products.FindAll(p => p.Description == description, null, null, b => b.Price));
        }
        [HttpPost]
        public IActionResult CreateProduct(ProductDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Quantity = productDto.Quantity
            };

            return Ok(_unitOfWork.Products.Create(product)); 
        }
        [HttpPost]
        [Route("with new")]
        public IActionResult CreateProduct2(ProductDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Quantity = productDto.Quantity
            };

            _unitOfWork.Products.Create(product);
            _unitOfWork.Complete();
            return Ok(product);
        }
        [HttpPut]
        public async Task<IActionResult> Update(int id, [FromBody] ProductDto productDto)
        {
            var product = _unitOfWork.Products.GetById(id);
            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.Quantity = productDto.Quantity;
            
            return Ok(_unitOfWork.Products.Update(product)); 
        }
        [HttpGet("paged")]
        public async Task<ActionResult<PaginatedResult<Product>>> GetProducts(
        int pageNumber = 1, int pageSize = 10, string sortBy = null, string filter = null)
        {
            var result = await _unitOfWork.Products.GetProductsAsync(pageNumber, pageSize, sortBy, filter);
            return Ok(result);
        }
        //[HttpPatch("{id}")]
        //public async Task<IActionResult> UpdateProduct(int id, [FromBody] JsonPatchDocument<Product> patchDoc)
        //{
        //    if (patchDoc == null)
        //    {
        //        return BadRequest();
        //    }

        //    // ابحث عن المنتج المراد تحديثه
        //    var product = await _dbContext.Products.FindAsync(id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    // تطبيق التحديثات على المنتج
        //    patchDoc.ApplyTo(product);

        //    // تحقق من صحة المدخلات بعد التحديث
        //    //if (!ModelState.IsValid)
        //    //{
        //    //    return BadRequest(ModelState);
        //    //}

        //    // حفظ التحديثات في قاعدة البيانات
        //    await _dbContext.SaveChangesAsync();

        //    return Ok(product);
        //}
        [HttpPatch("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] JsonPatchDocument<Product> patchDoc)
        {
           var product =  _unitOfWork.Products.GetById(id);
            patchDoc.ApplyTo(product);
             _unitOfWork.Products.Update(product);
            _unitOfWork.Complete();
            return Ok(product);
        }

        [HttpDelete]
        [Route("")]
        public ActionResult Delete(int id) 
        {
            var p =_unitOfWork.Products.GetById(id);
            _unitOfWork.Products.Delete(p, id);
            _unitOfWork.Complete();
            return Ok(id);
        }
    }
}
