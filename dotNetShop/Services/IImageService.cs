using Microsoft.AspNetCore.Http;

namespace dotNetShop.Services
{
    public interface IImageService
    {
        string GetResolvedImageFilePath(string imageFileName, bool isThumbnail);
     
        string GenerateThumbnailDefault(string imageFileName, int width);

        string GenerateThumbnail(string imageFileName, int size);

        string SaveImage(IFormFile imageFile);

        void DeleteImage(string imageFilePath);
    }
}
