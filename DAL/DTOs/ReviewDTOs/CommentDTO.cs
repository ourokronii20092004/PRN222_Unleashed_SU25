using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.ReviewDTOs
{
    public class CommentDTO
    {
        public int CommentId { get; set; }
        public int? ReviewId { get; set; }
        public string CommentContent { get; set; }
        public DateTimeOffset? CommentCreatedAt { get; set; }
        public DateTimeOffset? CommentUpdatedAt { get; set; }
        public int? ParentCommentId { get; set; }
    }

    public class CommentCreateDTO
    {
        public int? ReviewId { get; set; }
        public string CommentContent { get; set; }
        public int? ParentCommentId { get; set; }
    }

    public class CommentDetailDTO : CommentDTO
    {
        public ReviewDTO Review { get; set; }
        public CommentDTO ParentComment { get; set; }
        public IEnumerable<CommentDTO> Replies { get; set; }
    }

    public class CommentTreeDTO
    {
        public CommentDTO Comment { get; set; }
        public IEnumerable<CommentTreeDTO> Replies { get; set; }
    }
}
