using AcSoft.Domain.Enums;

namespace AcSoft.Domain.ValueObjects;

public sealed record AnswerValue
{
    private readonly List<MeasurementReading> _measurements = [];

    private AnswerValue() { }

    private AnswerValue(
        AnswerType type,
        AnswerStatus? status = null,
        decimal? numericValue = null,
        string? textValue = null,
        IReadOnlyCollection<MeasurementReading>? measurements = null)
    {
        if (!Enum.IsDefined(type))
            throw new ArgumentOutOfRangeException(nameof(type));

        Type = type;
        Status = status;
        NumericValue = numericValue;
        TextValue = textValue;

        if (measurements is not null)
            _measurements.AddRange(measurements);
    }

    public AnswerType Type { get; private set; }

    public AnswerStatus? Status { get; private set; }

    public decimal? NumericValue { get; private set; }

    public string? TextValue { get; private set; }

    public IReadOnlyCollection<MeasurementReading> Measurements =>
        _measurements.AsReadOnly();

    public static AnswerValue FromStatus(AnswerStatus status)
    {
        if (!Enum.IsDefined(status))
            throw new ArgumentOutOfRangeException(nameof(status));

        return new AnswerValue(
            AnswerType.Status,
            status: status);
    }

    public static AnswerValue FromNumeric(decimal value)
    {
        return new AnswerValue(
            AnswerType.Numeric,
            numericValue: value);
    }

    public static AnswerValue FromText(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(
                "El texto de respuesta es obligatorio.",
                nameof(value));

        return new AnswerValue(
            AnswerType.Text,
            textValue: value.Trim());
    }

    public static AnswerValue FromMeasurements(
        IEnumerable<MeasurementReading> measurements)
    {
        ArgumentNullException.ThrowIfNull(measurements);

        var items = measurements.ToArray();

        if (items.Length == 0)
            throw new ArgumentException(
                "Debe existir al menos una medición.",
                nameof(measurements));

        if (items.Any(x => x is null))
            throw new ArgumentException(
                "La colección contiene mediciones nulas.",
                nameof(measurements));

        return new AnswerValue(
            AnswerType.Measurement,
            measurements: items);
    }

    public bool Matches(AnswerType expectedType)
    {
        return Type == expectedType;
    }

    public bool MatchesMeasurementType(
        MeasurementType expectedType)
    {
        return Type == AnswerType.Measurement &&
               _measurements.Count > 0 &&
               _measurements.All(
                   x => x.Value.Type == expectedType);
    }
}