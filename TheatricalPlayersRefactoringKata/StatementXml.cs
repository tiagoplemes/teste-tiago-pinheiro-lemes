using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace TheatricalPlayersRefactoringKata
{
    public class StatementXml
    {
        public string PrintXml(Invoice invoice, Dictionary<string, Play> plays)
        {
            var amounted = new CalculatedAmount();
            var doneAmounted = amounted.Calculated(invoice, plays);

            var volumeCredits = 0;
            decimal totalValue = 0;
            var cultureInfo = CultureInfo.InvariantCulture;

            XElement root = new XElement("Statement",
               new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
               new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"),
               new XElement("Customer", invoice.Customer),
               new XElement("Items")
           );

            var document = new XDocument(
                new XDeclaration("1.0", "utf-8", null),
                root
            );

            foreach (var item in doneAmounted)
            {
                var performance = invoice.Performances.First(x => x.PlayId == item.Name);
                var formattedAmountOwed = item.Value % 1 == 0
                    ? item.Value.ToString("F0", cultureInfo)
                    : item.Value.ToString("F1", cultureInfo);

                root.Element("Items").Add(
                    new XElement("Item",
                    new XElement("AmountOwed", formattedAmountOwed),
                    new XElement("EarnedCredits", item.Credit.ToString()),
                    new XElement("Seats", performance.Audience)
                    )
                );
                volumeCredits += item.Credit;
                totalValue += item.Value;
            }

            var formattedTotalValue = totalValue % 1 == 0
                ? totalValue.ToString("F0", cultureInfo)
                : totalValue.ToString("F1", cultureInfo);

            root.Add(
                new XElement("AmountOwed", formattedTotalValue),
                new XElement("EarnedCredits", volumeCredits.ToString())
            );

            return string.Concat(document.Declaration.ToString(), "\r\n", document.ToString());
        }
    }
}
