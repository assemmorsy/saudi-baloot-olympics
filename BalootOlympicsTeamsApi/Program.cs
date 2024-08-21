using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.ReadConfigurationFile(builder.Environment);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
string MyAllowSpecificOrigins = builder.Services.ConfigureCORS(builder.Environment);
builder.Services.DbConfiguration(builder.Configuration, builder.Environment);

builder.Services.RegisterSettings(builder.Configuration);
builder.Services.RegisterServices(builder.Configuration, builder.Environment);
builder.Services.RegisterRepos(builder.Configuration, builder.Environment);

builder.Services.ConfigureFluentValidation();
builder.Services.ConfigureMediatR();
builder.Services.AddCarter();

LoggerServiceExtension.AddLoggerConfiguration(builder.Configuration, builder.Environment);
builder.Host.UseSerilog();

var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// app.UseHttpsRedirection();

app.UseSerilogRequestLogging();
app.MapCarter();

app.Run();