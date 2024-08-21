namespace BalootOlympicsTeamsApi.DbSchemaConfiguration;
public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.ToTable("teams");
        builder.HasKey(e => e.Id).HasName("teams_pkey");
        builder.Property(e => e.Id)
            .HasColumnName("id");
    }
}