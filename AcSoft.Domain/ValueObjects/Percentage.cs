using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Domain.ValueObjects;

public sealed record Percentage
{
    public decimal Value { get; }

    private Percentage(decimal value)
    {
        Value = value;
    }

    public static Percentage Create(decimal value)
    {
        if (value < 0 || value > 100)
        {
            throw new ArgumentOutOfRangeException(
                nameof(value),
                "El porcentaje debe estar entre 0 y 100.");
        }

        return new Percentage(value);
    }
}
