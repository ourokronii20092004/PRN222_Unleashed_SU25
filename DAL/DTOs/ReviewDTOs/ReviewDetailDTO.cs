using DAL.DTOs.CommentDTOs;
using DAL.DTOs.ProductDTOs;
using DAL.DTOs.UserDTOs;
using DAL.Models;
using System;
using System.Collections.Generic;

namespace DAL.DTOs.ReviewDTOs
{
    public class ReviewDetailDTO
    {
        public int ReviewId { get; set; }
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public Guid OrderId { get; set; }
        public int ReviewRating { get; set; }
        public DateTimeOffset? ReviewCreatedAt { get; set; }
        public DateTimeOffset? ReviewUpdatedAt { get; set; }
        public ProductDTO Product { get; set; }
        public virtual User? User { get; set; }
    }
}