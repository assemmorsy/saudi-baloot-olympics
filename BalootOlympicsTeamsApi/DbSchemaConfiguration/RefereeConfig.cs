namespace BalootOlympicsTeamsApi.DbSchemaConfiguration;
public class RefereeConfiguration : IEntityTypeConfiguration<Referee>
{
    public void Configure(EntityTypeBuilder<Referee> builder)
    {
        builder.ToTable("referees");
        builder.HasKey(e => e.Id).HasName("referees_pkey");
        builder.HasIndex(e => e.Phone, "player_phone_key").IsUnique();
        builder.HasIndex(e => e.Username, "player_username_key").IsUnique();

        builder.Property(e => e.Id)
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasColumnName("id");

        builder.Property(e => e.Name)
            .HasMaxLength(100)
            .HasColumnName("name")
            .IsRequired(required: true);

        builder.Property(e => e.Phone)
            .HasMaxLength(100)
            .HasColumnName("phone")
            .IsRequired(required: true);

        builder.Property(e => e.Username)
            .HasMaxLength(100)
            .HasColumnName("username")
            .IsRequired(required: true);
    }
}