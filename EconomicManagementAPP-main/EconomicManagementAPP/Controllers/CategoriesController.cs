using EconomicManagementAPP.Models;
using EconomicManagementAPP.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EconomicManagementAPP.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IRepositorieCategories repositorieCategories;


        public CategoriesController(IRepositorieCategories repositorieCategories)
        {
            this.repositorieCategories = repositorieCategories;
        }
        public async Task<IActionResult> Index()
        {
            string login = HttpContext.Session.GetString("user");
            if (login != null)
            {
                var categories = await repositorieCategories.getCategories();
                return View(categories);
            }
            return RedirectToAction("Login", "User");
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Categories categories)
        {
            if (!ModelState.IsValid)
            {
                return View(categories);
            }
            var categoriesExist =
               await repositorieCategories.Exist(categories.Name);

            if (categoriesExist)
            {
                ModelState.AddModelError(nameof(categories.Name),
                    $"The categorie {categories.Name} already exist.");

                return View(categories);
            }
            await repositorieCategories.Create(categories);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> VerificaryCategorie(string Name)
        {
            var categoriesExist = await repositorieCategories.Exist(Name);

            if (categoriesExist)
            {
                return Json($"The categories {Name} already exist");
            }
            return Json(true);
        }

        [HttpGet]
        public async Task<ActionResult> Update(int Id)
        {
            var categorie = await repositorieCategories.getCategoriesById(Id);

            if (categorie is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(categorie);
        }

        [HttpPost]
        public async Task<ActionResult> Update(Categories categories)
        {
            var categorie = await repositorieCategories.getCategoriesById(categories.Id);

            if (categorie is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            await repositorieCategories.Update(categories);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            var categorie = await repositorieCategories.getCategoriesById(Id);

            if (categorie is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            return View(categorie);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCategories(int Id)
        {
            var categorie = await repositorieCategories.getCategoriesById(Id);

            if (categorie is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            await repositorieCategories.Delete(Id);
            return RedirectToAction("Index");
        }
    }
}
