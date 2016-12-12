using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using TodoApp.Interfaces;

namespace TodoApp.Controllers
{
    public class TodoApi : Controller
    {
        private readonly ITodoRepository _repository;

        public TodoApi(ITodoRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Get()
        {
            var result = _repository.GetLastTodos(10);

            return new ObjectResult(result);
        }

    }
}
