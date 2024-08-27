namespace BalootOlympicsTeamsApi.DbContexts;

public partial class OlympicsContext(DbContextOptions<OlympicsContext> options) : DbContext(options)
{
    #region  dbSets
    public virtual DbSet<Player> Players { get; set; }
    public virtual DbSet<ConfirmationRequest> ConfirmationRequests { get; set; }
    public virtual DbSet<Group> Groups { get; set; }
    public virtual DbSet<Team> Teams { get; set; }
    public virtual DbSet<Match> Matches { get; set; }
    public virtual DbSet<Referee> Referees { get; set; }

    #endregion
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ConfirmationRequestConfiguration).Assembly);
        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}