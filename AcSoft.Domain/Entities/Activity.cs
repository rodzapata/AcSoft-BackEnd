using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Domain.Entities;

public sealed class Activity
{
    private readonly List<ActivityQuestion> _questions = [];

    private Activity() { }

    private Activity(
        Guid id,
        string code,
        string name,
        string description)
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
    }

    public Guid Id { get; private set; }

    public string Code { get; private set; } = null!;

    public string Name { get; private set; } = null!;

    public string Description { get; private set; } = null!;

    public IReadOnlyCollection<ActivityQuestion> Questions =>
        _questions.AsReadOnly();

    public static Activity Create(
        string code,
        string name,
        string description)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException(
                "El código de la actividad es obligatorio.",
                nameof(code));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                "El nombre de la actividad es obligatorio.",
                nameof(name));
        }

        return new Activity(
            Guid.NewGuid(),
            code.Trim(),
            name.Trim(),
            description?.Trim() ?? string.Empty);
    }

    public void AddQuestion(
        Question question,
        int sequence,
        bool isRequired = true)
    {
        ArgumentNullException.ThrowIfNull(question);

        if (_questions.Any(
            x => x.QuestionId == question.Id))
        {
            throw new InvalidOperationException(
                "La pregunta ya está asociada a esta actividad.");
        }

        if (_questions.Any(
            x => x.Sequence == sequence))
        {
            throw new InvalidOperationException(
                "Ya existe una pregunta con esa secuencia.");
        }

        var activityQuestion = ActivityQuestion.Create(
            Id,
            question,
            sequence,
            isRequired);

        _questions.Add(activityQuestion);
    }
}
