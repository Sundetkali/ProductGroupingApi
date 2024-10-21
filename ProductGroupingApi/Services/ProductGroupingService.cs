using ProductGroupingApi.Models;
using Microsoft.EntityFrameworkCore; 
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductGroupingApi.Data;

namespace ProductGroupingApi.Services
{
    public class ProductGroupingService
    {
        private readonly AppDbContext _context;

        public ProductGroupingService(AppDbContext context)
        {
            _context = context;
        }
        public List<ProductGroup> GroupProducts()
        {
            var groups = new List<ProductGroup>();
            var products = _context.Products.Where(p => p.Quantity > 0).ToList();
            int groupNumber = 1;

            while (products.Any(p => p.Quantity > 0))
            {
                var currentGroup = new ProductGroup
                {
                    GroupName = $"Группа {groupNumber++}",
                    TotalPrice = 0
                };

                foreach (var product in products.Where(p => p.Quantity > 0))
                {
                    while (product.Quantity > 0 && currentGroup.TotalPrice + product.PricePerUnit <= 200)
                    {
                        currentGroup.Products.Add(product);
                        currentGroup.TotalPrice += product.PricePerUnit;
                        product.Quantity--; 
                    }
                }

                if (currentGroup.Products.Any())
                {
                    groups.Add(currentGroup);
                }
            }

            SaveProductGroups(groups);
            return groups;
        }

        private void SaveProductGroups(List<ProductGroup> groups)
        {
            foreach (var group in groups)
            {
                var existingGroup = _context.ProductGroups.FirstOrDefault(g => g.GroupName == group.GroupName);
                if (existingGroup == null)
                {
                    _context.ProductGroups.Add(group);
                }
            }
            _context.SaveChanges();
        }
    }
}
