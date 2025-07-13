using System;
using System.Collections.Generic;
using DAL.DTOs.CommentDTOs;
using DAL.DTOs.UserDTOs;
using DAL.DTOs.ProductDTOs;

namespace DAL.DTOs.ReviewDTOs
{
    public class ReviewDetailDTO
    {
        public int ReviewId { get; set; }
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public Guid OrderId { get; set; }
        public int ReviewRating { get; set; }
        public string ReviewContent { get; set; }
        public DateTimeOffset? ReviewCreatedAt { get; set; }
        public DateTimeOffset? ReviewUpdatedAt { get; set; }
        public ProductDTO Product { get; set; }
        public UserDetailDTO User { get; set; }
        public IEnumerable<CommentDTO> Comments { get; set; }
    }
}