using dotNetShop.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotNetShop.Services
{
    public interface ICategoryService
    {
        Task<IList<Category>> GetAllCategories();

        Task<Category> GetCategory(int categoryId);

        Task<Category> CreateCategory(Category category);

        Task<Category> UpdateCategory(Category category);

        Task DeleteCategory(int categoryId);
    }
}
