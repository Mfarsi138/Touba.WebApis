using System.Linq;
using System.Reflection;

namespace Touba.WebApis.Helpers.Helpers
{
    /// <summary>
    /// Contains custom HTTP status codes that are not defined in the HTTP Specification.
    /// Codes are categorized according to the HTTP specification (RFC 2616) to the following classes:
    /// 2xx: Success        - The action was successfully received, understood, and accepted
    /// 3xx: Redirection    - Further action must be taken in order to complete the request
    /// 4xx: Client Error   - The request contains bad syntax or cannot be fulfilled
    /// 5xx: Server Error   - The server failed to fulfill an apparently valid request
    /// 
    /// Some of these codes are assigned, when adding a specific code make sure to add to the unassigned codes, which are:
    /// Success             - 209-225, 227-299
    /// Redirection         - 309-399
    /// Client Error        - 418-420, 432-450, 452-499
    /// Server Error        - 512-599
    /// 
    /// Use 600+ for custom business (DAB) related errors and responses.
    /// 
    /// Take a look at CustomStatus.Success class for an example on adding new custom statuses.
    /// 
    /// <example>
    /// Usage example in Controller:
    /// <code>
    /// return StatusCode(CustomStatus.Success.SuccessCustomStatus.Code, CustomStatus.Success.SuccessCustomStatus.Message);
    /// </code>
    /// </example>
    /// </summary>
    public static class CustomStatus
    {
        #region Success Custom Codes

        /// <summary>
        /// Success custom status codes
        /// </summary>
        /// <value> 209-225, 227-299 </value>
        public class Success
        {
            public static CustomStatusCode SuccessCharityBoxStatus = new CustomStatusCode(211, "Success charity box Status Response Message");
        }

        #endregion

        #region Redirection Custom Codes

        /// <summary>
        /// Redirection custom status codes
        /// </summary>
        /// <value> 309-399 </value>
        public class Redirection
        {
            public static CustomStatusCode RedirectCharityBoxStatus = new CustomStatusCode(310, "Redirect charity box Status Response Message");
        }

        #endregion

        #region Client Error Custom Codes

        /// <summary>
        /// Client Error custom status codes
        /// </summary>
        /// <value> 418-420, 432-450, 452-499 </value>
        public class ClientError
        {
            /* CharityBox */

            public static CustomStatusCode ClientErrorCharityBoxStatus = new CustomStatusCode(419, "ClientError charity box Status Response Message");
            public static CustomStatusCode BadRequestCharityBoxStatus = new CustomStatusCode(420, "BadRequest charity box Status Response Message");
            public static CustomStatusCode NotFoundCharityBoxStatus = new CustomStatusCode(421, "NotFound charity box Status Response Message");


            /* Zaka */

            /// <summary>
            /// Returned if the User Id sent in request is not an assessor when it should be
            /// </summary>
            public static CustomStatusCode InvalidAssessor = new CustomStatusCode(422, "Invalid user or user is not an assessor");

            /// <summary>
            /// Returned if no assessors were found, for a request that requires at least a single assessor
            /// </summary>
            public static CustomStatusCode NoAssessors = new CustomStatusCode(423, "No assessors were found");
        }

        #endregion

        #region Server Error Custom Codes

        /// <summary>
        /// Server Error custom status codes
        /// </summary>
        /// <value> 512-599 </value>
        public class ServerError
        {
            public static CustomStatusCode ServerErrorCharityBoxStatus = new CustomStatusCode(513, "ServerError charity box Status Response Message");

        }

        #endregion

        #region DAB Business Custom Codes

        /// <summary>
        /// Business related custom status codes
        /// </summary>
        /// <value> 600+ </value>
        public class DAB
        {
        }

        #endregion

        /// <summary>
        /// Returns an assigned custom code's message
        /// </summary>
        /// <param name="statusCode">The numeric custom status code</param>
        /// <returns>The assigned custom code's alphanumeric message</returns>
        public static string GetCodeMessage(int statusCode)
        {
            var statusCategories = typeof(CustomStatus).GetNestedTypes();

            foreach (var statusCategory in statusCategories)
            {
                // Get the CustomStatusCode field
                var statusCodeMessage = (CustomStatusCode) statusCategory.GetFields(BindingFlags.Public | BindingFlags.Static)
                    .Where(c => ((CustomStatusCode) c.GetValue(null)).Code == statusCode).FirstOrDefault().GetValue(null);

                // Check if it has a message
                if (statusCodeMessage != null && !string.IsNullOrEmpty(statusCodeMessage.Message)) return statusCodeMessage.Message;
                else continue;
            }

            return null;
        }
    }

    public class CustomStatusCode
    {
        public readonly int  Code;
        public readonly string Message;

        public CustomStatusCode(int Code, string Message)
        {
            this.Code = Code;
            this.Message = Message;
        }
    }

}
