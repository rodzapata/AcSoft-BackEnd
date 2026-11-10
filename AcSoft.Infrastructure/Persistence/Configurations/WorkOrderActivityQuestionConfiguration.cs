using AcSoft.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Infrastructure.Persistence.Configurations;

public sealed class WorkOrderActivityQuestionConfiguration
    : IEntityTypeConfiguration<WorkOrderActivityQuestion>
{
    public void Configure(
        EntityTypeBuilder<WorkOrderActivityQuestion> builder)
    {
        builder.ToTable("WorkOrderActivityQuestions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.WorkOrderActivityId)
            .IsRequired();

        builder.Property(x => x.QuestionId)
            .IsRequired();

        builder.Property(x => x.QuestionCode)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(x => x.QuestionText)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.Sequence)
            .IsRequired();

        builder.Property(x => x.IsRequired)
            .IsRequired();

        builder.Property(x => x.AnswerType)
            .IsRequired();

        builder.Property(x => x.MeasurementType);

        // Evita repetir la misma posición dentro
        // de una ejecución de actividad.
        builder.HasIndex(x => new
        {
            x.WorkOrderActivityId,
            x.Sequence
        })
        .IsUnique();
    }
}
