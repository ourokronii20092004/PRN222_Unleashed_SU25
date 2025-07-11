using DAL.DTOs.ProductDTOs;
using DAL.DTOs.UserDTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.ReviewDTOs
{
    public class ReviewDTO
    {
        public int ReviewId { get; set; }
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public Guid OrderId { get; set; }
        public int? ReviewRating { get; set; }
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public virtual Order? Order { get; set; }

        public virtual Product? Product { get; set; }

        public virtual User? User { get; set; }
    }

    public class ReviewCreateDTO    
    {
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public Guid OrderId { get; set; } 
        public int? ReviewRating { get; set; }
    }

    public class ReviewDetailDTO : ReviewDTO
    {
        public ProductDTO Product { get; set; }
        public UserDetailDTO User { get; set; }
        public IEnumerable<CommentDTO> Comments { get; set; }
    }
}
