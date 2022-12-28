using MediatR;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using TwitterStatistics.Api.Queries;
using TwitterStatistics.Api.QueryHandlers;
using TwitterStatistics.Api.Site.HostedService;
using TwitterStatistics.Shared.Data.Models;
using TwitterStatistics.Tms.CommandHandlers;
using TwitterStatistics.Tms.Commands;
using static Serilog.RollingInterval;

var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder()
                               .AddJsonFile("appsettings.json", false, true).AddEnvironmentVariables().Build();
var logPath = configuration.GetSection("Logging").GetSection("Information").GetSection("LogPath").GetValue<string>("_fileName");
var loggerConfig = new LoggerConfiguration()
                .Enrich.FromLogContext().MinimumLevel.Verbose().WriteTo.Logger(l => l.MinimumLevel.Information().WriteTo.File(logPath, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 10));

var outputTemplate = "[{Timestamp:HH:mm:ss}] {Message}{NewLine}";

//Log.Logger = loggerConfig.CreateLogger();
Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("logs/.txt", outputTemplate: outputTemplate, rollingInterval: Day)
                .CreateLogger();


// Add services to the container.
builder.Services.AddMediatR(typeof(Program));
//builder.Services.AddMediatR(typeof(TwitterController));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Twitter Stats API",
        Description = "An ASP.NET Core Web API for fetching and showing Twitter Stream Status"
    });

    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
RegisterComponents(builder);



var app = builder.Build();


// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void RegisterComponents(WebApplicationBuilder builder)
{
    ///Query
    builder.Services.AddScoped<IRequest<TweetStat>, GetTwitterStatsQuery>();
    builder.Services.AddScoped<IRequestHandler<GetTwitterStatsQuery, TweetStat>, GetTwitterStatsQueryHandler>();

    //Command
    builder.Services.AddScoped<IRequest<bool>, StoreTweetsCommand>();
    builder.Services.AddScoped<IRequestHandler<StoreTweetsCommand, bool>, StoreTweetsCommandHandler>();

    //api client
    //builder.Services.AddScoped<ITweetApiClient, TweetApiClient>();

    //back Job
    builder.Services.AddHostedService<TwitterHostedService>();

    //model - db
    builder.Services.AddSingleton<Store>();

}
