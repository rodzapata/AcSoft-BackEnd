using AcSoft.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Infrastructure.Persistence.Configurations;

public sealed class ActivityQuestionConfiguration
    : IEntityTypeConfiguration<ActivityQuestion>
{
    public void Configure(EntityTypeBuilder<ActivityQuestion> builder)
    {
        builder.ToTable("ActivityQuestions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ActivityId)
            .IsRequired();

        builder.Property(x => x.Sequence)
            .IsRequired();

        builder.Property(x => x.IsRequired)
            .IsRequired();

        // QuestionId es una propiedad calculada del dominio.
        builder.Ignore(x => x.QuestionId);

        // EF Core utiliza una FK de sombra llamada QuestionId.
        builder.HasOne(x => x.Question)
            .WithMany()
            .HasForeignKey("QuestionId")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex("ActivityId", "Sequence")
            .IsUnique();
    }
}
