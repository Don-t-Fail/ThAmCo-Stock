using System;

namespace ThAmCo.Stock.Data
{
    public class OrderRequest
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public DateTime SubmittedTime { get; set; }
        public bool Approved { get; set; }
        public DateTime ApprovedTime { get; set; }
        public bool Deleted { get; set; }
    }
}
