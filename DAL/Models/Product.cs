namespace DAL.Models
{
    public sealed class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public int Platform { get; set; }
        public string DateCreated { get; set; }
        public int TotalRating { get; set; }
    }
}
