using Arowolo_Delivery_Project.Data;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace Arowolo_Delivery_Project.Services.BackgroundJobs
{
    public class BackgroundJob : IJob
    {
        private readonly ILogger<BackgroundJob> _logger;
        private readonly ApplicationDbContext _context;

        public BackgroundJob(ILogger<BackgroundJob> logger,
            ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation($"Background job started at {DateTime.UtcNow}");

            var oldDate = DateTime.Now.AddDays(-3);

            var expiredTokens =
                await _context.LogoutTokens.Where(x => x.CreateDateTime <= oldDate && !x.DeleteDate.HasValue).ToListAsync();
            _logger.LogInformation($"Tokens to delete - {expiredTokens.Count}");
            _context.LogoutTokens.RemoveRange(expiredTokens);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Background job finished at {DateTime.UtcNow}");
        }
    }
}
