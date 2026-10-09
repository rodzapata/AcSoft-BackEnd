using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Domain.ValueObjects;

public sealed record MeasurementReading
{
    public string Point { get; }

    public MeasurementValue Value { get; }

    private MeasurementReading(
        string point,
        MeasurementValue value)
    {
        Point = point;
        Value = value;
    }

    public static MeasurementReading Create(
        string point,
        MeasurementValue value)
    {
        if (string.IsNullOrWhiteSpace(point))
        {
            throw new ArgumentException(
                "El punto de medición es obligatorio.",
                nameof(point));
        }

        ArgumentNullException.ThrowIfNull(value);

        return new MeasurementReading(
            point.Trim(),
            value);
    }
}
