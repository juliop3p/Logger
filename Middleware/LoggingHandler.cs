using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Log.Models;
using Log.Service;

namespace Log.Middleware
{
    public class LoggingHandler : DelegatingHandler
    {
        private readonly Logger _logger;
        
        public LoggingHandler(Logger logger)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var startTime = DateTime.Now;
            // Captura o request
            var requestExternal = new RequestDetails()
            {
                Url = request.RequestUri.ToString(),
                Timestamp = startTime.ToString(),
                Method = request.Method.ToString(),
            };

            var externalApiRequest = new ExternalApiCall()
            {
                ApiName = request.RequestUri.AbsolutePath,
                Request = requestExternal
            };

            // Chama o próximo handler na cadeia
            var response = await base.SendAsync(request, cancellationToken);

            // Captura a resposta, se necessário
            var endTime = DateTime.Now;
            var responseExternal = new ResponseDetails()
            {
                StatusCode = (int)response.StatusCode,
                Timestamp = endTime.ToString(),
                DurationMs = (int)(endTime - startTime).TotalMilliseconds,
            };
            externalApiRequest.Response = responseExternal;
            
            _logger.SetExternalApiRequestAndResponse(externalApiRequest);

            return response;
        }
    }
}