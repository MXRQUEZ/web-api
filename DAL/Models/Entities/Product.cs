namespace DAL.Models.Entities
{
    public sealed class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Platform Platform { get; set; }

        public string DateCreated { get; set; }

        public Genre Genre { get; set; }

        public Rating Rating { get; set; }

        public string Logo { get; set; }

        public string Background { get; set; }

        public int Price { get; set; }

        public int Count { get; set; }

        public int TotalRating { get; set; }
    }
}