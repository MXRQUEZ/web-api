namespace DAL.Models
{
    public sealed class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Platform { get; set; }

        public string DateCreated { get; set; }

        public int TotalRating { get; set; }

        public string Genre { get; set; }

        public int Rating { get; set; }

        public string Logo { get; set; }

        public string Background { get; set; }

        public string Price { get; set; }

        public int Count { get; set; }
    }
}
