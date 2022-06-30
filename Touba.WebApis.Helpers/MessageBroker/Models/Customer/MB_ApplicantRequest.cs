using System;
using System.Collections.Generic;
using System.Text;

namespace Touba.WebApis.Helpers.MessageBroker.Models.Customer
{
    public class MB_ApplicantRequest
    {
        private List<string> _ApplicantUserId = new List<string>();
        public List<string> ApplicantUserId
        {
            get { return _ApplicantUserId; }
            set { _ApplicantUserId = value; }
        }
    }

}
