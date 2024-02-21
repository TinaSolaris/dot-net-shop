using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using dotNetShop.Data;
using dotNetShop.Models;
using dotNetShop.Services;

namespace dotNetShop.Controllers
{
    [Authorize(Roles = nameof(Role.Admin))]
    [Authorize(Policy = Settings.PolicyName.Admin)]
    public class ArticleController : Controller
    {
        private const int THUMB_WIDTH = 200;
        private readonly IImageService _imageService;
        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;

        public ArticleController(IImageService imageService, IArticleService articleService, ICategoryService categoryService)
        {
            _imageService = imageService;
            _articleService = articleService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var articles = await _articleService.GetAllArticles();

            return View(articles);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateCategories(null);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,ImagePath,ThumbnailPath,ImageFile,CategoryId")] Article article)
        {
            if (ModelState.IsValid)
            {
                if (article.ImageFile != null)
                {
                    article.ImagePath = _imageService.SaveImage(article.ImageFile);
                    article.ThumbnailPath = _imageService.GenerateThumbnail(article.ImagePath, THUMB_WIDTH);
                }

                article = await _articleService.CreateArticle(article);

                return RedirectToAction(nameof(Index));
            }

            await PopulateCategories(article);
            _articleService.ExtendViewModel(article);

            return View(article);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var article = await _articleService.GetArticle(id.Value);

            if (article == null)
                return NotFound();

            return View(article);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var article = await _articleService.GetArticle(id.Value);

            if (article == null)
                return NotFound();

            await PopulateCategories(article);
            _articleService.ExtendViewModel(article);

            return View(article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,ImagePath,ThumbnailPath,ImageFile,CategoryId")] Article article)
        {
            if (id != article.Id)
                return NotFound();

            if (!await ArticleExists(article.Id))
                return NotFound();

            if (ModelState.IsValid)
            {                
                if (article.ImageFile != null)
                {
                    DeleteArticleImages(article);

                    article.ImagePath = _imageService.SaveImage(article.ImageFile);
                    article.ThumbnailPath = _imageService.GenerateThumbnail(article.ImagePath, THUMB_WIDTH);
                }

                article = await _articleService.UpdateArticle(article);

                return RedirectToAction(nameof(Index));
            }

            await PopulateCategories(article);
            _articleService.ExtendViewModel(article);

            return View(article);
        }

        private async Task PopulateCategories(Article article)
        {
            ViewBag.Categories = new SelectList(await _categoryService.GetAllCategories(), "Id", "Name", (article != null ? article.CategoryId : null));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var article = await _articleService.GetArticle(id.Value);

            if (article == null)
                return NotFound();

            _articleService.ExtendViewModel(article);

            return View(article);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _articleService.GetArticle(id);

            if (article == null)
                return NotFound();

            DeleteArticleImages(article);

            await _articleService.DeleteArticle(id);

            return RedirectToAction(nameof(Index));
        }

        private void DeleteArticleImages(Article article)
        {
            if (!string.IsNullOrEmpty(article.ImagePath))
                _imageService.DeleteImage(article.ImagePath);

            if (!string.IsNullOrEmpty(article.ThumbnailPath))
                _imageService.DeleteImage(article.ThumbnailPath);
        }

        private async Task<bool> ArticleExists(int id)
        {
            var article = await _articleService.GetArticle(id);

            return article != null;
        }
    }
}