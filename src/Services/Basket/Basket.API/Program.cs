
using BuildingBlocks.Exceptions.Handler;

var builder = WebApplication.CreateBuilder(args);

// Add services to container.

// when we install carter ad building blocks it scans building blocks only
//builder.Services.AddCarter(); but carter doesn't have functionality like mediatR for this we will install carter directily to in api  for this we will 
builder.Services.AddCarter();

var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

// validators
//builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddMarten(option =>
{
    option.Connection(builder.Configuration.GetConnectionString("Database")!);
    //option.AutoCreateSchemaObjects();
    option.Schema.For<ShoppingCart>().Identity(_ => _.UserName);
}).UseLightweightSessions();

// add exception handler
builder.Services.AddExceptionHandler<CustomExceptionHandler>();



builder.Services.AddScoped<IBasketRepository, BasketRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.

// option empty because we are relay on custom handler
app.MapCarter();
app.UseExceptionHandler(option => { });

app.Run();
