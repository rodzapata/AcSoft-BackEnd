using AcSoft.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Application.Abstractions.Persistence;

public interface IWorkSessionRepository
{
    Task<WorkSession?> GetActiveByTechnicianAsync(
        Guid technicianId,
        CancellationToken cancellationToken);

    Task AddAsync(
        WorkSession workSession,
        CancellationToken cancellationToken);
}
