using Microsoft.AspNetCore.Http.HttpResults;
using System.Numerics;
using TheatricalPlayersRefactoringKata;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/faturamentoXLM", (int numberLines, int audience, string type, string namePerformace) =>
{
    if (!(type == "comedy" || type == "history" || type == "tragedy"))
    {
        return null;
    }
    var plays = new Dictionary<string, Play>
        {
            { namePerformace, new Play(namePerformace, numberLines, type) }
        };

    var invoice = new Invoice(
        "BigCo",
        new List<Performance>
        {
                new Performance(namePerformace, audience)
        }
    );

    StatementXml statementXml = new StatementXml();
    var result = statementXml.PrintXml(invoice, plays);
    return result;
})
.WithName("GetfaturamentoXML")
.WithOpenApi();

app.MapGet("/faturamentoTXT", (int numberLines, int audience, string type, string namePerformace) =>
{
    if (!(type == "comedy" || type == "history" || type == "tragedy"))
    {
        return null;
    }
    var plays = new Dictionary<string, Play>
        {
            { namePerformace, new Play(namePerformace, numberLines, type) }
        };

    var invoice = new Invoice(
        "BigCo",
        new List<Performance>
        {
                new Performance(namePerformace, audience)
        }
    );

    StatementPrinter statementPrinter = new StatementPrinter();
    var result = statementPrinter.Print(invoice, plays);
    return result;
})
.WithName("GetfaturamentoTXT")
.WithOpenApi();

app.Run();