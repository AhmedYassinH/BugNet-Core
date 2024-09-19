
using Microsoft.Extensions.Hosting;
using NpgsqlTypes;
using Serilog;
using Serilog.Core.Enrichers;
using Serilog.Sinks.PostgreSQL;
using Serilog.Sinks.PostgreSQL.ColumnWriters;

namespace BugNetCore.Services.Logging.Configuration
{
    public static class LoggingConfiguration
    {
        public static IServiceCollection RegisterLoggingInterfaces(this IServiceCollection services)
        {
            services.AddScoped(typeof(IAppLogging<>), typeof(AppLogging<>));
            return services;
        }

        internal static readonly string OutputTemplate =
            @"[{Timestamp:yy-MM-dd HH:mm:ss} {Level}]{ApplicationName}:{SourceContext}{NewLine}Message:{Message}{NewLine}in method {MemberName} at {FilePath}:{LineNumber}{NewLine}{Exception}{NewLine}";



        internal static readonly IDictionary<string, ColumnWriterBase> columnOptions = new Dictionary<string, ColumnWriterBase>
        {
            { "Message", new RenderedMessageColumnWriter(NpgsqlDbType.Text) },
            { "MessageTemplate", new MessageTemplateColumnWriter(NpgsqlDbType.Text) },
            { "Level", new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
            { "RaiseDate", new TimestampColumnWriter(NpgsqlDbType.TimestampTz) },
            { "Exception", new ExceptionColumnWriter(NpgsqlDbType.Text) },
            { "Properties", new LogEventSerializedColumnWriter(NpgsqlDbType.Jsonb) },
            { "PropsTest", new PropertiesColumnWriter(NpgsqlDbType.Jsonb) },
            { "MachineName", new SinglePropertyColumnWriter("MachineName", PropertyWriteMethod.ToString, NpgsqlDbType.Text, "l") },
            { "FilePath", new SinglePropertyColumnWriter("FilePath", PropertyWriteMethod.ToString, NpgsqlDbType.Text, "l") },
            { "ApplicationName", new SinglePropertyColumnWriter("ApplicationName", PropertyWriteMethod.ToString, NpgsqlDbType.Text, "l") },
            { "MemberName", new SinglePropertyColumnWriter("MemberName", PropertyWriteMethod.ToString, NpgsqlDbType.Text, "l") },
            { "LineNumber", new SinglePropertyColumnWriter("LineNumber", PropertyWriteMethod.Raw, NpgsqlDbType.Integer) }
        };



        public static void ConfigureSerilog(this WebApplicationBuilder builder)
        {
            builder.Logging.ClearProviders();
            var config = builder.Configuration;
            var settings = config.GetSection(nameof(AppLoggingSettings)).Get<AppLoggingSettings>();
            var connectionStringName = settings.PostgerSQLServer.ConnectionStringName;
            var connectionString = config.GetConnectionString(connectionStringName);
            var tableName = settings.PostgerSQLServer.TableName;
            var schema = settings.PostgerSQLServer.Schema;
            string restrictedToMinimumLevel = settings.General.RestrictedToMinimumLevel;
            if (!Enum.TryParse<LogEventLevel>(restrictedToMinimumLevel, out var logLevel))
            {
                logLevel = LogEventLevel.Debug;
            }
            try
            {
                var log = new LoggerConfiguration()
                    .MinimumLevel.Is(logLevel)
                    .Enrich.FromLogContext()
                    .Enrich.With(new PropertyEnricher("ApplicationName", config.GetValue<string>("ApplicationName")))
                    .Enrich.WithMachineName()
                    .WriteTo.File(
                        path: builder.Environment.IsDevelopment()
                            ? Path.Combine("Logs", settings.File.FileName) // "log_BugNetCore.txt"
                            : settings.File.FullLogPathAndFileName,
                        rollingInterval: RollingInterval.Day,
                        restrictedToMinimumLevel: logLevel,
                        outputTemplate: OutputTemplate)
                    .WriteTo.Console(restrictedToMinimumLevel: logLevel)
                    .WriteTo.PostgreSQL(
                    connectionString: connectionString,
                    tableName: tableName,
                    columnOptions,
                    restrictedToMinimumLevel: logLevel,
                    needAutoCreateTable: false,
                    schemaName: schema
                    //,useCopy: true,
                    //queueLimit: 100,
                    //batchSizeLimit: 1,
                    //period: new TimeSpan(0, 0, 1),
                    //formatProvider: null
                    );


                builder.Logging.AddSerilog(log.CreateLogger(), false);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
