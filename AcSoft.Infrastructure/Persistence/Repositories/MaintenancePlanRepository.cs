using AcSoft.Application.Abstractions.Persistence;
using AcSoft.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Infrastructure.Persistence.Repositories;

public sealed class MaintenancePlanRepository : IMaintenancePlanRepository
{
    private readonly AcDbContext _context;

    public MaintenancePlanRepository(AcDbContext context)
    {
        _context = context;
    }

    public async Task<MaintenancePlan?> GetByIdWithActivitiesAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _context.MaintenancePlans
            .AsNoTracking()
            .Include(x => x.Activities)
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }
}
