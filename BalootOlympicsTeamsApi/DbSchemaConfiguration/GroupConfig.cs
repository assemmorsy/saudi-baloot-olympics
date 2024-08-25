namespace BalootOlympicsTeamsApi.DbSchemaConfiguration;
public class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.ToTable("groups");
        builder.HasKey(e => e.Id).HasName("groups_pkey");

        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.CheckInAt)
            .HasColumnType("timestamp with time zone")
            .HasConversion(ConfigurationUtils.DateTimeOffsetValueConverter)
            .HasColumnName("check_in_at")
            .IsRequired(required: true);

        builder.Property(e => e.StartPlayAt)
            .HasColumnType("timestamp with time zone")
            .HasConversion(ConfigurationUtils.DateTimeOffsetValueConverter)
            .HasColumnName("start_play_at")
            .IsRequired(required: true);

        builder.Property(e => e.Name)
            .HasMaxLength(30)
            .HasColumnName("name")
            .IsRequired(required: true);

    }
}