using AcSoft.Domain.Enums;
using AcSoft.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Domain.Entities;

public sealed class WorkOrderActivity
{
    private readonly List<WorkOrderActivityQuestion> _questions = [];
    private readonly List<ActivityAnswer> _answers = [];
    public bool IsUnplanned { get; private set; }

    private WorkOrderActivity() { }

    private WorkOrderActivity(
        Guid id,
        Guid workOrderId,
        Guid activityId,
        ExecutionMoment moment,
        int sequence,
        bool isUnplanned)
    {
        Id = id;
        WorkOrderId = workOrderId;
        ActivityId = activityId;
        Moment = moment;
        Sequence = sequence;
        IsUnplanned = isUnplanned;
    }

    public Guid Id { get; private set; }
    public Guid WorkOrderId { get; private set; }
    public Guid ActivityId { get; private set; }

    public ExecutionMoment Moment { get; private set; }
    public int Sequence { get; private set; }

    public bool IsCompleted { get; private set; }

    public IReadOnlyCollection<WorkOrderActivityQuestion> Questions =>
        _questions.AsReadOnly();

    public IReadOnlyCollection<ActivityAnswer> Answers =>
        _answers.AsReadOnly();


    public static WorkOrderActivity Create(
    Guid workOrderId,
    Guid activityId,
    ExecutionMoment moment,
    int sequence,
    bool isUnplanned = false)
    {
        if (workOrderId == Guid.Empty)
            throw new ArgumentException(
                "La orden de trabajo es obligatoria.",
                nameof(workOrderId));

        if (activityId == Guid.Empty)
            throw new ArgumentException(
                "La actividad es obligatoria.",
                nameof(activityId));

        if (!Enum.IsDefined(moment))
            throw new ArgumentOutOfRangeException(
                nameof(moment),
                "El momento de ejecución no es válido.");

        if (sequence <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(sequence),
                "La secuencia debe ser mayor que cero.");

        return new WorkOrderActivity(
            Guid.NewGuid(),
            workOrderId,
            activityId,
            moment,
            sequence,
            isUnplanned);
    }

    public void ConfigureQuestions(Activity activity)
    {
        EnsureNotCompleted();

        ArgumentNullException.ThrowIfNull(activity);

        if (activity.Id != ActivityId)
            throw new InvalidOperationException(
                "La actividad del catálogo no corresponde a esta ejecución.");

        if (_questions.Count > 0 || _answers.Count > 0)
            throw new InvalidOperationException(
                "La configuración de preguntas ya fue realizada.");

        foreach (var catalogQuestion in activity.Questions.OrderBy(q => q.Sequence))
        {
            var snapshot = WorkOrderActivityQuestion.CreateSnapshot(
                Id,
                catalogQuestion);

            _questions.Add(snapshot);
        }
    }

    public void AddAnswer(Guid questionId, AnswerValue value)
    {
        EnsureNotCompleted();

        ArgumentNullException.ThrowIfNull(value);

        var question = FindQuestion(questionId);

        ValidateAnswerValue(question, value);

        if (_answers.Any(a => a.QuestionId == questionId))
            throw new InvalidOperationException(
                "Esta pregunta ya tiene una respuesta. Utiliza ChangeAnswer para modificarla.");

        _answers.Add(ActivityAnswer.Create(questionId, value));
    }

    public void ChangeAnswer(Guid questionId, AnswerValue value)
    {
        EnsureNotCompleted();

        ArgumentNullException.ThrowIfNull(value);

        var question = FindQuestion(questionId);

        ValidateAnswerValue(question, value);

        var answer = _answers.SingleOrDefault(
            a => a.QuestionId == questionId);

        if (answer is null)
            throw new InvalidOperationException(
                "No existe una respuesta para esta pregunta.");

        answer.ChangeValue(value);
    }

    public void Complete()
    {
        EnsureNotCompleted();

        if (_questions.Count == 0)
            throw new InvalidOperationException(
                "No se puede completar la ejecución sin configurar sus preguntas.");

        var unansweredRequiredQuestions = _questions
            .Where(q => q.IsRequired)
            .Where(q => !_answers.Any(a => a.QuestionId == q.QuestionId))
            .ToList();

        if (unansweredRequiredQuestions.Count > 0)
        {
            var questionCodes = string.Join(
                ", ",
                unansweredRequiredQuestions.Select(q => q.QuestionCode));

            throw new InvalidOperationException(
                $"No se puede completar la ejecución. " +
                $"Faltan respuestas obligatorias: {questionCodes}.");
        }

        IsCompleted = true;
    }

    private WorkOrderActivityQuestion FindQuestion(Guid questionId)
    {
        if (questionId == Guid.Empty)
            throw new ArgumentException(
                "La pregunta es obligatoria.",
                nameof(questionId));

        return _questions.SingleOrDefault(q => q.QuestionId == questionId)
            ?? throw new InvalidOperationException(
                "La pregunta no pertenece a esta ejecución de actividad.");
    }

    private static void ValidateAnswerValue(
        WorkOrderActivityQuestion question,
        AnswerValue value)
    {
        if (!value.Matches(question.AnswerType))
            throw new InvalidOperationException(
                "El tipo de respuesta no corresponde al tipo configurado para la pregunta.");

        if (question.AnswerType == AnswerType.Measurement)
        {
            if (question.MeasurementType is null)
                throw new InvalidOperationException(
                    "La pregunta de medición no tiene una magnitud configurada.");

            if (!value.MatchesMeasurementType(question.MeasurementType.Value))
                throw new InvalidOperationException(
                    "La magnitud de la medición no corresponde a la pregunta.");
        }
    }

    private void EnsureNotCompleted()
    {
        if (IsCompleted)
            throw new InvalidOperationException(
                "La ejecución ya está completada y no puede modificarse.");
    }


}
