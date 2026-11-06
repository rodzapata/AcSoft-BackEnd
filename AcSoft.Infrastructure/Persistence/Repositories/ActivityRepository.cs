using AcSoft.Application.Abstractions.Persistence;
using AcSoft.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Infrastructure.Persistence.Repositories;

public sealed class ActivityRepository : IActivityRepository
{
    private readonly AcDbContext _context;

    public ActivityRepository(AcDbContext context)
    {
        _context = context;
    }

    public async Task<Activity?> GetByIdWithQuestionsAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _context.Activities
            .AsNoTracking()
            .Include(x => x.Questions)
                .ThenInclude(x => x.Question)
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }
}
