using AutoMapper;
using DAL.DTOs.CategoryDTOs;
using DAL.DTOs.CommentDTOs;
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
            CreateMap<Comment, CommentDetailDTO>();
            CreateMap<CommentCreateDTO, Comment>();
        }
    }
}
