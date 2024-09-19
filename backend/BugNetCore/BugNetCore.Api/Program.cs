using BugNetCore.Api.Filters.Hub;
using BugNetCore.Api.Hubs;
using BugNetCore.Models.Profiles;
using BugNetCore.Services.DataServices;
using BugNetCore.Services.DataServices.Settings;
using BugNetCore.Services.MailService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        // Configure logging with Serilog
        builder.ConfigureSerilog();
        builder.Services.RegisterLoggingInterfaces();


        // Adding the IHttpContextAccessor to the DI container
        builder.Services.AddHttpContextAccessor();

        builder.Services
            .AddControllers(config =>
            {
                config.Filters.Add(typeof(CustomExceptionFilter));
                config.SuppressAsyncSuffixInActionNames = false;
            })
            .AddJsonOptions(options =>
            {
                // serialize enums as strings in api responses
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

                // prevent cyclic references
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

                // allow trainling comma
                options.JsonSerializerOptions.AllowTrailingCommas = true;

            })
            .ConfigureApiBehaviorOptions(options =>
            {
                //suppress automatic model state binding errors
                options.SuppressModelStateInvalidFilter = true;

                options.SuppressMapClientErrors = false;
                options.ClientErrorMapping[StatusCodes.Status404NotFound].Link = "https://httpstatuses.com/404";
                options.ClientErrorMapping[StatusCodes.Status404NotFound].Title = "Invalid location";
            });

        // Adding Memory cache
        builder.Services.AddMemoryCache();


        // Add API versioning
        builder.Services.AddBugNetCoreApiVersionConfiguration(new ApiVersion(1, 0, "Beta"));

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddAndConfigureSwagger(
            builder.Configuration,
            Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"),
            true
            );

        // The DbContext
        var connectionString = builder.Configuration.GetConnectionString("BugNetCore");
        Console.WriteLine($"Connection string is {connectionString}");
        builder.Services.AddDbContextPool<ApplicationDbContext>(
            options => options.UseNpgsql(connectionString,
            sqlOptions => sqlOptions.EnableRetryOnFailure().CommandTimeout(60))
            );

        // AutoMapper
        builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

        // Data Access Layer Repos
        builder.Services.AddRepositories();


        // Data Services
        builder.Services.AddDataServices();

        // Email Service
        builder.Services.AddEmailService(builder.Configuration);

        // SignalR
        builder.Services.AddSignalR(
                    // o =>
                    // {
                    //     o.AddFilter<AuthHubFilter>();
                    // }
                    )
                    .AddJsonProtocol(options =>
                {
                    options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });


        // Authentication
        var jwtOptions = builder.Configuration.GetSection("jwt").Get<JwtOptions>();
        var gitHubSettings = builder.Configuration.GetSection("GitHubSettings").Get<GitHubSettings>();
        builder.Services.AddSingleton(jwtOptions);
        builder.Services.AddSingleton(gitHubSettings);
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        // If the request is for our hub...
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            path.StartsWithSegments("/hubs"))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SignKey)),


                };
            }
            )
            .AddOAuth("github", options =>
            {
                options.ClientId = gitHubSettings.ClientId;
                options.ClientSecret = gitHubSettings.ClientSecret;
                options.CallbackPath = gitHubSettings.CallbackPath;

                options.AuthorizationEndpoint = gitHubSettings.AuthorizationEndpoint;
                options.TokenEndpoint = gitHubSettings.TokenEndpoint;
                options.UserInformationEndpoint = gitHubSettings.UserInformationEndpoint;

                options.SaveTokens = true;

                options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                options.ClaimActions.MapJsonKey(ClaimTypes.Name, "login");
                options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");

                options.Events.OnTicketReceived = async context =>
                {
                    context.HandleResponse();
                    context.HttpContext.Response.Redirect(context.ReturnUri);

                };

                options.Events.OnCreatingTicket = async context =>
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
                    using var result = await context.Backchannel.SendAsync(request);
                    var user = await result.Content.ReadFromJsonAsync<JsonElement>();

                    context.RunClaimActions(user);

                    // Create or retrieve user from database
                    using var scope = context.HttpContext.RequestServices.CreateScope();
                    var userDataService = scope.ServiceProvider.GetRequiredService<IUserDataService>();

                    var email = user.GetString("email");
                    var dbUser = await userDataService.FindByEmailAsNoTrackingAsync(email);

                    if (dbUser == null)
                    {
                        dbUser = new User
                        {
                            Email = email,
                            Username = user.GetString("login"),
                            UserRole = Role.Customer,
                            PasswordHash = JwtHelpers.HashPassword(Guid.NewGuid().ToString()),  // Random password
                            IsVerified = true
                        };
                        await userDataService.AddAsync(dbUser);
                    }
                    // Generate JWT token
                    var token = JwtHelpers.GenerateJwtToken(dbUser, jwtOptions);

                    // Get the return Url 
                    var redirectUrl = context.Properties.Items["state"];

                    // construct the cookie data
                    var state = $"{Guid.NewGuid}|{redirectUrl}|{token}";
                    // Set the authentication cookie
                    context.HttpContext.Response.Cookies.Append("github_state", state, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        Expires = DateTimeOffset.UtcNow.AddMinutes(1)
                    });


                };

            });

        // Configure Kestrel 
        builder.WebHost.ConfigureKestrel(serverOptions =>
        {
            serverOptions.Listen(System.Net.IPAddress.Any, 7283);
            //         serverOptions.Listen(System.Net.IPAddress.Any, 5053, listenOptions =>
            //    {
            //        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
            //        listenOptions.UseHttps(httpsOptions =>
            //        {
            //            var certPath = Path.Combine(builder.Environment.ContentRootPath, "localhost.pfx");
            //            var certPassword = "sslStrongPassword";

            //            httpsOptions.ServerCertificate = new X509Certificate2(certPath, certPassword);
            //        });
            //    });

        });

        // CORS policy
        var allowedOrigins = new[] { "https://bugnet.ahmedyassin.dev", "http://localhost:3030", "http://localhost:7283", "http://localhost:80", "https://localhost:443" };
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin();
            });

            options.AddPolicy("AllowSpecificOrigins", builder =>
            {
                builder.WithOrigins(allowedOrigins)
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials();
            });
        });




        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            // Initialize the database
            if (app.Configuration.GetValue<bool>("RebuildDataBase"))
            {
                using var scope = app.Services.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                SampleDataInitializer.InitializeData(dbContext);
            }
        }

        app.UseSwagger();
        app.UseSwaggerUI(
            options =>
            {
                using var scope = app.Services.CreateScope();
                var versionProvider = scope.ServiceProvider.GetRequiredService<IApiVersionDescriptionProvider>();
                // build a swagger endpoint for each discovered API version
                foreach (var description in versionProvider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                        $"BugNet Core - {description.GroupName.ToUpperInvariant()}");
                }
            });

        app.UseStaticFiles();

        // app.UseHttpsRedirection();


        //Add CORS Policy
        app.UseCors("AllowSpecificOrigins");


        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.MapHub<ChatHub>("chat");
        app.MapHub<NotificationHub>("/hubs/notification");


        app.Run();
    }
}