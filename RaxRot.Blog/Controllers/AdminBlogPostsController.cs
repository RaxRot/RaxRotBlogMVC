using Microsoft.AspNetCore.Mvc;
using RaxRot.Blog.Models.ViewModels;
using RaxRot.Blog.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using RaxRot.Blog.Models.Domain;

namespace RaxRot.Blog.Controllers
{
    public class AdminBlogPostsController : Controller
    {
        private ITagRepository _tagRepository;
        private IBlogPostRepository _blogPostRepository;
        public AdminBlogPostsController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository)
        {
            _tagRepository = tagRepository;
            _blogPostRepository = blogPostRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var tags = await _tagRepository.GetAllAsync();
            var model = new AddBlogPostRequest
            {
                Tags = tags.Select(x => new SelectListItem
                {
                    Text = x.DisplayName,
                    Value = x.Id.ToString()
                })
            };

            return View(model);
        }

        [HttpPost, ActionName("Add")]
        public async Task<IActionResult> AddBlog(AddBlogPostRequest addBlogPostRequest)
        {
            var blogPost = new BlogPost
            {
                Heading = addBlogPostRequest.Heading,
                PageTitle = addBlogPostRequest.PageTitle,
                Content = addBlogPostRequest.Content,
                ShortDescription = addBlogPostRequest.ShortDescription,
                FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
                UrlHandle = addBlogPostRequest.UrlHandle,
                PublishedDate = addBlogPostRequest.PublishedDate,
                Author = addBlogPostRequest.Author,
                Visible = addBlogPostRequest.Visible
            };

            var selectedTags = new List<Tag>();

            foreach (var selectedTagId in addBlogPostRequest.SelectedTags)
            {
                var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
                var existingTag = await _tagRepository.GetAsync(selectedTagIdAsGuid);

                if (existingTag != null)
                {
                    selectedTags.Add(existingTag);
                }
            }

            blogPost.Tags = selectedTags;

            await _blogPostRepository.AddAsync(blogPost);

            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var blogPosts = await _blogPostRepository.GetAllAsync();

            return View(blogPosts);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var blogPost = await _blogPostRepository.GetAsync(id);
            var tagsDomainModel = await _tagRepository.GetAllAsync();

            if (blogPost != null)
            {
                var model = new EditBlogPostRequest
                {
                    Id = blogPost.Id,
                    Heading = blogPost.Heading,
                    PageTitle = blogPost.PageTitle,
                    Content = blogPost.Content,
                    Author = blogPost.Author,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    ShortDescription = blogPost.ShortDescription,
                    PublishedDate = blogPost.PublishedDate,
                    Visible = blogPost.Visible,
                    Tags = tagsDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),

                    SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray()

                };

                return View(model);
            }


            return View(null);
        }

        [HttpPost,ActionName("Edit")]
        public async Task<IActionResult> EditBlog(EditBlogPostRequest editBlogPostRequest)
        {
            var blogPostDomainModel = new BlogPost
            {
                Id = editBlogPostRequest.Id,
                Heading = editBlogPostRequest.Heading,
                PageTitle = editBlogPostRequest.PageTitle,
                Content = editBlogPostRequest.Content,
                Author = editBlogPostRequest.Author,
                ShortDescription = editBlogPostRequest.ShortDescription,
                FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
                UrlHandle = editBlogPostRequest.UrlHandle,
                PublishedDate = editBlogPostRequest.PublishedDate,
                Visible = editBlogPostRequest.Visible
            };

            var selectedTags = new List<Tag>();
            foreach(var selectedTag in editBlogPostRequest.SelectedTags)
            {
                if(Guid.TryParse(selectedTag,out var tag))
                {
                   var foundedTag = await _tagRepository.GetAsync(tag);

                    if(foundedTag != null)
                    {
                        selectedTags.Add(foundedTag);
                    }
                }
            }

            blogPostDomainModel.Tags = selectedTags;

           var updatedBlog = await _blogPostRepository.UpdateAsync(blogPostDomainModel);
            if(updatedBlog != null)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Edit");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var blogPost = await _blogPostRepository.GetAsync(id);
            var tagsDomainModel = await _tagRepository.GetAllAsync();

            if (blogPost != null)
            {
                var model = new DeleteBlogPostRequest
                {
                    Id = blogPost.Id,
                    Heading = blogPost.Heading,
                    PageTitle = blogPost.PageTitle,
                    Content = blogPost.Content,
                    Author = blogPost.Author,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    ShortDescription = blogPost.ShortDescription,
                    PublishedDate = blogPost.PublishedDate,
                    Visible = blogPost.Visible,
                    Tags = tagsDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),

                    SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray()

                };

                return View(model);
            }


            return View(null);
        }

        [HttpPost,ActionName("Delete")]
        public async Task<IActionResult> DeleteBlog(DeleteBlogPostRequest deleteBlogPostRequest)
        {
           var deletedBlogPost = await _blogPostRepository.DeleteAsync(deleteBlogPostRequest.Id);
            if (deletedBlogPost != null)
            {
                return RedirectToAction("List");
            }


             return RedirectToAction("Delete");
            
        }
    }


}
