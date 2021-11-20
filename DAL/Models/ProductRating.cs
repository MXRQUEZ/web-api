namespace DAL.Models
{
    public sealed class ProductRating
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public User User { get; set; }
        public Product Product { get; set; }
    }
}
