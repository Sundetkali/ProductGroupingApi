using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using ProductGroupingApi.Models;
using ProductGroupingApi.Data;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProductGroupingApi.Services;

namespace ProductGroupingApi.Controllers 
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ProductGroupingService _groupingService;

        public ProductsController(AppDbContext context, ProductGroupingService groupingService)
        {
            _context = context;
            _groupingService = groupingService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Пожалуйста, загрузите корректный Excel файл.");

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets.FirstOrDefault();

            if (worksheet == null)
                return BadRequest("Лист в файле не найден.");

            var products = new List<Product>();
            for (int row = 2; row <= worksheet.Dimension.Rows; row++)
            {
                var name = worksheet.Cells[row, 1].Text;
                var unit = worksheet.Cells[row, 2].Text;
                var price = decimal.Parse(worksheet.Cells[row, 3].Text);
                var quantity = int.Parse(worksheet.Cells[row, 4].Text);

                products.Add(new Product
                {
                    Name = name,
                    Unit = unit,
                    PricePerUnit = price,
                    Quantity = quantity
                });
            }
            await SaveProductsToDatabase(products);

            return Ok("Файл загружен и данные успешно сохранены.");
        }
        private async Task SaveProductsToDatabase(List<Product> products)
        {
            if (!await _context.Database.CanConnectAsync())
            {
                await _context.Database.EnsureCreatedAsync();
            }
            foreach (var product in products)
            {
                var existingProduct = await _context.Products
                    .FirstOrDefaultAsync(p => p.Name == product.Name && p.Unit == product.Unit);

                if (existingProduct == null)
                {
                    _context.Products.Add(product); 
                }
            }
            await _context.SaveChangesAsync();
        }

        [HttpGet("group-products")]
        public IActionResult GroupProducts()
        {
            var groups = _groupingService.GroupProducts();
            return Ok(groups);
        }
    }
}