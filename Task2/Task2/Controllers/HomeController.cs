using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Task2.Models;

namespace Task2.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public HomeController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user.Block)
            {
                await _signInManager.SignOutAsync();
                user.Check = false;
                user.Status = false;
                IdentityResult result = await _userManager.UpdateAsync(user);
            }
            if(user == null)
            {
                await _signInManager.SignOutAsync();
            }
            var model = _userManager.Users.ToList();
            
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Index(List<User> model, string s1)
        {
            IdentityResult result = null;
            for (int i = 0; i < model.Count; i++)
            {
                if (model[i].Check)
                {
                    if (s1 == "Delete") 
                    {
                        User user = await _userManager.FindByNameAsync(model[i].Login);
                        if (user != null)
                        {
                            if (User.Identity.Name == user.UserName)
                            {
                                await _signInManager.SignOutAsync();
                            }
                            result = await _userManager.DeleteAsync(user);
                        }
                        else
                        {
                            ModelState.AddModelError("", "Пользователь не найден");
                        }
                    }
                    if (s1 == "Block")
                    {
                        User user = await _userManager.FindByNameAsync(model[i].Login);
                        if (user != null)
                        {
                            user.Block = true;
                            user.Status = false;
                            result = await _userManager.UpdateAsync(user);
                            if (result.Succeeded)
                            {
                                if (User.Identity.Name == user.UserName)
                                {
                                    await _signInManager.SignOutAsync();
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("", "Что-то пошло не так");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Пользователь не найден");
                        }
                    }
                    if (s1 == "UnBlock")
                    {
                        User user = await _userManager.FindByNameAsync(model[i].Login);
                        if (user != null)
                        {
                            user.Block = false;
                            result = await _userManager.UpdateAsync(user);
                            if (result.Succeeded)
                            {
                                
                            }
                            else
                            {
                                ModelState.AddModelError("", "Что-то пошло не так");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Пользователь не найден");
                        }
                    }
                }
            }
            return RedirectToAction("Index");
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