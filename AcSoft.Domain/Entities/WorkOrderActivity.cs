using AcSoft.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Domain.Entities;

public sealed class WorkOrderActivity
{
    private readonly List<ActivityAnswer> _answers = [];

    private WorkOrderActivity() { }

    private WorkOrderActivity(
        Guid id,
        Guid workOrderId,
        Guid activityId,
        ExecutionMoment moment,
        int sequence)
    {
        Id = id;
        WorkOrderId = workOrderId;
        ActivityId = activityId;
        Moment = moment;
        Sequence = sequence;
    }

    public Guid Id { get; private set; }

    public Guid WorkOrderId { get; private set; }

    public Guid ActivityId { get; private set; }

    public ExecutionMoment Moment { get; private set; }

    public int Sequence { get; private set; }

    public IReadOnlyCollection<ActivityAnswer> Answers =>
        _answers.AsReadOnly();

    public static WorkOrderActivity Create(
        Guid workOrderId,
        Guid activityId,
        ExecutionMoment moment,
        int sequence)
    {
        if (workOrderId == Guid.Empty)
        {
            throw new ArgumentException(
                "La orden de trabajo es obligatoria.",
                nameof(workOrderId));
        }

        if (activityId == Guid.Empty)
        {
            throw new ArgumentException(
                "La actividad es obligatoria.",
                nameof(activityId));
        }

        if (sequence <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(sequence),
                "La secuencia debe ser mayor que cero.");
        }

        return new WorkOrderActivity(
            Guid.NewGuid(),
            workOrderId,
            activityId,
            moment,
            sequence);
    }

    public void AddAnswer(ActivityAnswer answer)
    {
        ArgumentNullException.ThrowIfNull(answer);

        _answers.Add(answer);
    }
}
