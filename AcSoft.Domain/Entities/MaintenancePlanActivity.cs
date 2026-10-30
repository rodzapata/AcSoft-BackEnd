using AcSoft.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Domain.Entities;

public sealed class MaintenancePlanActivity
{
    private MaintenancePlanActivity()
    {
    }

    private MaintenancePlanActivity(
        Guid id,
        Guid maintenancePlanId,
        Guid activityId,
        ExecutionMoment moment,
        int sequence)
    {
        Id = id;
        MaintenancePlanId = maintenancePlanId;
        ActivityId = activityId;
        Moment = moment;
        Sequence = sequence;
    }

    public Guid Id { get; private set; }

    public Guid MaintenancePlanId { get; private set; }

    public Guid ActivityId { get; private set; }

    public ExecutionMoment Moment { get; private set; }

    public int Sequence { get; private set; }

    public static MaintenancePlanActivity Create(
        Guid maintenancePlanId,
        Guid activityId,
        ExecutionMoment moment,
        int sequence)
    {
        if (maintenancePlanId == Guid.Empty)
        {
            throw new ArgumentException(
                "El plan de mantenimiento es obligatorio.",
                nameof(maintenancePlanId));
        }

        if (activityId == Guid.Empty)
        {
            throw new ArgumentException(
                "La actividad es obligatoria.",
                nameof(activityId));
        }

        if (!Enum.IsDefined(moment))
        {
            throw new ArgumentOutOfRangeException(
                nameof(moment),
                "El momento de ejecución no es válido.");
        }

        if (sequence <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(sequence),
                "La secuencia debe ser mayor que cero.");
        }

        return new MaintenancePlanActivity(
            Guid.NewGuid(),
            maintenancePlanId,
            activityId,
            moment,
            sequence);
    }
}

