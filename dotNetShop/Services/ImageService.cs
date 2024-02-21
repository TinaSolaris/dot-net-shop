using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace dotNetShop.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        private const string ROOT_PATH = "/";
        private const string UPLOAD_FOLDER_NAME = "upload";
        private const string IMAGE_FOLDER_NAME = "image";
        private const string DEFAULT_IMAGE_NAME = "no_photo.jpg";
        private const string DEFAULT_THUMB_NAME = "no_photo_thumb.jpg";

        private string _uploadFolderPath;

        private string UploadFolderPath {
            get 
            {
                if (string.IsNullOrWhiteSpace(_uploadFolderPath))
                {
                    _uploadFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, UPLOAD_FOLDER_NAME);

                    if (!Directory.Exists(_uploadFolderPath))
                        Directory.CreateDirectory(_uploadFolderPath);
                }

                return _uploadFolderPath;
            }
        }

        public ImageService(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public string GetResolvedImageFilePath(string imageFileName, bool isThumbnail)
        {
            string result;

            if (string.IsNullOrEmpty(imageFileName))
                result = Path.Combine(ROOT_PATH, IMAGE_FOLDER_NAME, (isThumbnail ? DEFAULT_THUMB_NAME : DEFAULT_IMAGE_NAME));
            else
                result = Path.Combine(ROOT_PATH, UPLOAD_FOLDER_NAME, imageFileName);

            result = result.Replace(Path.DirectorySeparatorChar, '/');

            return result;
        }

        public string GenerateThumbnail(string imageFileName, int size)
        {
            if (string.IsNullOrEmpty(imageFileName))
                return null;

            string imagePath = Path.Combine(UploadFolderPath, imageFileName);
            string thumbnailFileName = imageFileName.Insert(imageFileName.LastIndexOf('.'), "_thumb");

            using (var originalImage = Image.FromFile(imagePath))
            {
                using (var thumbnail = new Bitmap(size, size))
                {
                    using (var graphics = Graphics.FromImage(thumbnail))
                    {
                        graphics.CompositingQuality = CompositingQuality.HighQuality;
                        graphics.SmoothingMode = SmoothingMode.HighQuality;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                        double ratioX = (double)size / (double)originalImage.Width;
                        double ratioY = (double)size / (double)originalImage.Height;
                        double ratio = ratioX < ratioY ? ratioX : ratioY;

                        int newHeight = Convert.ToInt32(originalImage.Height * ratio);
                        int newWidth = Convert.ToInt32(originalImage.Width * ratio);

                        int posX = Convert.ToInt32((size - (originalImage.Width * ratio)) / 2);
                        int posY = Convert.ToInt32((size - (originalImage.Height * ratio)) / 2);

                        graphics.Clear(Color.White);
                        graphics.DrawImage(originalImage, posX, posY, newWidth, newHeight);
                    }

                    string thumbnailFilePath = Path.Combine(UploadFolderPath, thumbnailFileName);

                    thumbnail.Save(thumbnailFilePath, ImageFormat.Jpeg);
                }
            }

            return thumbnailFileName;
        }

        public string GenerateThumbnailDefault(string imageFileName, int width)
        {
            if (string.IsNullOrEmpty(imageFileName))
                return null;

            string imagePath = Path.Combine(UploadFolderPath, imageFileName);
            string thumbnailFileName = imageFileName.Insert(imageFileName.LastIndexOf('.'), "_thumb");

            using (var originalImage = Image.FromFile(imagePath))
            {
                int height = (int)((double)originalImage.Height / originalImage.Width * width);

                using (var thumbnail = new Bitmap(width, height))
                {
                    using (var graphics = Graphics.FromImage(thumbnail))
                    {
                        graphics.CompositingQuality = CompositingQuality.HighQuality;
                        graphics.SmoothingMode = SmoothingMode.HighQuality;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        
                        graphics.DrawImage(originalImage, 0, 0, width, height);
                    }

                    string thumbnailFilePath = Path.Combine(UploadFolderPath, thumbnailFileName);

                    thumbnail.Save(thumbnailFilePath, ImageFormat.Jpeg);
                }
            }

            return thumbnailFileName;
        }

        public string SaveImage(IFormFile image)
        {
            if (image == null)
                return null;

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(image.FileName);

            string imagePath = Path.Combine(UploadFolderPath, uniqueFileName);

            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                image.CopyTo(fileStream);
            }

            return uniqueFileName;
        }

        public void DeleteImage(string imagePath)
        {
            string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "upload");
            string filePath = Path.Combine(uploadFolder, imagePath);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }        
    }
}
