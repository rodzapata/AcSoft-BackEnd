using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Domain.ValueObjects;

public sealed record Temperature
{
    public decimal Value { get; }

    private Temperature(decimal value)
    {
        Value = value;
    }

    public static Temperature Create(decimal value)
    {
        if (value < -273.15m)
        {
            throw new ArgumentOutOfRangeException(
                nameof(value),
                "La temperatura no puede ser inferior al cero absoluto.");
        }

        return new Temperature(value);
    }
}
