using dotNetShop.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotNetShop.Services
{
    public interface IArticleService
    {
        void ExtendViewModel(Article article);

        Task<IList<Article>> GetArticles(int categoryId, int index, int count);
        Task<IList<Article>> GetArticlesByCategory(int categoryId);

        Task<IList<Article>> GetAllArticles();

        Task<Article> GetArticle(int articleId);

        Task<Article> CreateArticle(Article article);

        Task<Article> UpdateArticle(Article article);

        Task DeleteArticle(int articleId);
    }
}
