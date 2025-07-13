using System;

namespace DAL.DTOs.CommentDTOs
{
    public class CommentDetailDTO
    {
        public int CommentId { get; set; }
        public int? ReviewId { get; set; }
        public Guid UserId { get; set; }
        public string CommentContent { get; set; }
        public DateTimeOffset? CommentCreatedAt { get; set; }
        public DateTimeOffset? CommentUpdatedAt { get; set; }
    }
}