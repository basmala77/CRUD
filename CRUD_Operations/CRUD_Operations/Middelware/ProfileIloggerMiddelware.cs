using System.Diagnostics;

namespace CRUD_Operations.Middelware
{
    public class ProfileIloggerMiddelware
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly ILogger<ProfileIloggerMiddelware> _logger;
        public ProfileIloggerMiddelware(RequestDelegate requestDelegate, ILogger<ProfileIloggerMiddelware> logger)
        {
            _requestDelegate = requestDelegate;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            await _requestDelegate(context);
            stopwatch.Stop();
            _logger.LogInformation($"Request '{context.Request.Path}' took '{stopwatch.ElapsedMilliseconds}'");
        }

    }
}
