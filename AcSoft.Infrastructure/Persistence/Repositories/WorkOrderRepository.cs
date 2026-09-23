using AcSoft.Application.Abstractions.Persistence;
using AcSoft.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Infrastructure.Persistence.Repositories;

public sealed class WorkOrderRepository
    : IWorkOrderRepository
{
    private readonly AcDbContext _context;

    public WorkOrderRepository(AcDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        WorkOrder workOrder,
        CancellationToken cancellationToken)
    {
        await _context.WorkOrders.AddAsync(
            workOrder,
            cancellationToken);
    }

    public async Task<WorkOrder?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _context.WorkOrders
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }
}
