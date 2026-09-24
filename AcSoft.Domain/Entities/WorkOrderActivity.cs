using AcSoft.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Domain.Entities;

public sealed class WorkOrderActivity
{
    private WorkOrderActivity()
    {
    }

    private WorkOrderActivity(
        Guid id,
        Guid activityId)
    {
        Id = id;
        ActivityId = activityId;
        Status = WorkOrderActivityStatus.Pending;
    }

    public Guid Id { get; private set; }

    public Guid ActivityId { get; private set; }

    public ExecutionMoment Moment { get; private set; }

    public WorkOrderActivityStatus Status { get; private set; }

    internal static WorkOrderActivity Create(Guid activityId)
    {
        if (activityId == Guid.Empty)
            throw new ArgumentException(
                "La actividad es obligatoria.",
                nameof(activityId));

        return new WorkOrderActivity(
            Guid.NewGuid(),
            activityId);
    }
}