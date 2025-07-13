using System;

namespace DAL.DTOs.ReviewDTOs
{
    public class ReviewCreateDTO
    {
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public Guid OrderId { get; set; }
        public int ReviewRating { get; set; }
        public string ReviewContent { get; set; }
    }
}