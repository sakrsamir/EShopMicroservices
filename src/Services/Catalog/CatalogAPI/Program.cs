


using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to container.


// when we install carter ad building blocks it scans building blocks only
//builder.Services.AddCarter(); but carter doesn't have functionality like mediatR for this we will install carter directily to in api  for this we will 
builder.Services.AddCarter();

var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config =>
{
    // register all services in this api solution like carter
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

// validators
builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddMarten(option =>
{
    option.Connection(builder.Configuration.GetConnectionString("Database")!);
    //option.AutoCreateSchemaObjects();
}).UseLightweightSessions();

if (builder.Environment.IsDevelopment())
{
    builder.Services.InitializeMartenWith<CataloInitialData>();
}

// add exception handler
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

// add health check
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!);

var app = builder.Build();

// Configure the HTTP request pipeline. 


app.MapCarter();


// option empty because we are relay on custom handler
app.UseExceptionHandler(option => { });


app.UseHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.Run();
