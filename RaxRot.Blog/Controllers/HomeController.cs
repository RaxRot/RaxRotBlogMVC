using Microsoft.AspNetCore.Mvc;
using RaxRot.Blog.Models;
using RaxRot.Blog.Models.ViewModels;
using RaxRot.Blog.Repositories;
using System.Diagnostics;

namespace RaxRot.Blog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogPostRepository _blogPost;
        private readonly ITagRepository _tagRepository;

        public HomeController(ILogger<HomeController> logger,
            IBlogPostRepository blogPost,
            ITagRepository tagRepository)
        {
            _logger = logger;
            _blogPost = blogPost;
            _tagRepository = tagRepository;
        }

        public async Task<IActionResult> Index()
        {
           var blogPosts = await _blogPost.GetAllAsync();

           var tags = await _tagRepository.GetAllAsync();

            var model = new HomeViewModel
            {
                BlogPosts = blogPosts,
                Tags = tags
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
