using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Domain.ValueObjects;

public sealed record Mass
{
    public decimal Value { get; }

    private Mass(decimal value)
    {
        Value = value;
    }

    public static Mass Create(decimal value)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(value),
                "La masa no puede ser negativa.");
        }

        return new Mass(value);
    }
}
