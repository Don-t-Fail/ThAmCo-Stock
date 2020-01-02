namespace ThAmCo.Stock.Models.ViewModel
{
    public class OrderRequestReviewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string Supplier { get; set; }
    }
}
