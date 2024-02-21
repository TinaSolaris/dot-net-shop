using dotNetShop.Data;
using dotNetShop.Services;
using dotNetShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace dotNetShop.Controllers
{
    [AllowAnonymous]
    public class ShopController : Controller
    {
        private readonly ShopDbContext _context;
        private readonly IArticleService _articleService;

        public ShopController(ShopDbContext context, IArticleService articleService)
        {
            _context = context;
            _articleService = articleService;
        }

        public IActionResult Index()
        {
            var model = new ShopIndexViewModel();
            model.Categories = new SelectList(_context.Categories, "Id", "Name");

            return View(model);
        }

        private const int COUNT = 5;
        private const int DEFAULT_INDEX = 0;

        [HttpGet]
        public async Task<IActionResult> List(int categoryId)
        {
            if (categoryId <= 0)
                return NotFound();

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == categoryId);

            if (category == null)
                return NotFound();

            ViewBag.CategoryName = category.Name;
            ViewBag.Count = COUNT;

            var articles = await _articleService.GetArticles(categoryId, DEFAULT_INDEX, COUNT);

            return View("LazyLoadingList", articles);
        }

        [HttpGet]
        public async Task<IActionResult> ListPartial(int categoryId, int? index)
        {
            if (categoryId <= 0)
                return NotFound();

            var articles = await _articleService.GetArticles(categoryId, index ?? DEFAULT_INDEX, COUNT);

            // Thread.Sleep(1000); // Just to show the Loading message

            return PartialView("_ArticleListPartial", articles);
        }

        [HttpGet]
        public async Task<IActionResult> LoadNextArticles(int categoryId, int index, int count)
        {
            var articles = await _articleService.GetArticles(categoryId, index, count);

            return PartialView("_ArticleListPartial", articles);
        }
    }
}