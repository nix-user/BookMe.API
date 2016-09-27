using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BookMe.WebApi
{
    public class XHttpMethodDelegatingHandler : DelegatingHandler
    {
        private static readonly string[] AllowedHttpMethods = { "PUT", "DELETE" };
        private static readonly string HttpMethodHeader = "X-HTTP-Method";

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Method == HttpMethod.Post && request.Headers.Contains(HttpMethodHeader))
            {
                string httpMethod = request.Headers.GetValues(HttpMethodHeader).FirstOrDefault();
                if (AllowedHttpMethods.Contains(httpMethod, StringComparer.InvariantCultureIgnoreCase))
                {
                    request.Method = new HttpMethod(httpMethod);
                }
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}