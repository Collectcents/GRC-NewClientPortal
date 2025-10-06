using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace GRC_NewClientPortal.Models.GRCEmail
{
    public class LogHelper
    {
        private readonly ILogger<LogHelper> _logger;
        private readonly IConfiguration _config;

        public LogHelper(ILogger<LogHelper> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public void LogError(Exception ex)
        {
            var ipAddress = GetIPAddress();
            var regKey = _config["RegKey"] ?? "Unknown";

            _logger.LogError(ex,
                "Application: GRCClientPortal | IP: {IPAddress} | RegKey: {RegKey} | Message: {Message}",
                ipAddress,
                regKey,
                ex.Message);
        }

        private string GetIPAddress()
        {
            try
            {
                return System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName())
                                     .FirstOrDefault()?.ToString() ?? "Unknown IP";
            }
            catch
            {
                return "Unknown IP";
            }
        }
    }
}
