namespace DAL.Models.Entities
{
    public sealed class OrderItem
    {
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public int Amount { get; set; }
        public bool IsBought { get; set; }
    }
}
