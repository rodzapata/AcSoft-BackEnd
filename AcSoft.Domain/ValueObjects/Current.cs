using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Domain.ValueObjects;

public sealed record Current
{
    public decimal Value { get; }

    private Current(decimal value)
    {
        Value = value;
    }

    public static Current Create(decimal value)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(value),
                "La corriente no puede ser negativa.");
        }

        return new Current(value);
    }
}
