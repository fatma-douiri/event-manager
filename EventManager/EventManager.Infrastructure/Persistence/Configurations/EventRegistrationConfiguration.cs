using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using EventManager.Domain.Model;

public class EventRegistrationConfiguration : IEntityTypeConfiguration<EventRegistration>
{
    public void Configure(EntityTypeBuilder<EventRegistration> builder)
    {
        builder.HasKey(er => er.Id);

        builder.Property(er => er.RegisteredAt)
            .IsRequired();

        builder.HasOne(er => er.Event)
            .WithMany(e => e.Registrations)
            .HasForeignKey(er => er.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(er => er.User)
            .WithMany(u => u.EventRegistrations)
            .HasForeignKey(er => er.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(er => new { er.EventId, er.UserId }).IsUnique();
    }
}