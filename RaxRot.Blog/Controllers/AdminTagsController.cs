using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaxRot.Blog.Data;
using RaxRot.Blog.Models.Domain;
using RaxRot.Blog.Models.ViewModels;

namespace RaxRot.Blog.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public AdminTagsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var allTags= await _dbContext.Tags.ToListAsync();
            return View(allTags);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost, ActionName("Add")]
        public async Task<IActionResult> AddTag(AddTagRequest addTagRequest)
        {
            var tag = new Tag()
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };


            await _dbContext.Tags.AddAsync(tag);
            await _dbContext.SaveChangesAsync();


            return RedirectToAction("List");
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var tag=await _dbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
            if (tag != null) 
            {
                var editTagRequest = new EditTagRequest()
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };

                return View(editTagRequest);
            }

            return View();
        }

        [HttpPost,ActionName("Edit")]
        public IActionResult UpdateTag(EditTagRequest editTagRequest)
        {
            var tag = new Tag()
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName
            };

            var existingTag = _dbContext.Tags.Find(tag.Id);

            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;


                _dbContext.SaveChanges();

                return RedirectToAction("List");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var tag = await _dbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
            if (tag != null)
            {
                var deleteTagRequest = new DeleteTagRequest()
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };

                return View(deleteTagRequest);
            }

            return View();
        }

        [HttpPost,ActionName("Delete")]
        public async Task<IActionResult> DeleteTag(DeleteTagRequest deleteTagRequest)
        {
            var tag= await _dbContext.Tags.FirstOrDefaultAsync(x=>x.Id==deleteTagRequest.Id);
            if(tag != null)
            {
                _dbContext.Remove(tag);
                _dbContext.SaveChanges();

               return RedirectToAction("List");
            }
            return View();
        }
    }
}
