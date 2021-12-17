namespace DAL.Models.Entities
{
    public sealed class ProductRating
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
    }
}