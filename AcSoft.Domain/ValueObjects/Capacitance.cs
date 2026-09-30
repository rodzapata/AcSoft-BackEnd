using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Domain.ValueObjects;

public sealed record Capacitance
{
    public decimal Value { get; }

    private Capacitance(decimal value)
    {
        Value = value;
    }

    public static Capacitance Create(decimal value)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(value),
                "La capacitancia no puede ser negativa.");
        }

        return new Capacitance(value);
    }
}
