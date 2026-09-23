using AcSoft.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Application.Abstractions.Persistence;

public interface IWorkOrderRepository
{
    Task AddAsync(
        WorkOrder workOrder,
        CancellationToken cancellationToken);

    Task<WorkOrder?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);
}
