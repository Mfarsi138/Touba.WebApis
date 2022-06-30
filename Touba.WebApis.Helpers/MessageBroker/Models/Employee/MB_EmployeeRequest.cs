using System;
using System.Collections.Generic;
using System.Text;

namespace Touba.WebApis.Helpers.MessageBroker.Models.Employee
{
    public class MB_EmployeeRequest
    {
        public string UserId { get; set; }
    }
    public class MB_EmployeeListRequest
    {
        public string filter { get; set; }
    }
    public class MB_GetEmployeeListResponse
    {
        private List<MB_EmployeeResponse> _Rows = new List<MB_EmployeeResponse>();
        public List<MB_EmployeeResponse> Rows
        {
            get { return _Rows; }
            set { _Rows = value; }
        }
    }

    public class MB_EmployeeResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public long? NationalityId { get; set; }
        public string UserId { get; set; }
        public long? OrganizationalLevel1Id { get; set; }
        public long? OrganizationalLevel2Id { get; set; }
        public long? OrganizationalLevel3Id { get; set; }
        public long? OrganizationalLevel4Id { get; set; }
        public string FatherName { get; set; }
        public string EmirateId { get; set; }
    }
}
