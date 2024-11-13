using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Quartz;

namespace Application.Job.PaymentStatusJob;

public class PaymentStatusJobs : IJob
{
    private readonly DataContext _context;

    public PaymentStatusJobs(DataContext context)
    {
        _context = context;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var currentDate = DateTime.UtcNow;

        var payments = await _context
            .Payments.Include(p => p.RentalRequest)
            .ThenInclude(r => r.Equipment)
            .Where(e =>
                !e.IsDeleted
                && e.PaymentStatus != "Done"
                && e.RentalRequest.Status != "Pending"
                && e.TransactionId == null
                && e.Transaction == null
            )
            .ToListAsync();

        foreach (var pm in payments)
        {
            if (pm.PaymentStatus == "Success" && pm.PaymentStatus != "Paid")
            {
                pm.PaymentStatus = "Paid";
            }
            if (pm.RentalRequest.StarDate <= currentDate && pm.PaymentStatus != "On Rented")
            {
                pm.PaymentStatus = "On Rented";
            }

            if (pm.RentalRequest.EndDate <= currentDate && pm.PaymentStatus != "Done")
            {
                pm.PaymentStatus = "Done";

                var rentalHistory = new RentalHistory
                {
                    EquipmentId = pm.RentalRequest.EquipmentId,
                    RenterId = pm.RentalRequest.UserId,
                    Invoice = pm.RentalRequest.Invoice,
                    RentalStartDate = pm.RentalRequest.StarDate,
                    RentalEndDate = pm.RentalRequest.EndDate,
                    RentalCost = pm.Amount,
                    Location = pm.RentalRequest.Equipment.Location
                };

                await _context.RentalHistories.AddAsync(rentalHistory);
            }
        }

        await _context.SaveChangesAsync();
    }
}
