using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Quartz;
using Quartz.Xml.JobSchedulingData20;

namespace Application.Job.SparepartStatusJob;

public class SparepartStatusJobs : IJob
{
    private readonly DataContext _context;

    public SparepartStatusJobs(DataContext context)
    {
        _context = context;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var currentDate = DateTime.UtcNow;

        var sparepart = await _context
            .Transactions.Include(p => p.TransactionDetails)
            .ThenInclude(r => r.SparePart)
            .Where(e =>
                !e.IsDeleted
                && e.Status != "Done"
                && e.TransactionDetails.Any(t => t.Equipment == null)
                && e.TransactionDetails.Any(t => t.EquipmentId == null)
            )
            .ToListAsync();

        foreach (var sp in sparepart)
        {
            var daysSinceTransaction = (currentDate - sp.TransactionDate).TotalDays;

            if (daysSinceTransaction >= 1 && daysSinceTransaction < 2 && sp.Status != "Shipping")
            {
                sp.Status = "Shipping";
            }
            else if (daysSinceTransaction >= 2 && sp.Status != "Done")
            {
                sp.Status = "Done";
            }
        }

        await _context.SaveChangesAsync();
    }
}
