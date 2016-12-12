using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Interfaces;
using TodoApp.Models;

namespace TodoApp
{
    public class TodoSqlRepository : ITodoRepository
    {
        private readonly TodoDbContext _context;

        public TodoSqlRepository(TodoDbContext context)
        {
            _context = context;
        }

        public TodoItem Get(Guid todoId, Guid userId)
        {
            var todo = _context.TodoItems.FirstOrDefault(t => t.Id == todoId);
            if (todo != null)
            {
                if (todo.UserId != userId)
                {
                    throw new TodoAccessDeniedException(userId: userId, todoId: todoId);
                }
                return todo;
            }

            return null;
        }

        public void Add(TodoItem todoItem)
        {
            _context.TodoItems.Add(todoItem);
            _context.SaveChanges();
        }

        public bool Remove(Guid todoId, Guid userId)
        {
            var todo = Get(todoId, userId);
            if (todo != null)
            {
                _context.TodoItems.Remove(todo);
                return true;
            }

            return false;
        }

        public void Update(TodoItem todoItem, Guid userId)
        {
            if (todoItem.UserId != userId)
            {
                throw new TodoAccessDeniedException(userId: userId, todoId: todoItem.Id);
            }

            // Setting state of the TodoItem as "modified" marks object as "dirty" inside our context.
            // Because of that, when we call SaveChanges(), EF will have to update all properties of todoItem object inside DB 
            // What row it will actually update depends on the key {todoItem.Id}
            _context.Entry(todoItem).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public bool MarkAsCompleted(Guid todoId, Guid userId)
        {
            var todo = Get(todoId, userId);
            if (todo != null)
            {
                todo.MarkAsCompleted();
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<TodoItem> GetAll(Guid userId)
        {
            return _context.TodoItems.Where(t => t.UserId == userId).ToList();
        }

        public List<TodoItem> GetActive(Guid userId)
        {
            return _context.TodoItems.Where(t => t.UserId == userId && t.IsCompleted == false).ToList();
        }

        public List<TodoItem> GetCompleted(Guid userId)
        {
            return _context.TodoItems.Where(t => t.UserId == userId && t.IsCompleted).ToList();
        }

        public List<TodoItem> GetFiltered(Func<TodoItem, bool> filterFunction, Guid userId)
        {
            return _context.TodoItems.Where(t => t.UserId == userId).Where(filterFunction).ToList();
        }

        public List<TodoItem> GetLastTodos(int n)
        {
            return _context.TodoItems.OrderBy(t => t.DateCreated).Take(n).ToList();
        }
    }
}
