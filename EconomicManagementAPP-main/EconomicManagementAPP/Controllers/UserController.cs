using EconomicManagementAPP.Models;
using Microsoft.AspNetCore.Mvc;
using EconomicManagementAPP.Interfaces;
using Microsoft.AspNetCore.Http;

namespace EconomicManagementAPP.Controllers
{
    public class UserController : Controller 
    {
        private readonly IRepositorieUser repositorieUser;
        public UserController(IRepositorieUser repositorieUser)  
        {
            this.repositorieUser = repositorieUser;
        }

        public async Task<IActionResult> Index() 
        {
            var users = await repositorieUser.getUser();
            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User users) 
        {
            if (!ModelState.IsValid)
            {
                return View(users);
            }
            var userExist = await repositorieUser.Exist(users.Email);

            if (userExist)
            {
                ModelState.AddModelError(nameof(users.Email),
                    $"The Account {users.Email} Already Exist.");
                return View(users);
            }

            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(users.Pass);
            users.Pass = Convert.ToBase64String(encryted);

            await repositorieUser.Create(users);
            
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> VerificaryUser(string Email)
        {
            var userExist = await repositorieUser.Exist(Email);

            if (userExist)
            {
                return Json($"The Email {Email} is already registered");
            }
            return Json(true);
        }

        [HttpGet]
        public async Task<ActionResult> UpdateUser(int Id)
        {
            var user = await repositorieUser.getUserById(Id);

            if (user is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(user);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            var user = await repositorieUser.getUserById(Id);
            if (user is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateUser(User users)
        {
            var user = await repositorieUser.getUserById(users.Id);
            if (user is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(users.Pass);
            users.Pass = Convert.ToBase64String(encryted);

            await repositorieUser.UpdateUser(users);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(int Id)
        {
            var user = await repositorieUser.getUserById(Id);

            if (user is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            await repositorieUser.Delete(Id);

            return RedirectToAction("Index");

            
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            var result = await repositorieUser.Login(loginViewModel.Email, loginViewModel.Password);

            if (result is null)
            {
                ModelState.AddModelError(String.Empty, "Wrong Email Or Password");
                return View(loginViewModel); 
            }
            else
            {
                HttpContext.Session.SetInt32("user", result.Id);
                return RedirectToAction("Index", "Accounts");
            }

        }

        [HttpGet]
        public IActionResult LogOut(User user)
        {
            string login = HttpContext.Session.GetString("user");
            if (login != null)
            {
                return View(user);
            }
            return RedirectToAction("Login", "Users");
        }

        [HttpPost]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
