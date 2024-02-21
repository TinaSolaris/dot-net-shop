using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dotNetShop.Data;
using dotNetShop.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using dotNetShop.Auth;

namespace dotNetShop.ApiControllers
{
    [EnableCors]
    [AllowAnonymous]
    [BasicAuthentication]
    [Route("api/article/")]
    [ApiController]
    public class ArticleApiController : ControllerBase
    {
        private readonly ShopDbContext _context;

        public ArticleApiController(ShopDbContext context)
        {
            _context = context;
        }

        // GET: api/article/?categoryId=1&index=0&count=10
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles(
            [FromQuery(Name = "categoryId")] int? categoryId,
            [FromQuery(Name = "index")] int? index,
            [FromQuery(Name = "count")] int? count)
        {
            if (categoryId == null)
                return await _context.Articles
                    .Include(a => a.Category)
                    .ToListAsync();

            return await _context.Articles
                .Where(a => a.CategoryId == categoryId.Value)
                .Include(a => a.Category)
                .Skip(index ?? 0)
                .Take(count ?? 10)
                .ToListAsync();
        }

        // GET: api/article/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Article>> GetArticle(int id)
        {
            var article = await _context.Articles
                .Include(a => a.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (article == null)
                return NotFound();

            return article;
        }

        // PUT: api/article/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Article>> PutArticle(int id, Article article)
        {
            if (id != article.Id)
                return BadRequest();

            _context.Entry(article).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(id))
                    return NotFound();
                else
                    throw;
            }

            return article;
        }

        // POST: api/article
        [HttpPost]
        public async Task<ActionResult<Article>> PostArticle(Article article)
        {
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetArticle), new { id = article.Id }, article);
        }

        // DELETE: api/article/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
                return NotFound();

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }
    }
}
