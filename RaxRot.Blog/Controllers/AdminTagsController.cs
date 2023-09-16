using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaxRot.Blog.Data;
using RaxRot.Blog.Models.Domain;
using RaxRot.Blog.Models.ViewModels;
using RaxRot.Blog.Repositories;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace RaxRot.Blog.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly ITagRepository _tagRepository;

        public AdminTagsController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var allTags = await _tagRepository.GetAllAsync();
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


            await _tagRepository.AddAsync(tag);


            return RedirectToAction("List");
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var tag = await _tagRepository.GetAsync(id);
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
        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> UpdateTag(EditTagRequest editTagRequest)
        {
            var tag = new Tag()
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName
            };

            var updatedTag = await _tagRepository.UpdateAsync(tag);
            if (updatedTag != null)
            {
                // Show success
                return RedirectToAction("List");
            }
            else
            {
                // Show error
                ModelState.AddModelError("", "Tag not found or could not be updated.");
                return View(editTagRequest); // Return to the edit view with the input values
            }
        }


        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var tag = await _tagRepository.GetAsync(id);
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

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteTag(DeleteTagRequest deleteTagRequest)
        {

            var deletedTag = await _tagRepository.DeleteAsync(deleteTagRequest.Id);
            if (deletedTag != null)
            {
                //Success
                return RedirectToAction("List");
            }
            return View();
        }
    }
}
