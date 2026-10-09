using AcSoft.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Domain.Entities;

public sealed class ActivityAnswer
{
    private ActivityAnswer() { }

    private ActivityAnswer(
        Guid id,
        Guid questionId,
        AnswerValue value)
    {
        Id = id;
        QuestionId = questionId;
        Value = value;
    }

    public Guid Id { get; private set; }

    public Guid QuestionId { get; private set; }

    public AnswerValue Value { get; private set; } = null!;

    public static ActivityAnswer Create(
        Guid questionId,
        AnswerValue value)
    {
        if (questionId == Guid.Empty)
        {
            throw new ArgumentException(
                "La pregunta es obligatoria.",
                nameof(questionId));
        }

        ArgumentNullException.ThrowIfNull(value);

        return new ActivityAnswer(
            Guid.NewGuid(),
            questionId,
            value);
    }

    public void ChangeValue(AnswerValue value)
    {
        ArgumentNullException.ThrowIfNull(value);

        Value = value;
    }
}
