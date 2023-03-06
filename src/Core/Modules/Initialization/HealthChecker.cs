using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Nocturne.Auth.Core.Modules.Initialization
{
    public static class HealthChecker
    {
        public static async Task<bool> CheckAsync(
            IHealthCheck healthCheck,
            int retryCount = 10,
            int delayInMilliseconds = 5000)
        {
            HealthCheckResult result;
            var count = 0;

            var context = new HealthCheckContext
            {
                Registration = new(
                    name: "",
                    instance: healthCheck,
                    failureStatus: HealthStatus.Unhealthy,
                    tags: null),
            };

            do
            {
                result = await healthCheck.CheckHealthAsync(context);
                count++;

                await Task.Delay(delayInMilliseconds);
            }
            while (result.Status != HealthStatus.Healthy && count < retryCount);

            return result.Status == HealthStatus.Healthy;
        }
    }
}
