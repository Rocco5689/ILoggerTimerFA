using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace ILoggerTestV35602
{
    public interface IMyLoggerProvider
    {
        void TestMethod();
        void WriteLogProvider(string logMessage, ExecutionContext context);
    }
}