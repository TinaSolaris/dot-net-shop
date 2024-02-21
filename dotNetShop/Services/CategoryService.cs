using dotNetShop.Data;
using dotNetShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace dotNetShop.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CategoryService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient CreateHttpClient()
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.AppendRequestDefaults();

            return httpClient;
        }

        public async Task<Category> CreateCategory(Category category)
        {
            try
            {
                HttpClient httpClient = CreateHttpClient();

                string url = Settings.WebApi.CategoryPath;
                var response = await httpClient.PostAsJsonAsync(url, category);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                Category result = JsonConvert.DeserializeObject<Category>(jsonResponse);

                return result;
            }
            catch (Exception ex)
            {
                // TODO: Log the error
                throw;
            }
        }

        public async Task DeleteCategory(int categoryId)
        {
            try
            {
                HttpClient httpClient = CreateHttpClient();

                string url = Settings.WebApi.CategoryPath + categoryId;
                var response = await httpClient.DeleteAsync(url);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                // TODO: Log the error
                throw;
            }
        }

        public async Task<IList<Category>> GetAllCategories()
        {
            try
            {
                HttpClient httpClient = CreateHttpClient();

                string url = Settings.WebApi.CategoryPath;

                var response = await httpClient.GetFromJsonAsync(url, typeof(IEnumerable<Category>));
                IEnumerable<Category> categories = (IEnumerable<Category>)response;

                return categories.ToList();
            }
            catch (Exception ex)
            {
                // TODO: Log the error
                throw;
            }
        }

        public async Task<Category> GetCategory(int categoryId)
        {
            try
            {
                HttpClient httpClient = CreateHttpClient();

                string url = Settings.WebApi.CategoryPath + categoryId;

                var response = await httpClient.GetFromJsonAsync(url, typeof(Category));
                Category category = (Category)response;

                return category;
            }
            catch (Exception ex)
            {
                // TODO: Log the error
                throw;
            }
        }

        public async Task<Category> UpdateCategory(Category category)
        {
            try
            {
                HttpClient httpClient = CreateHttpClient();

                string url = Settings.WebApi.CategoryPath + category.Id;
                var response = await httpClient.PutAsJsonAsync(url, category);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                Category result = JsonConvert.DeserializeObject<Category>(jsonResponse);

                return result;
            }
            catch (Exception ex)
            {
                // TODO: Log the error
                throw;
            }
        }
    }
}
