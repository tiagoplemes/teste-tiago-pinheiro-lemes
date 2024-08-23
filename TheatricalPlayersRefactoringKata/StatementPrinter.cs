using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace TheatricalPlayersRefactoringKata;

public class StatementPrinter
{
    public string Print(Invoice invoice, Dictionary<string, Play> plays)
    {
        var amounted = new CalculatedAmount();
        var doneAmounted = amounted.Calculated(invoice, plays);

        var result = string.Format("Statement for {0}\n", invoice.Customer);
        CultureInfo cultureInfo = new CultureInfo("en-US");

        var volumeCredits = 0;
        decimal totalValue = 0;

        // print line for this order
        foreach (var item in doneAmounted)
        {
            var performance = invoice.Performances.First(x => x.PlayId == item.Name);
            var playName = plays[performance.PlayId].Name;  // Obtém o nome completo da peça
            result += String.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n", playName,
                item.Value, performance.Audience);
            volumeCredits += item.Credit;
            totalValue += item.Value;
        }
        result += String.Format(cultureInfo, "Amount owed is {0:C}\n", totalValue);
        result += String.Format("You earned {0} credits\n", volumeCredits);
        return result;
    }
}
