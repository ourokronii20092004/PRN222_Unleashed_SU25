using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.DTOs.ImageUploadDTOs
{
    public class ImgbbUploadData
    {
        [JsonPropertyName("url")] // This is the direct image URL
        public string? Url { get; set; }

        [JsonPropertyName("display_url")]
        public string? DisplayUrl { get; set; }
        // You can add other fields like 'filename' from ImgBB's image object if you want to compare
        // [JsonPropertyName("image")]
        // public ImgbbImageDetails Image { get; set; }
    }
    public class ImgbbUploadResponse
    {
        [JsonPropertyName("data")]
        public ImgbbUploadData? Data { get; set; }

        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }
    }

}
