using DAL.Models;
using System;

namespace DAL.DTOs.ReviewDTOs
{
    public class ReviewCreateDTO
    {
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public Guid OrderId { get; set; }
        public int ReviewRating { get; set; }
        public virtual User? User { get; set; }
    }
}