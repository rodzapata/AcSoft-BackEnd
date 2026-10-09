using AcSoft.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Domain.ValueObjects;

public sealed record AnswerValue
{
    public AnswerType Type { get; }

    public AnswerStatus? Status { get; }

    public decimal? NumericValue { get; }

    public string? TextValue { get; }

    public IReadOnlyCollection<MeasurementReading> Measurements { get; }

    private AnswerValue(
        AnswerType type,
        AnswerStatus? status = null,
        decimal? numericValue = null,
        string? textValue = null,
        IReadOnlyCollection<MeasurementReading>? measurements = null)
    {
        Type = type;
        Status = status;
        NumericValue = numericValue;
        TextValue = textValue;
        Measurements = measurements ?? [];
    }

    public static AnswerValue FromStatus(
        AnswerStatus status)
    {
        return new AnswerValue(
            AnswerType.Status,
            status: status);
    }

    public static AnswerValue FromNumeric(
        decimal value)
    {
        return new AnswerValue(
            AnswerType.Numeric,
            numericValue: value);
    }

    public static AnswerValue FromText(
        string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(
                "El texto de respuesta es obligatorio.",
                nameof(value));
        }

        return new AnswerValue(
            AnswerType.Text,
            textValue: value.Trim());
    }

    public static AnswerValue FromMeasurements(
        IEnumerable<MeasurementReading> measurements)
    {
        ArgumentNullException.ThrowIfNull(measurements);

        var items = measurements.ToArray();

        if (items.Length == 0)
        {
            throw new ArgumentException(
                "Debe existir al menos una medición.",
                nameof(measurements));
        }

        return new AnswerValue(
            AnswerType.Measurement,
            measurements: items);
    }
}
