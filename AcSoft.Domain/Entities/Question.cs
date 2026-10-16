using AcSoft.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Domain.Entities;

public sealed class Question
{
    private Question() { }

    private Question(
        Guid id,
        string code,
        string text,
        AnswerType answerType,
        MeasurementType? measurementType)
    {
        Id = id;
        Code = code;
        Text = text;
        AnswerType = answerType;
        MeasurementType = measurementType;
    }

    public Guid Id { get; private set; }

    public string Code { get; private set; } = null!;

    public string Text { get; private set; } = null!;

    public AnswerType AnswerType { get; private set; }

    public MeasurementType? MeasurementType { get; private set; }

    public static Question Create(
        string code,
        string text,
        AnswerType answerType,
        MeasurementType? measurementType = null)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException(
                "El código de la pregunta es obligatorio.",
                nameof(code));
        }

        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException(
                "El texto de la pregunta es obligatorio.",
                nameof(text));
        }

        if (!Enum.IsDefined(answerType))
        {
            throw new ArgumentOutOfRangeException(
                nameof(answerType),
                "El tipo de respuesta no es válido.");
        }

        if (answerType == AnswerType.Measurement &&
            measurementType is null)
        {
            throw new ArgumentException(
                "Una pregunta de medición debe especificar la magnitud.",
                nameof(measurementType));
        }

        if (answerType != AnswerType.Measurement &&
            measurementType is not null)
        {
            throw new ArgumentException(
                "Solo las preguntas de medición pueden especificar una magnitud.",
                nameof(measurementType));
        }

        if (measurementType.HasValue &&
            !Enum.IsDefined(measurementType.Value))
        {
            throw new ArgumentOutOfRangeException(
                nameof(measurementType),
                "La magnitud de medición no es válida.");
        }

        return new Question(
            Guid.NewGuid(),
            code.Trim(),
            text.Trim(),
            answerType,
            measurementType);
    }
}
