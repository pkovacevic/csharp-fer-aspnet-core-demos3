using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Interfaces;
using TodoApp.Models;

namespace TodoApp.Components
{
    public class LastTodosComponent : ViewComponent
    {
        private readonly ITodoRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public LastTodosComponent(ITodoRepository repository, UserManager<ApplicationUser> userManager )
        {
            _repository = repository;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(int n)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            // Good enough for demo.. but "getting all" from DB which is expensive
            // just to ditch everything except first n.. not so smart
            return View(_repository.GetLastTodos(n));
        }



    }
}
