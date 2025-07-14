using DAL.DTOs.CommentDTOs;
using DAL.Models;
using System;
using System.Collections.Generic;

namespace DAL.DTOs.ReviewDTOs
{
    public class ReviewDTO
    {
        public int ReviewId { get; set; }
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public Guid OrderId { get; set; }
        public int ReviewRating { get; set; }
        public DateTimeOffset? ReviewCreatedAt { get; set; }
        public DateTimeOffset? ReviewUpdatedAt { get; set; }
        public virtual User? User { get; set; }
        public virtual Order? Order { get; set; }
        public virtual Product? Product { get; set; }
    }
}