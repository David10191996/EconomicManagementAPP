using EconomicManagementAPP.Models;
using Microsoft.AspNetCore.Mvc;
using EconomicManagementAPP.Interfaces;

namespace EconomicManagementAPP.Controllers
{
    public class OperationTypesController : Controller
    {
        private readonly IRepositorieOperationTypes repositorieOperationTypes;
        public OperationTypesController(IRepositorieOperationTypes repositorieOperationTypes)
        {
            this.repositorieOperationTypes = repositorieOperationTypes;
        }
        public async Task<IActionResult> Index()
        {
            string login = HttpContext.Session.GetString("user");
            if (login != null)
            {
                var operationTypes = await repositorieOperationTypes.getOperationTypes();
                return View(operationTypes);
            }
            return RedirectToAction("Login", "User");
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(OperationTypes operationTypes)
        {
            if (!ModelState.IsValid)
            {
                return View(operationTypes);
            }
            var operationTypeExist =
               await repositorieOperationTypes.Exist(operationTypes.Description);

            if (operationTypeExist)
            {
                ModelState.AddModelError(nameof(operationTypes.Description),
                $"The operation types {operationTypes.Description} already exist.");

                return View(operationTypes);
            }
            await repositorieOperationTypes.Create(operationTypes);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> VerificaryOperationType(string Description)
        {
            var operationType = await repositorieOperationTypes.Exist(Description);

            if (operationType)
            { 
                return Json($"The Operation {Description} already exist");
            }
            return Json(true);
        }

        [HttpGet]
        public async Task<ActionResult> Update(int Id)
        {
            var operationType = await repositorieOperationTypes.getOperationTypesById(Id);

            if (operationType is null)
            {

                return RedirectToAction("NotFound", "Home");
            }

            return View(operationType);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            var operation = await repositorieOperationTypes.getOperationTypesById(Id);

            if (operation is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(operation);
        }

        [HttpPost]
        public async Task<ActionResult> Update(OperationTypes operationTypes)
        {
            var operationType = await repositorieOperationTypes.getOperationTypesById(operationTypes.Id);

            if (operationType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await repositorieOperationTypes.Update(operationTypes);
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> DeleteOperationType(int Id)
        {
            var operation = await repositorieOperationTypes.getOperationTypesById(Id);

            if (operation is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await repositorieOperationTypes.Delete(Id);
            return RedirectToAction("Index");
        }


    }
}
