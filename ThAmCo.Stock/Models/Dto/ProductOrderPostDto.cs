namespace ThAmCo.Stock.Models.Dto
{
    public class ProductOrderPostDto
    {
        public string AccountName { get; set; }
        public string CardNumber { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
