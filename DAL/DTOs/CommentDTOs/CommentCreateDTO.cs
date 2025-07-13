using System;

namespace DAL.DTOs.CommentDTOs
{
    public class CommentCreateDTO
    {
        public int? ReviewId { get; set; }
        public Guid UserId { get; set; }
        public string CommentContent { get; set; }
    }
}