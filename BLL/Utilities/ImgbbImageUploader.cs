using BLL.Utilities.Interfaces;
using DAL.DTOs.ImageUploadDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Utilities
{
    public class ImgbbImageUploader : IImageUploader
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ImgbbImageUploader> _logger;
        private readonly string _imgbbApiKey;
        private const string ImgbbUploadUrl = "https://api.imgbb.com/1/upload";

        public ImgbbImageUploader(HttpClient httpClient, ILogger<ImgbbImageUploader> logger, IConfiguration configuration)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _imgbbApiKey = configuration["ExternalApiKeys:ImgBB"] ?? throw new ArgumentNullException("ImgBB API key is missing in configuration.");
        }

        public async Task<ImageUploadResultDTO?> UploadImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                _logger.LogWarning("UploadImageAsync (IFormFile) called with null or empty file.");
                return null;
            }

            if (!imageFile.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning($"Upload attempt with non-image content type: {imageFile.ContentType} for file {imageFile.FileName}.");
                return null;
            }

            await using var memoryStream = new MemoryStream();
            await imageFile.CopyToAsync(memoryStream);
            var imageBytes = memoryStream.ToArray();

            return await UploadImageAsync(imageBytes, imageFile.FileName, imageFile.ContentType);
        }

        public async Task<ImageUploadResultDTO?> UploadImageAsync(byte[] imageBytes, string fileName, string contentType)
        {
            if (imageBytes == null || imageBytes.Length == 0)
            {
                _logger.LogWarning("UploadImageAsync (bytes) called with null or empty imageBytes.");
                return null;
            }

            string originalFileName = fileName; // Keep original for the result
            if (string.IsNullOrWhiteSpace(fileName))
            {
                // Generate a name if one isn't provided, but the result should ideally reflect the actual name used or provided
                originalFileName = $"image_{Path.GetRandomFileName().Replace(".", "")}{Path.GetExtension(contentType.Split('/').LastOrDefault() ?? ".tmp")}";
                fileName = originalFileName;
                _logger.LogInformation($"No filename provided for byte array upload. Generated: {fileName}");
            }

            if (!contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning($"UploadImageAsync (bytes) attempt with non-image content type: {contentType} for file {fileName}.");
                return null;
            }

            try
            {
                using var formData = new MultipartFormDataContent();
                formData.Add(new StringContent(_imgbbApiKey), "key");

                var imageContent = new ByteArrayContent(imageBytes);
                imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);
                formData.Add(imageContent, "image", fileName);

                var response = await _httpClient.PostAsync(ImgbbUploadUrl, formData);

                if (response.IsSuccessStatusCode)
                {
                    var imgbbResponse = await response.Content.ReadFromJsonAsync<ImgbbUploadResponse>();
                    if (imgbbResponse != null && imgbbResponse.Success && imgbbResponse.Data != null && !string.IsNullOrEmpty(imgbbResponse.Data.Url))
                    {
                        _logger.LogInformation($"Image '{originalFileName}' uploaded successfully to ImgBB. Direct URL: {imgbbResponse.Data.Url}");
                        return new ImageUploadResultDTO(originalFileName, imgbbResponse.Data.Url);
                    }
                    else
                    {
                        var errorDetail = await response.Content.ReadAsStringAsync();
                        _logger.LogError("ImgBB upload reported failure or missing data. Status: {Status}. Response: {Detail}", imgbbResponse?.Status, errorDetail);
                        return null;
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("ImgBB HTTP error during upload for file '{FileName}'. Status: {StatusCode}. Response: {ErrorContent}", originalFileName, response.StatusCode, errorContent);
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception during ImgBB image upload for file '{FileName}'.", originalFileName);
                return null;
            }
        }

        Task<ImageUploadResultDTO?> IImageUploader.UploadImageAsync(IFormFile imageFile)
        {
            throw new NotImplementedException();
        }

        Task<ImageUploadResultDTO?> IImageUploader.UploadImageAsync(byte[] imageBytes, string fileName, string contentType)
        {
            throw new NotImplementedException();
        }
    }
}
