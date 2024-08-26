namespace BalootOlympicsTeamsApi.DbSchemaConfiguration;
public class MatchConfiguration : IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        builder.ToTable("matches");
        builder.HasKey(e => e.Id).HasName("match_pkey");

        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.Level)
            .HasColumnName("level");

        builder
            .Property(e => e.QydhaGameId)
            .HasColumnName("qydha_game_id");

        builder
            .Property(e => e.RefereeId)
            .HasColumnName("referee_id");

        builder.Property(e => e.StartAt)
            .HasColumnType("timestamp with time zone")
            .HasConversion(ConfigurationUtils.DateTimeOffsetValueConverter)
            .HasColumnName("start_at");

        builder
            .HasOne(e => e.Group)
            .WithMany(e => e.Matches)
            .HasForeignKey(e => e.GroupId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(e => e.UsTeam)
            .WithMany()
            .HasForeignKey(e => e.UsTeamId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(e => e.ThemTeam)
            .WithMany()
            .HasForeignKey(e => e.ThemTeamId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(m => m.MatchQualifyUsTeam)
            .WithOne()
            .HasForeignKey<Match>(m => m.MatchQualifyUsTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(m => m.MatchQualifyThemTeam)
            .WithOne()
            .HasForeignKey<Match>(m => m.MatchQualifyThemTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(e => e.Winner)
            .HasMaxLength(30)
            .HasColumnName("winner")
            .HasConversion<string>();

        builder.Property(e => e.State)
            .HasMaxLength(10)
            .HasColumnName("state")
            .HasConversion<string>()
            .IsRequired(required: true);


    }
}