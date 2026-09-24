using AcSoft.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Domain.Entities;

public sealed class WorkOrderAssignment
{
    private WorkOrderAssignment()
    {
    }

    private WorkOrderAssignment(
        Guid id,
        Guid technicianId,
        DateTimeOffset assignedAt)
    {
        Id = id;
        TechnicianId = technicianId;
        AssignedAt = assignedAt;
    }

    public Guid Id { get; private set; }

    public Guid TechnicianId { get; private set; }

    public DateTimeOffset AssignedAt { get; private set; }

    public DateTimeOffset? UnassignedAt { get; private set; }

    public AssignmentEndReason? EndReason { get; private set; }

    public bool IsActive => UnassignedAt is null;

    internal static WorkOrderAssignment Create(
        Guid technicianId,
        DateTimeOffset assignedAt)
    {
        if (technicianId == Guid.Empty)
            throw new ArgumentException(
                "El técnico es obligatorio.",
                nameof(technicianId));

        return new WorkOrderAssignment(
            Guid.NewGuid(),
            technicianId,
            assignedAt);
    }

    internal void End(
        DateTimeOffset unassignedAt,
        AssignmentEndReason reason)
    {
        if (!IsActive)
            throw new InvalidOperationException(
                "La asignación ya fue finalizada.");

        if (unassignedAt < AssignedAt)
            throw new InvalidOperationException(
                "La fecha de desasignación no puede ser anterior a la asignación.");

        UnassignedAt = unassignedAt;
        EndReason = reason;
    }
}
