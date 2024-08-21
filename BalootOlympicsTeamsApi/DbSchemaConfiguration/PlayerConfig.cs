using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace BalootOlympicsTeamsApi.DbSchemaConfiguration;
public class PlayerConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.ToTable("players");
        builder.HasKey(e => e.Id).HasName("players_pkey");
        builder.HasIndex(e => e.Email, "player_email_key").IsUnique();
        builder.HasIndex(e => e.Phone, "player_phone_key").IsUnique();

        builder.Property(e => e.Id)
            .HasMaxLength(20)
            .HasColumnName("id");

        builder.Property(e => e.Email)
            .HasMaxLength(100)
            .HasColumnName("email")
            .IsRequired(required: true);

        builder.Property(e => e.Phone)
            .HasMaxLength(30)
            .HasColumnName("phone")
            .IsRequired(required: true);

        builder.Property(e => e.NameAr)
            .HasMaxLength(100)
            .HasColumnName("name_ar")
            .IsRequired(required: true);

        builder.Property(e => e.NameEn)
            .HasMaxLength(100)
            .HasColumnName("name_en")
            .IsRequired(required: true);

        builder.Property(e => e.Comment)
            .HasMaxLength(512)
            .HasColumnName("comment")
            .IsRequired(required: true);

        builder.Property(e => e.State)
            .HasMaxLength(10)
            .HasColumnName("state")
            .HasConversion<string>()
            .IsRequired(required: true);

        builder.Property(e => e.TeamId)
            .HasColumnType("INT")
            .HasColumnName("team_id")
            .IsRequired(required: false);

        builder
            .HasOne(e => e.Team)
            .WithMany(e => e.Players)
            .HasForeignKey(e => e.TeamId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}