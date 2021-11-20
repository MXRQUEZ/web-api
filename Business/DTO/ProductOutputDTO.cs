namespace Business.DTO
{
    public sealed class ProductOutputDTO : ProductDTO
    {
        /// <summary>
        /// Product rating from 1 to 100
        /// </summary>
        /// <example>
        /// 10 - bad;
        /// 30 - could be better;
        /// 50 - okay;
        /// 70 - good;
        /// 100 - masterpiece.
        /// </example>

        public int TotalRating { get; set; }

        /// <summary>
        /// Link to logo picture
        /// </summary>
        
        public string Logo { get; set; }

        /// <summary>
        /// Link to background picture
        /// </summary>
        
        public string Background { get; set; }
    }
}
