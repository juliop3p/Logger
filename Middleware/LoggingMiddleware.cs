using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Log.Models;
using Log.Service;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Log.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Logger _logger;
        private readonly List<string> _ignoredExtensions = new List<string>
        {
            ".ico",
            ".css",
            ".js",
            ".jpg",
            ".png",
            ".gif",
            ".svg"
        };

        public LoggingMiddleware(RequestDelegate next, Logger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var startTime = DateTime.Now;
            _logger.SetInitialTimestamp(startTime);
            
            if (!ShouldLogRequest(context))
            {
                await _next(context);
            }
            
            // Captura detalhes da requisição
            var request = context.Request;
            var requestDetails = new RequestDetails
            {
                Method = request.Method,
                Url = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}",
                Headers = request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()),
                Timestamp = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss:ff")
            };
            _logger.SetRequestDetails(requestDetails);

            // Captura detalhes da resposta
            var originalResponseBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                // Chama o próximo middleware na pipeline
                await _next(context);

                // Lê a resposta e transforma em string para log
                responseBody.Seek(0, SeekOrigin.Begin);
                var responseText = await new StreamReader(responseBody).ReadToEndAsync();
                responseBody.Seek(0, SeekOrigin.Begin);

                // Cria os detalhes da resposta para log
                var responseDetails = new ResponseDetails
                {
                    StatusCode = context.Response.StatusCode,
                    Headers = context.Response.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()),
                    //Body = responseText, // Inclui o corpo da resposta no log
                    Timestamp = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss:ff")
                };

                _logger.SetResponseDetails(responseDetails);

                // Copia a resposta de volta para o fluxo original
                await responseBody.CopyToAsync(originalResponseBodyStream);
            }

            // Finaliza e salva o log
            var endTime = DateTime.Now;
            _logger.SetEndTimestamp(endTime);
            _logger.SetDuration(startTime, endTime);
            _logger.SaveLog();
        }
        
        private bool ShouldLogRequest(HttpContext context)
        {
            // Verifica se a requisição não é para um arquivo estático
            bool isStaticFileRequest = _ignoredExtensions.Any(ext => context.Request.Path.Value.EndsWith(ext, StringComparison.OrdinalIgnoreCase));

            return !isStaticFileRequest;
        }
    }
}