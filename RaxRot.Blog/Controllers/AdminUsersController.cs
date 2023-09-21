﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RaxRot.Blog.Models.ViewModels;
using RaxRot.Blog.Repositories;

namespace RaxRot.Blog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminUsersController(IUserRepository userRepository,
            UserManager<IdentityUser>userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
           var users = await _userRepository.GetAll();
            var usersViewModel = new UserViewModel();
            usersViewModel.Users = new List<User>();

            foreach (var user in users)
            {
                usersViewModel.Users.Add(new User
                {
                    Id=Guid.Parse(user.Id),
                    UserName=user.UserName,
                    EmailAddress=user.Email
                });
            }
            return View(usersViewModel);
        }

        [HttpPost]
        public async Task<IActionResult>List(UserViewModel request)
        {
            var identityUser = new IdentityUser
            {
                UserName = request.UserName,
                Email = request.Email
            };

           var identityResult =
                await _userManager.CreateAsync(identityUser,request.Password);

            if (identityResult is not null)
            {
                if (identityResult.Succeeded)
                {
                    //asign rioles
                    var roles = new List<string> { "User" };
                    if (request.AdminRoleCheckBox)
                    {
                        roles.Add("Admin");
                    }

                   identityResult = 
                        await _userManager.AddToRolesAsync(identityUser, roles);

                    if (identityResult is not null && identityResult.Succeeded)
                    {
                        return RedirectToAction("List", "AdminUsers");
                    }
                }
            }

            return View();   
        }

        [HttpPost]
        public async Task<IActionResult>Delete(Guid id)
        {
           var user = await _userManager.FindByIdAsync(id.ToString());
            if (user != null)
            {
              var identityResult =  await _userManager.DeleteAsync(user);
                if (identityResult is not null)
                {
                    if (identityResult.Succeeded)
                    {
                        return RedirectToAction("List","AdminUsers");
                    }
                }
            }
            return View();
        }
    }
}
