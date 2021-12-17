using System.ComponentModel.DataAnnotations;

namespace Business.Parameters
{
    public sealed class PageParameters
    {
        private const int MaxPageSize = 10;

        private int _pageSize = 5;

        [Required] public int PageNumber { get; set; } = 1;

        [Required]
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }
}