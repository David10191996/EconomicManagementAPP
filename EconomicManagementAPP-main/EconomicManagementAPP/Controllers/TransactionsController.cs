using Microsoft.AspNetCore.Mvc;
using EconomicManagementAPP.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EconomicManagementAPP.Models;

namespace EconomicManagementAPP.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly IRepositorieTransactions repositorieTransactions;
        public TransactionsController(IRepositorieTransactions repositorieTransactions)
        {
            this.repositorieTransactions = repositorieTransactions;
        }
        public async Task<IActionResult> Index()
        {
            string login = HttpContext.Session.GetString("user");
            if (login != null)
            {
                var transactions = await repositorieTransactions.getTransactions();
                return View(transactions);
            }
            return RedirectToAction("Login", "User");
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Transactions transactions)
        {
            if (!ModelState.IsValid)
            {
                return View(transactions);
            }
            transactions.TransactionDate = DateTime.Now;

            await repositorieTransactions.Create(transactions);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Update(int Id)
        {
            var transaction = await repositorieTransactions.getTransactionsById(Id);

            if (transaction is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(transaction);
        }

        [HttpPost]
        public async Task<ActionResult> Update(Transactions transactions)
        {
            var transaction = await repositorieTransactions.getTransactionsById(transactions.Id);

            if (transaction is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            await repositorieTransactions.Update(transactions);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            var transaction = await repositorieTransactions.getTransactionsById(Id);

            if (transaction is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            return View(transaction);
        }
        public async Task<IActionResult> DeleteTransaction(int Id)
        {
            var transaction = await repositorieTransactions.getTransactionsById(Id);

            if (transaction is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await repositorieTransactions.Delete(Id);
            return RedirectToAction("Index");
        }
    }
}
