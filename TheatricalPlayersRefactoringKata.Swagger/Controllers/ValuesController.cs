using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TheatricalPlayersRefactoringKata.Swagger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ValuesController>/5
        [HttpGet]
        public string Get(int numberLines, int audience, string type, string namePerformace)
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
        }
    }
}
