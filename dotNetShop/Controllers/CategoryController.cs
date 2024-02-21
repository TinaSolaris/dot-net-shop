using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using dotNetShop.Data;
using dotNetShop.Models;
using dotNetShop.Services;
using Microsoft.AspNetCore.Authorization;

namespace dotNetShop.Controllers
{
    [Authorize(Roles = nameof(Role.Admin))]
    [Authorize(Policy = Settings.PolicyName.Admin)]
    public class CategoryController : Controller
    {
        private readonly IImageService _imageService;
        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;

        public CategoryController(IImageService imageService, IArticleService articleService, ICategoryService categoryService)
        {
            _imageService = imageService;
            _articleService = articleService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllCategories();
            return View(categories);            
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                category = await _categoryService.CreateCategory(category);

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var category = await _categoryService.GetCategory(id.Value);

            if (category == null)
                return NotFound();

            return View(category);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var category = await _categoryService.GetCategory(id.Value);

            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Category category)
        {
            if (id != category.Id)
                return NotFound();

            if (!await CategoryExists(category.Id))
                return NotFound();

            if (ModelState.IsValid)
            {
                category = await _categoryService.UpdateCategory(category);

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var category = await _categoryService.GetCategory(id.Value);

            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _categoryService.GetCategory(id);

            if (category == null)
                return NotFound();

            var articles = await _articleService.GetArticlesByCategory(id);

            var result = articles.ToList();
            result.ForEach(a => DeleteArticle(a));

            await _categoryService.DeleteCategory(id);

            return RedirectToAction(nameof(Index));
        }

        private void DeleteArticle(Article article)
        {
            if (!string.IsNullOrEmpty(article.ImagePath))
                _imageService.DeleteImage(article.ImagePath);

            if (!string.IsNullOrEmpty(article.ThumbnailPath))
                _imageService.DeleteImage(article.ThumbnailPath);

            _articleService.DeleteArticle(article.Id);
        }

        private async Task<bool> CategoryExists(int id)
        {
            var category = await _categoryService.GetCategory(id);
            return category != null;
        }
    }
}
