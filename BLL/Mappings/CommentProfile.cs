using AutoMapper;
using DAL.DTOs.CategoryDTOs;
using DAL.DTOs.ReviewDTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Mappings
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, CommentDTO>().ReverseMap();
            CreateMap<Comment, CommentDetailDTO>();
            CreateMap<CommentCreateDTO, Comment>();
        }
    }
}
