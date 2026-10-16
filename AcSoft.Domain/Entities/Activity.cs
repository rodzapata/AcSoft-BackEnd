using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Domain.Entities;

public sealed class Activity
{
    private readonly List<Question> _questions = [];

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

    public IReadOnlyCollection<Question> Questions =>
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

    public void AddQuestion(Question question)
    {
        ArgumentNullException.ThrowIfNull(question);

        if (_questions.Any(q => q.Id == question.Id))
        {
            throw new InvalidOperationException(
                "La pregunta ya está asociada a esta actividad.");
        }

        _questions.Add(question);
    }
}
