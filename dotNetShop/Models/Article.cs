using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotNetShop.Models
{
    public class Article
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Too short name")]
        [MaxLength(200, ErrorMessage = "Too long name, should not exceed {0} characters")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(0.0, double.MaxValue)]
        public double Price { get; set; }

        [NotMapped]
        [Display(Name = "New Image")]
        [JsonIgnore]
        public IFormFile ImageFile { get; set; }

        [Display(Name = "Image")]
        public string ImagePath { get; set; }

        [NotMapped]
        [Display(Name = "Image")]
        public string ResolvedImageFilePath { get; set; }

        [Display(Name = "Thumbnail")]
        public string ThumbnailPath { get; set; }

        [NotMapped]
        [Display(Name = "Thumbnail")]
        public string ResolvedThumbnailFilePath { get; set; }

        public Category Category { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }       
    }
}
