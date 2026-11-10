using AcSoft.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AcSoft.Infrastructure.Persistence.Configurations;

public sealed class ActivityAnswerConfiguration
    : IEntityTypeConfiguration<ActivityAnswer>
{
    public void Configure(EntityTypeBuilder<ActivityAnswer> builder)
    {
        builder.ToTable("ActivityAnswers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.QuestionId)
            .IsRequired();

        // AnswerValue es un objeto de valor propiedad
        // de ActivityAnswer.
        builder.OwnsOne(x => x.Value, answer =>
        {
            answer.Property(x => x.Type)
                .HasColumnName("AnswerType")
                .IsRequired();

            answer.Property(x => x.Status)
                .HasColumnName("AnswerStatus");

            answer.Property(x => x.NumericValue)
                .HasColumnName("NumericValue");

            answer.Property(x => x.TextValue)
                .HasColumnName("TextValue")
                .HasMaxLength(1000);

            // Mediciones asociadas a la respuesta.
            answer.OwnsMany(x => x.Measurements, measurement =>
            {
                measurement.ToTable("AnswerMeasurements");

                measurement.WithOwner()
                    .HasForeignKey("ActivityAnswerId");

                measurement.Property<int>("Id");

                measurement.HasKey("ActivityAnswerId", "Id");

                measurement.Property(x => x.Point)
                    .HasMaxLength(50)
                    .IsRequired();

                measurement.OwnsOne(x => x.Value, value =>
                {
                    value.Property(x => x.Type)
                        .HasColumnName("MeasurementType")
                        .IsRequired();

                    value.Property(x => x.Value)
                        .HasColumnName("MeasuredValue")
                        .IsRequired();
                });
            });

            answer.Navigation(x => x.Measurements)
                .HasField("_measurements")
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        });

        builder.Navigation(x => x.Value)
            .IsRequired();
    }
}
