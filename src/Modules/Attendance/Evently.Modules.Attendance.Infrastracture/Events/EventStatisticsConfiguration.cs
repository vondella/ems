using Evently.Modules.Attendance.Domain.Events;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Attendance.Infrastracture.Events;

internal sealed class EventStatisticsConfiguration : IEntityTypeConfiguration<EventStatistics>
{
    public void Configure(EntityTypeBuilder<EventStatistics> builder)
    {
        builder.ToTable("event_statistics");

        builder.HasKey(es => es.EventId);

        builder.Property(es => es.EventId).ValueGeneratedNever();
    }
}