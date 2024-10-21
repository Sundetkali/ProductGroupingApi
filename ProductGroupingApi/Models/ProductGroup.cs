namespace ProductGroupingApi.Models
{
    public class ProductGroup
    {
        public int Id { get; set; } 
        public string GroupName { get; set; } 
        public decimal TotalPrice { get; set; } 
        public List<Product> Products { get; set; } = new List<Product>(); 

    }
}
