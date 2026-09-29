using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Domain.ValueObjects;

public sealed record MeasurementRange
{
    public decimal Minimum { get; }
    public decimal Maximum { get; }

    private MeasurementRange(
        decimal minimum,
        decimal maximum)
    {
        Minimum = minimum;
        Maximum = maximum;
    }

    public static MeasurementRange Create(
        decimal minimum,
        decimal maximum)
    {
        if (minimum > maximum)
        {
            throw new ArgumentException(
                "El valor mínimo no puede ser mayor que el máximo.");
        }

        return new MeasurementRange(
            minimum,
            maximum);
    }

    public bool Contains(decimal value)
    {
        return value >= Minimum &&
               value <= Maximum;
    }
}