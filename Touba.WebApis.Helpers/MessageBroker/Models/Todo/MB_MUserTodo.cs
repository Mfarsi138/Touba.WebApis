using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Touba.WebApis.Helpers.MessageBroker.Models.Todo
{
    public class MB_MUserTodoRequest
    {
        public string UserId { get; set; }
        public long Entity { get; set; }
        public long EntityId { get; set; }
        public string ExtraData { get; set; }
        public long Status { get; set; }        
    }
    public class MB_MToodResponse
    {
        public long TodoId { get; set; }
    }
}
