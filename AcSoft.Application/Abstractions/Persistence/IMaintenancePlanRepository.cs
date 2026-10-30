using AcSoft.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Application.Abstractions.Persistence;

public interface IMaintenancePlanRepository
{
    Task<MaintenancePlan?> GetByIdWithActivitiesAsync(
        Guid id,
        CancellationToken cancellationToken);
}