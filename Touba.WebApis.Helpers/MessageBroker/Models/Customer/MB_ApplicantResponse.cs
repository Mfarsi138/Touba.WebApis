using System;
using System.Collections.Generic;
using System.Text;

namespace Touba.WebApis.Helpers.MessageBroker.Models.Customer
{
    public class MB_ApplicantResponse
    {
        private List<ApplicantData> _Applicant = new List<ApplicantData>();
        public List<ApplicantData> Applicant
        {
            get { return _Applicant; }
            set { _Applicant= value; }
        }
    }
    public class ApplicantData
    {
        public string userID { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
    }
}
