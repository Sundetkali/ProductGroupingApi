namespace ProductGroupingApi.Models
{
    public class Product
    {
        public int Id { get; set; } 
        public string Name { get; set; } 
        public string Unit { get; set; } 
        public decimal PricePerUnit { get; set; } 
        public int Quantity { get; set; }       
    }
}
