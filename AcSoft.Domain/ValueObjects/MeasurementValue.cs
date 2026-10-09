using AcSoft.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Domain.ValueObjects;

public sealed record MeasurementValue
{
    public MeasurementType Type { get; }

    public decimal Value { get; }

    private MeasurementValue(
        MeasurementType type,
        decimal value)
    {
        Type = type;
        Value = value;
    }

    public static MeasurementValue FromVoltage(Voltage voltage)
    {
        ArgumentNullException.ThrowIfNull(voltage);

        return new MeasurementValue(
            MeasurementType.Voltage,
            voltage.Value);
    }

    public static MeasurementValue FromCurrent(Current current)
    {
        ArgumentNullException.ThrowIfNull(current);

        return new MeasurementValue(
            MeasurementType.Current,
            current.Value);
    }

    public static MeasurementValue FromPressure(Pressure pressure)
    {
        ArgumentNullException.ThrowIfNull(pressure);

        return new MeasurementValue(
            MeasurementType.Pressure,
            pressure.Value);
    }

    public static MeasurementValue FromTemperature(Temperature temperature)
    {
        ArgumentNullException.ThrowIfNull(temperature);

        return new MeasurementValue(
            MeasurementType.Temperature,
            temperature.Value);
    }

    public static MeasurementValue FromCapacitance(Capacitance capacitance)
    {
        ArgumentNullException.ThrowIfNull(capacitance);

        return new MeasurementValue(
            MeasurementType.Capacitance,
            capacitance.Value);
    }

    public static MeasurementValue FromMass(Mass mass)
    {
        ArgumentNullException.ThrowIfNull(mass);

        return new MeasurementValue(
            MeasurementType.Mass,
            mass.Value);
    }
}
