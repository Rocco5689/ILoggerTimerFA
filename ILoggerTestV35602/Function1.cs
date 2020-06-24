using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace ILoggerTestV35602
{

    public class Function1
    {
        private readonly ILogger logger;
        private readonly IMyLoggerProvider _otherClass;

        public Function1(ILogger<Function1> _logger, IMyLoggerProvider otherClass)
        {
            logger = _logger;
            _otherClass = otherClass;
        }

        [FunctionName("Function1")]
        public void Run([TimerTrigger("%timerSchedule%")]TimerInfo myTimer, ILogger log, ExecutionContext context)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");


            // Passing in the logger Initiated by the Function Scope
            WriteLogFunction("Test Log From Function!!!", log, context);

            // Passing in the logger Initiated by the Host Scope of the MyLoggerProvider
            _otherClass.WriteLogProvider("Test Log From Logger Provider!!!", context);

            _otherClass.TestMethod();
            
        }

        public void WriteLogFunction(string logMessage, ILogger log, ExecutionContext context)
        {
            var path = String.Empty;

            try
            {
                path = Environment.GetEnvironmentVariable("HOME");

                if (path == null)
                {
                    path = String.Empty;
                }

                if (!path.ToLower().Contains("home"))
                {
                    path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                }
                
                if (path.ToLower().Contains("home"))
                {
                    path = path + @"\Logfiles";
                }
            }
            catch(Exception exc)
            {
                log.LogError(exc.Message);
            }

            log.LogInformation($"Log Path used by Function: {path}");

            System.IO.File.AppendAllText(path + @"\" + "TestLoggingFile.txt", $"{DateTime.Now} - InvocId: {context.InvocationId} - {logMessage}\n");
        }
    }
}
