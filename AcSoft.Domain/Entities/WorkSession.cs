using AcSoft.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Domain.Entities;

public sealed class WorkSession
{
    private WorkSession()
    {
    }

    private WorkSession(
        Guid id,
        Guid technicianId,
        Guid workOrderId,
        WorkSessionType type,
        DateTimeOffset startedAt)
    {
        Id = id;
        TechnicianId = technicianId;
        WorkOrderId = workOrderId;
        Type = type;
        StartedAt = startedAt;
    }

    public Guid Id { get; private set; }

    public Guid TechnicianId { get; private set; }

    public Guid WorkOrderId { get; private set; }

    public WorkSessionType Type { get; private set; }

    public DateTimeOffset StartedAt { get; private set; }

    public DateTimeOffset? EndedAt { get; private set; }

    public bool IsActive => EndedAt is null;

    public TimeSpan? Duration =>
        EndedAt.HasValue
            ? EndedAt.Value - StartedAt
            : null;

    public static WorkSession Start(
        Guid technicianId,
        Guid workOrderId,
        WorkSessionType type,
        DateTimeOffset startedAt)
    {
        if (technicianId == Guid.Empty)
            throw new ArgumentException(
                "El técnico es obligatorio.",
                nameof(technicianId));

        if (workOrderId == Guid.Empty)
            throw new ArgumentException(
                "La orden de trabajo es obligatoria.",
                nameof(workOrderId));

        return new WorkSession(
            Guid.NewGuid(),
            technicianId,
            workOrderId,
            type,
            startedAt);
    }

    public void End(DateTimeOffset endedAt)
    {
        if (!IsActive)
            throw new InvalidOperationException(
                "La sesión ya ha finalizado.");

        if (endedAt < StartedAt)
            throw new InvalidOperationException(
                "La fecha de finalización no puede ser anterior al inicio.");

        EndedAt = endedAt;
    }
}
