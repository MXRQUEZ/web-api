﻿using System.ComponentModel.DataAnnotations;

namespace Business.DTO
{
    public sealed class ProductDTO
    {
        /// <summary>
        /// Product name
        /// </summary>
        /// <example>"My Best Game"</example>
        [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "Name must contain only letters")]
        public string Name { get; set; }
        /// <summary>
        /// Product platform. Use numbers to chose platform
        /// </summary>
        /// <example>
        /// 1 - PC;
        /// 2 - Mobile;
        /// 4 - PS;
        /// 8 - Xbox;
        /// 16 - Nintendo.
        /// </example>
        public int Platform { get; set; }

        /// <summary>
        /// Product date of creation
        /// </summary>
        /// <example>dd/mm/yyyy</example>
        [RegularExpression(@"^([012]\d|30|31)/(0\d|10|11|12)/\d{4}$", ErrorMessage = "Date must be dd/mm/yyyy")]
        public string DateCreated { get; set; }

        /// <summary>
        /// Product rating from 1 to 5
        /// </summary>
        /// <example>
        /// 1 - bad;
        /// 2 - not so bad;
        /// 3 - okay;
        /// 4 - good;
        /// 5 - masterpiece.
        /// </example>
        [RegularExpression(@"^[1-5]*$", ErrorMessage = "Numbers must be from 1 to 5 only")]
        public int TotalRating { get; set; }
    }
}