namespace BalootOlympicsTeamsApi.DbSchemaConfiguration;
public class ConfirmationRequestConfiguration : IEntityTypeConfiguration<ConfirmationRequest>
{
    public void Configure(EntityTypeBuilder<ConfirmationRequest> builder)
    {
        builder.ToTable("confirmation_request");
        builder.HasKey(e => e.Id).HasName("confirmation_request_pkey");

        builder.Property(e => e.Id)
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasColumnName("id");

        builder.Property(e => e.FirstPlayerId)
            .HasMaxLength(20)
            .HasColumnName("first_player_id")
            .IsRequired(required: true);

        builder.Property(e => e.FirstPlayerOtp)
            .HasMaxLength(6)
            .HasColumnName("first_player_otp")
            .IsRequired(required: true);

        builder.Property(e => e.SecondPlayerId)
            .HasMaxLength(20)
            .HasColumnName("second_player_id")
            .IsRequired(required: true);

        builder.Property(e => e.SecondPlayerOtp)
            .HasMaxLength(6)
            .HasColumnName("second_player_otp")
            .IsRequired(required: true);

        builder.Property(e => e.SentAt)
            .HasColumnType("timestamp with time zone")
            .HasConversion(ConfigurationUtils.DateTimeOffsetValueConverter)
            .HasColumnName("sent_at")
            .IsRequired(required: true);

        builder.Property(e => e.ConfirmedAt)
            .HasColumnType("timestamp with time zone")
            .HasConversion(ConfigurationUtils.DateTimeOffsetValueConverter)
            .HasColumnName("confirmed_at")
            .IsRequired(required: false);

        builder
            .HasOne(e => e.FirstPlayer)
            .WithMany()
            .HasForeignKey(e => e.FirstPlayerId)
            .HasConstraintName("fk_first_player_id")
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(e => e.SecondPlayer)
            .WithMany()
            .HasForeignKey(e => e.SecondPlayerId)
            .HasConstraintName("fk_second_player_id")
            .OnDelete(DeleteBehavior.Cascade);
    }
}