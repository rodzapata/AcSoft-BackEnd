using AcSoft.Domain.Enums;

namespace AcSoft.Domain.Entities;

public sealed class WorkOrderActivityQuestion
{
    private WorkOrderActivityQuestion() { }

    private WorkOrderActivityQuestion(
        Guid id,
        Guid workOrderActivityId,
        Guid questionId,
        string questionCode,
        string questionText,
        int sequence,
        bool isRequired,
        AnswerType answerType,
        MeasurementType? measurementType)
    {
        Id = id;
        WorkOrderActivityId = workOrderActivityId;
        QuestionId = questionId;
        QuestionCode = questionCode;
        QuestionText = questionText;
        Sequence = sequence;
        IsRequired = isRequired;
        AnswerType = answerType;
        MeasurementType = measurementType;
    }

    public Guid Id { get; private set; }
    public Guid WorkOrderActivityId { get; private set; }
    public Guid QuestionId { get; private set; }

    public string QuestionCode { get; private set; } = null!;
    public string QuestionText { get; private set; } = null!;

    public int Sequence { get; private set; }
    public bool IsRequired { get; private set; }

    public AnswerType AnswerType { get; private set; }
    public MeasurementType? MeasurementType { get; private set; }

    internal static WorkOrderActivityQuestion CreateSnapshot(
        Guid workOrderActivityId,
        ActivityQuestion catalogQuestion)
    {
        if (workOrderActivityId == Guid.Empty)
        {
            throw new ArgumentException(
                "La ejecución de la actividad es obligatoria.",
                nameof(workOrderActivityId));
        }

        ArgumentNullException.ThrowIfNull(catalogQuestion);

        var question = catalogQuestion.Question;

        return new WorkOrderActivityQuestion(
            Guid.NewGuid(),
            workOrderActivityId,
            question.Id,
            question.Code,
            question.Text,
            catalogQuestion.Sequence,
            catalogQuestion.IsRequired,
            question.AnswerType,
            question.MeasurementType);
    }
}
