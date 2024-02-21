using dotNetShop.Data;
using dotNetShop.Models;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace dotNetShop.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IImageService _imageService;
        private readonly IHttpClientFactory _httpClientFactory;

        public ArticleService(IImageService imageService, IHttpClientFactory httpClientFactory)
        {
            _imageService = imageService;
            _httpClientFactory = httpClientFactory;
        }

        public void ExtendViewModel(Article article)
        {
            article.ResolvedImageFilePath = _imageService.GetResolvedImageFilePath(article.ImagePath, false);
            article.ResolvedThumbnailFilePath = _imageService.GetResolvedImageFilePath(article.ThumbnailPath, true);
        }

        private HttpClient CreateHttpClient()
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.AppendRequestDefaults();

            return httpClient;
        }

        public async Task<IList<Article>> GetArticles(int categoryId, int index, int count)
        {
            try
            {
                HttpClient httpClient = CreateHttpClient();

                IDictionary<string, string> query = new Dictionary<string, string>
                {
                    ["categoryId"] = categoryId.ToString(),
                    ["index"] = index.ToString(),
                    ["count"] = count.ToString()
                };

                string url = QueryHelpers.AddQueryString(Settings.WebApi.ArticlePath, query);

                var response = await httpClient.GetFromJsonAsync(url, typeof(IEnumerable<Article>));
                IEnumerable<Article> articles = (IEnumerable<Article>)response;

                var result = articles.ToList();
                result.ForEach(a => ExtendViewModel(a));

                return articles.ToList();
            }
            catch (Exception ex)
            {
                // TODO: Log the error
                throw;
            }
        }

        public async Task<IList<Article>> GetArticlesByCategory(int categoryId)
        {
            try
            {
                HttpClient httpClient = CreateHttpClient();

                IDictionary<string, string> query = new Dictionary<string, string>
                {
                    ["categoryId"] = categoryId.ToString(),
                };

                string url = QueryHelpers.AddQueryString(Settings.WebApi.ArticlePath, query);

                var response = await httpClient.GetFromJsonAsync(url, typeof(IEnumerable<Article>));
                IEnumerable<Article> articles = (IEnumerable<Article>)response;

                var result = articles.ToList();
                result.ForEach(a => ExtendViewModel(a));

                return articles.ToList();
            }
            catch (Exception ex)
            {
                // TODO: Log the error
                throw;
            }
        }

        public async Task<IList<Article>> GetAllArticles()
        {
            try
            {
                HttpClient httpClient = CreateHttpClient();

                string url = Settings.WebApi.ArticlePath;

                var response = await httpClient.GetFromJsonAsync(url, typeof(IEnumerable<Article>));
                IEnumerable<Article> articles = (IEnumerable<Article>)response;

                var result = articles.ToList();
                result.ForEach(a => ExtendViewModel(a));

                return articles.ToList();
            }
            catch (Exception ex)
            {
                // TODO: Log the error
                throw;
            }
        }

        public async Task<Article> GetArticle(int articleId)
        {
            try
            {
                HttpClient httpClient = CreateHttpClient();

                string url = Settings.WebApi.ArticlePath + articleId;

                var response = await httpClient.GetFromJsonAsync(url, typeof(Article));
                Article article = (Article)response;

                ExtendViewModel(article);

                return article;
            }
            catch (Exception ex)
            {
                // TODO: Log the error
                throw;
            }
        }

        public async Task<Article> CreateArticle(Article article)
        {
            try
            {
                HttpClient httpClient = CreateHttpClient();

                string url = Settings.WebApi.ArticlePath;
                var response = await httpClient.PostAsJsonAsync(url, article);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                Article result = JsonConvert.DeserializeObject<Article>(jsonResponse);

                ExtendViewModel(result);

                return result;
            }
            catch (Exception ex)
            {
                // TODO: Log the error
                throw;
            }
        }

        public async Task<Article> UpdateArticle(Article article)
        {
            try
            {
                HttpClient httpClient = CreateHttpClient();

                string url = Settings.WebApi.ArticlePath + article.Id;
                var response = await httpClient.PutAsJsonAsync(url, article);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                Article result = JsonConvert.DeserializeObject<Article>(jsonResponse);

                ExtendViewModel(result);

                return result;
            }
            catch (Exception ex)
            {
                // TODO: Log the error
                throw;
            }
        }

        public async Task DeleteArticle(int articleId)
        {
            try
            {
                HttpClient httpClient = CreateHttpClient();

                string url = Settings.WebApi.ArticlePath + articleId;
                var response = await httpClient.DeleteAsync(url);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                // TODO: Log the error
                throw;
            }
        }
    }
}
