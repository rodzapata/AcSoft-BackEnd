using AcSoft.Application.Abstractions.Persistence;
using AcSoft.Domain.Entities;
using AcSoft.Infrastructure.Persistence.DbContext;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

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
