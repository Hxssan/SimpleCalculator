using CalculatorTest.Interfaces;
using CalculatorTest.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddScoped<ISimpleCalculator, SimpleCalculator>();

var app = builder.Build();

app.MapOpenApi();


app.UseHttpsRedirection();

app.MapPost("/calculator/add", (ISimpleCalculator calculator, int start, int amount) =>
    {
        try
        {
            var result = calculator.Add(start, amount);
            return Results.Ok(result);
        }
        catch (OverflowException ex)
        {
            return Results.BadRequest(new { message = ex.Message });
        }
    })
    .WithName("AddNumbers");

app.MapPost("/calculator/subtract", (ISimpleCalculator calculator, int start, int amount) =>
    {
        try
        {
            var result = calculator.Subtract(start, amount);
            return Results.Ok(result);
        }
        catch (OverflowException ex)
        {
            return Results.BadRequest(new { message = ex.Message });
        }
    })
    .WithName("SubtractNumbers");

app.Run();