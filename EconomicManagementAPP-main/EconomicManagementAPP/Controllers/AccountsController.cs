using EconomicManagementAPP.Interfaces;
using EconomicManagementAPP.Models;
using Microsoft.AspNetCore.Mvc;

namespace EconomicManagementAPP.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IRepositorieAccounts repositorieAccounts;

        public AccountsController(IRepositorieAccounts repositorieAccounts)
        {
            this.repositorieAccounts = repositorieAccounts;
        }
        public async Task<IActionResult> Index()
        {
            string login = HttpContext.Session.GetString("user");
            if (login != null)
            {
                var accounts = await repositorieAccounts.getAccounts();
                return View(accounts);
            }
            return RedirectToAction("Login", "User");
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Accounts accounts)
        {
            if (!ModelState.IsValid)
            {
                return View(accounts);
            }
            var accountExist =
               await repositorieAccounts.Exist(accounts.Name);

            if (accountExist)
            {
                ModelState.AddModelError(nameof(accounts.Name),
                    $"The account {accounts.Name} already exist.");

                return View(accounts);
            }
            await repositorieAccounts.Create(accounts);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> VerificaryAccount(string Name)
        {
            var accountExist = await repositorieAccounts.Exist(Name);

            if (accountExist)
            {
                return Json($"The account {Name} already exist");
            }
            return Json(true);
        }
        [HttpGet]
        public async Task<IActionResult> Update(int Id)
        {
            var account = await repositorieAccounts.getAccountsById(Id);

            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            return View(account);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Accounts accounts)
        {
            var account = await repositorieAccounts.getAccountsById(accounts.Id);

            if (accounts is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            await repositorieAccounts.Update(accounts);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            var account = await repositorieAccounts.getAccountsById(Id);

            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            return View(account);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAccount(int Id)
        {
            var account = await repositorieAccounts.getAccountsById(Id);

            if (account is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            await repositorieAccounts.Delete(Id);
            return RedirectToAction("Index");
        }
    }
}
