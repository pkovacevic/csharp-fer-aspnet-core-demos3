using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Interfaces;
using TodoApp.Models;
using TodoApp.Models.TodoViewModels;

namespace TodoApp.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly ITodoRepository _todoRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public TodoController(ITodoRepository todoRepository, UserManager<ApplicationUser> userManager)
        {
            _todoRepository = todoRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = await GetCurrentUserId();
            var todos = _todoRepository.GetActive(userId);

            return View(todos);
        }

        public async Task<IActionResult> Completed()
        {
            var userId = await GetCurrentUserId();
            var todos = _todoRepository.GetCompleted(userId);

            return View(todos);
        }

        public async Task<IActionResult> MarkAsCompleted(Guid todoId)
        {
            var userId = await GetCurrentUserId();
            _todoRepository.MarkAsCompleted(todoId, userId);
            return RedirectToAction("Index");
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddTodoViewModel model)
        {
            if (ModelState.IsValid)
            {
                _todoRepository.Add(new TodoItem(model.Text, await GetCurrentUserId()));
                return RedirectToAction("Index");
            }
            return View(model);
        }



        /// <summary>
        /// Gets Guid of the currently logged user. Think about pulling this helper method outside,
        /// so other controllers can use it!
        /// </summary>
        private async Task<Guid> GetCurrentUserId()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            return new Guid(user.Id);
        }
    }
}