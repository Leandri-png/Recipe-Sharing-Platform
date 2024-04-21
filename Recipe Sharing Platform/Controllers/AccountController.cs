using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Recipe_Sharing_Platform.Models;
using Recipe_Sharing_Platform.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipe_Sharing_Platform.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RemeberMe, false);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Invalid Login Attempt");
                }
                else
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

            }

            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserName = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Gender = model.Gender
                };

                var result = await userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    foreach (var errors in result.Errors)
                    {
                        ModelState.AddModelError("", errors.Description);
                    }
                }
                else
                {

                    await signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");

                }
            }

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            // Get the currently signed-in user
            var user = await userManager.GetUserAsync(User);

            // Create a ProfileViewModel and populate it with user information
            var profileViewModel = new ProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender.ToString() // Use the null conditional operator to avoid NullReferenceException
            };

            return View(profileViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GeneralInfo()
        {
            var user = await userManager.GetUserAsync(User);

            GeneralInfoViewModel model = new GeneralInfoViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender
            };

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> GeneralInfo(GeneralInfoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);

                
                user.PhoneNumber = model.PhoneNumber;
                user.UserName = model.Email;
                user.Email = model.Email;
           
               

                //// Fetch the existing user from the UserManager
                //var existingUser = await userManager.FindByIdAsync(user.Id);

                //// Update the user's security stamp
                //user.SecurityStamp = existingUser.SecurityStamp;

                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    if (model.NewPassword != null)
                    {
                        result = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

                        if (result.Succeeded)
                        {
                            return View("DataIsChanged");
                        }
                    }

                    return View("DataIsChanged");

                }

                foreach (var erros in result.Errors)
                {
                    ModelState.AddModelError("", erros.Description);
                }

            }
            return View(model);
        }

    }
}
