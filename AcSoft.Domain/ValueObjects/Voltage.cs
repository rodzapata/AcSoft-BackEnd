using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Domain.ValueObjects;

public sealed record Voltage
{
    public decimal Value { get; }

    private Voltage(decimal value)
    {
        Value = value;
    }

    public static Voltage Create(decimal value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(
                nameof(value),
                "El voltaje no puede ser negativo.");
        if (value > 1000)
            throw new ArgumentOutOfRangeException(
                nameof(value),
                "El voltaje supera el límite permitido.");

        return new Voltage(value);
    }
}
