using EconomicManagementAPP.Models;
using EconomicManagementAPP.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EconomicManagementAPP.Controllers
{
    public class AccountTypesController : Controller
    {
        private readonly IRepositorieAccountTypes repositorieAccountTypes;

        public AccountTypesController(IRepositorieAccountTypes repositorieAccountTypes)
        {
            this.repositorieAccountTypes = repositorieAccountTypes;
        }
        public async Task<IActionResult> Index()
        {
            string login = HttpContext.Session.GetString("user");
            if (login != null)
            {

                var accountTypes = await repositorieAccountTypes.getAccounts();
                return View(accountTypes);
            }
            
            return RedirectToAction("Login","User");
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccountTypes accountTypes)
        {
            if (!ModelState.IsValid)
            {
                return View(accountTypes);
            }

            accountTypes.UserId = (int)HttpContext.Session.GetInt32("user");
            accountTypes.OrderAccount = 1;

            var accountTypeExist =
               await repositorieAccountTypes.Exist(accountTypes.Name, accountTypes.UserId);

            if (accountTypeExist)
            {
                ModelState.AddModelError(nameof(accountTypes.Name),
                    $"The account {accountTypes.Name} already exist.");

                return View(accountTypes);
            }
            await repositorieAccountTypes.Create(accountTypes);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> VerificaryAccountType(string Name)
        {
            var UserId = 1;
            var accountTypeExist = await repositorieAccountTypes.Exist(Name, UserId);

            if (accountTypeExist)
            {
                return Json($"The account {Name} already exist");
            }

            return Json(true);
        }

        [HttpGet]
        public async Task<ActionResult> Update(int id)
        {
            var userId = 1;
            var accountType = await repositorieAccountTypes.getAccountById(id, userId);

            if (accountType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(accountType);
        }
        [HttpPost]
        public async Task<ActionResult> Update(AccountTypes accountTypes)
        {
            var userId = 1;
            var accountType = await repositorieAccountTypes.getAccountById(accountTypes.Id, userId);

            if (accountType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            await repositorieAccountTypes.Update(accountTypes);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = 1;
            var account = await repositorieAccountTypes.getAccountById(id, userId);

            if (account is null)
            {
                return RedirectToAction("NotFount", "Home");
            }

            return View(account);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var userId = 1;
            var account = await repositorieAccountTypes.getAccountById(id, userId);

            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await repositorieAccountTypes.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
