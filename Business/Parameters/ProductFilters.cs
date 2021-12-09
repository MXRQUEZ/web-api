using DAL.Models;

namespace Business.Parameters
{
    public sealed class ProductFilters
    {
        public Genre Genre { get; set; } = Genre.All;
        public Rating Rating { get; set; } = Rating.All;
        public bool RatingAscending { get; set; } = false;
        public bool PriceAscending { get; set; } = true;
    }
}