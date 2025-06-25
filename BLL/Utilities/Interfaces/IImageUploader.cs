using DAL.DTOs.ImageUploadDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Utilities.Interfaces
{
    public interface IImageUploader
    {
        /// <summary>
        /// Uploads an image file to ImgBB.
        /// </summary>
        /// <param name="imageFile">The image file (IFormFile) to upload.</param>
        /// <returns>An ImageUploadResult containing the file name and direct URL if successful; otherwise, null.</returns>
        Task<ImageUploadResultDTO?> UploadImageAsync(IFormFile imageFile);

        /// <summary>
        /// Uploads image bytes to ImgBB.
        /// </summary>
        /// <param name="imageBytes">The byte array of the image.</param>
        /// <param name="fileName">The desired file name for the uploaded image.</param>
        /// <param name="contentType">The MIME content type of the image (e.g., "image/jpeg").</param>
        /// <returns>An ImageUploadResult containing the file name and direct URL if successful; otherwise, null.</returns>
        Task<ImageUploadResultDTO?> UploadImageAsync(byte[] imageBytes, string fileName, string contentType);
    }
}
