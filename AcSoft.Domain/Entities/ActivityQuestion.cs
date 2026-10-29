using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Domain.Entities;

public sealed class ActivityQuestion
{
    private ActivityQuestion() { }

    private ActivityQuestion(
        Guid id,
        Guid activityId,
        Question question,
        int sequence,
        bool isRequired)
    {
        Id = id;
        ActivityId = activityId;
        Question = question;
        Sequence = sequence;
        IsRequired = isRequired;
    }

    public Guid Id { get; private set; }

    public Guid ActivityId { get; private set; }

    public Question Question { get; private set; } = null!;

    public Guid QuestionId => Question.Id;

    public int Sequence { get; private set; }

    public bool IsRequired { get; private set; }

    public static ActivityQuestion Create(
        Guid activityId,
        Question question,
        int sequence,
        bool isRequired = true)
    {
        if (activityId == Guid.Empty)
            throw new ArgumentException(
                "La actividad es obligatoria.",
                nameof(activityId));

        ArgumentNullException.ThrowIfNull(question);

        if (sequence <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(sequence),
                "La secuencia debe ser mayor que cero.");

        return new ActivityQuestion(
            Guid.NewGuid(),
            activityId,
            question,
            sequence,
            isRequired);
    }
}
