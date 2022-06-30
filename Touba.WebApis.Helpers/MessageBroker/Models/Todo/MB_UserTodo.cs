using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Touba.WebApis.Helpers.MessageBroker.Models.Todo
{
    public class MB_UserTodoRequest
    {
        public string UserId { get; set; }
        public long Entity { get; set; }
    }
    public class MB_UserTodoResponse
    {
        private List<MBTodo> _UserTodo = new List<MBTodo>();
        public List<MBTodo> UserTodo
        {
            get { return _UserTodo; }
            set { _UserTodo = value; }
        }
    }

    public class MBTodo
    {
        public long EntityId { get; set; }
        public string ExtraData { get; set; }
        public long Status { get; set; }
    }
}
