using AcSoft.Domain.Enums;
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
        Question question,
        AnswerValue value)
    {
        ArgumentNullException.ThrowIfNull(question);
        ArgumentNullException.ThrowIfNull(value);

        ValidateValue(question, value);

        return new ActivityAnswer(
            Guid.NewGuid(),
            question.Id,
            value);
    }

    public void ChangeValue(
        Question question,
        AnswerValue value)
    {
        ArgumentNullException.ThrowIfNull(question);
        ArgumentNullException.ThrowIfNull(value);

        if (question.Id != QuestionId)
        {
            throw new InvalidOperationException(
                "La pregunta no corresponde a esta respuesta.");
        }

        ValidateValue(question, value);

        Value = value;
    }

    private static void ValidateValue(
        Question question,
        AnswerValue value)
    {
        if (!value.Matches(question.AnswerType))
        {
            throw new InvalidOperationException(
                "El tipo de respuesta no corresponde al tipo de pregunta.");
        }

        if (question.AnswerType == AnswerType.Measurement &&
            !value.MatchesMeasurementType(
                question.MeasurementType!.Value))
        {
            throw new InvalidOperationException(
                "La magnitud de la medición no corresponde a la pregunta.");
        }
    }
}
