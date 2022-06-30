using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Touba.WebApis.Helpers.MessageBroker.Models.Todo
{
    public class MB_UUserData
    {
        public long Id { get; set; }
        public int? State { get; set; }
        public string UserId { get; set; }
        public DateTime? VisitDate { get; set; }

    }
}
