using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Touba.WebApis.Helpers.Helpers
{
    public static class HttpHelper
    {
        public static async Task<string> GetRequestBodyAsync(HttpRequest request, int bufferSize = 1024)
        {
            request.Body.Position = 0;
            using var reader = new StreamReader(request.Body,
                                                 encoding: Encoding.UTF8,
                                                 detectEncodingFromByteOrderMarks: false,
                                                 bufferSize: bufferSize,
                                                 leaveOpen: true);
            return await reader.ReadToEndAsync();
        }
    }
}
