using System;
using Log.Models;
using Newtonsoft.Json;
using Formatting = System.Xml.Formatting;

namespace Log.Service
{
    public class Logger
    {
        private LogEntry _logEntry;

        public Logger()
        {
            _logEntry = new LogEntry
            {
                RequestDetails = new RequestDetails { Timestamp = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss:ff") },
                LogTitle = "API Log"
            };
        }

        // Método para capturar a requisição (interno ao middleware)
        public void SetRequestDetails(RequestDetails request)
        {
            _logEntry.RequestDetails = request;
        }

        // Método para capturar a resposta (interno ao middleware)
        public void SetResponseDetails(ResponseDetails response)
        {
            _logEntry.ResponseDetails = response;
        }

        public void SetInitialTimestamp(DateTime initialTimestamp)
        {
            _logEntry.InitialTimestamp = initialTimestamp.ToString("dd/MM/yyyy HH:mm:ss:ff");
        }
        
        public void SetEndTimestamp(DateTime endTimestamp)
        {
            _logEntry.EndTimestamp = endTimestamp.ToString("dd/MM/yyyy HH:mm:ss:ff");
        }

        public void SetDuration(DateTime startTime, DateTime endTime)
        {
            _logEntry.Duration = (int)(endTime - startTime).TotalMilliseconds;
        }

        // Método para o desenvolvedor adicionar logs customizados
        public void AddCustomLog(string stepName, string message)
        {
            _logEntry.Steps.Add(new Steps
            {
                StepName = stepName,
                Message = message,
                Timestamp = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss:ff")
            });
        }
        
        
        public void SetExternalApiRequestAndResponse(ExternalApiCall externalApiCall)
        {
            _logEntry.ExternalApiCalls.Add(externalApiCall);
        }
        
        // Salvar ou enviar log (pode ser para qualquer provider)
        public void SaveLog()
        {
            // Simulação de envio de log
            string serializedLog = JsonConvert.SerializeObject(_logEntry, Newtonsoft.Json.Formatting.Indented);
            Console.WriteLine(serializedLog);

            // Aqui você pode enviar os logs para Datadog, CloudWatch, etc.
        }
    }
}