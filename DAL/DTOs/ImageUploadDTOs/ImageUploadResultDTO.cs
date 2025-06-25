using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.ImageUploadDTOs
{
    public class ImageUploadResultDTO
    {
        public string FileName { get; set; }
        public string Url { get; set; }
        public ImageUploadResultDTO(string fileName, string url)
        {
            FileName = fileName;
            Url = url;
        }
    }

}