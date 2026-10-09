using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Domain.ValueObjects;

public sealed record Pressure
{
    public decimal Value { get; }

    private Pressure(decimal value)
    {
        Value = value;
    }

    public static Pressure Create(decimal value)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(value),
                "La presión no puede ser negativa.");
        }

        return new Pressure(value);
    }
}