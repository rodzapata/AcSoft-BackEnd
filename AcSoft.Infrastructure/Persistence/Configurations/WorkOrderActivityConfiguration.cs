using AcSoft.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Infrastructure.Persistence.Configurations;

public sealed class WorkOrderActivityConfiguration
    : IEntityTypeConfiguration<WorkOrderActivity>
{
    public void Configure(EntityTypeBuilder<WorkOrderActivity> builder)
    {
        builder.ToTable("WorkOrderActivities");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.WorkOrderId)
            .IsRequired();

        builder.Property(x => x.ActivityId)
            .IsRequired();

        builder.Property(x => x.Moment)
            .IsRequired();

        builder.Property(x => x.Sequence)
            .IsRequired();

        builder.Property(x => x.IsUnplanned)
            .IsRequired();

        builder.Property(x => x.IsCompleted)
            .IsRequired();

        // Relación con WorkOrder.
        builder.HasOne<WorkOrder>()
            .WithMany(x => x.Activities)
            .HasForeignKey(x => x.WorkOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relación con Activity del catálogo.
        builder.HasOne<Activity>()
            .WithMany()
            .HasForeignKey(x => x.ActivityId)
            .OnDelete(DeleteBehavior.Restrict);

        // Preguntas configuradas para esta ejecución.
        builder.HasMany(x => x.Questions)
            .WithOne()
            .HasForeignKey(x => x.WorkOrderActivityId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Questions)
            .HasField("_questions")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        // Respuestas de esta ejecución.
        builder.HasMany(x => x.Answers)
            .WithOne()
            .HasForeignKey("WorkOrderActivityId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Answers)
            .HasField("_answers")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        // Evita repetir una actividad en la misma posición.
        builder.HasIndex(x => new
        {
            x.WorkOrderId,
            x.Moment,
            x.Sequence
        })
        .IsUnique();
    }
}
