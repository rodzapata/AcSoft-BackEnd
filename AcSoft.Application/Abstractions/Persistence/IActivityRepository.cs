using System;
using System.Collections.Generic;
using System.Text;
using AcSoft.Domain.Entities;

namespace AcSoft.Application.Abstractions.Persistence;

public interface IActivityRepository
{
    Task<Activity?> GetByIdWithQuestionsAsync(
        Guid id,
        CancellationToken cancellationToken);
}