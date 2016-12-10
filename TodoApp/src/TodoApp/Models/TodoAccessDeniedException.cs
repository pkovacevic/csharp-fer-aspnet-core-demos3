using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApp.Models
{
    public class TodoAccessDeniedException : Exception
    {
        public TodoAccessDeniedException(Guid userId, Guid todoId, Exception innerException = null) : base($"User with ID: {userId} tried to access {todoId} without permission.", innerException)
        {

        }
    }
}
