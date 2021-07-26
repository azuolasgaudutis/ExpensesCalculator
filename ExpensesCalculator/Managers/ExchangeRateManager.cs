using ExpensesCalculator.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ExpensesCalculator.Managers
{
    public class ExchangeRateManager
    {
        public string WebServiceUrl { get; set; }
        public string XMLNamespace { get; set; }
        public ExchangeRate GetExchangeRatesForCurrencyByDate(DateTime date, string currency)
        {
            ExchangeRate exchangeRate = new ExchangeRate()
            {
                Date = date,
                FromCurrency = currency
            };

            List<FxRate> fxRates = GetFxRatesFromWebService(date);
            FxRate fxExchangeRate = fxRates.Where(x => x.CcyAmt.Any(y => y.Ccy.Equals(currency))).FirstOrDefault();
            List<CcyAmt> ccyAmts = fxExchangeRate.CcyAmt;

            exchangeRate.ToExchangeRate = decimal.Parse(ccyAmts.Where(x => !x.Ccy.Equals(currency)).FirstOrDefault().Amt, CultureInfo.InvariantCulture);
            exchangeRate.FromExchangeRate = decimal.Parse(ccyAmts.Where(x => x.Ccy.Equals(currency)).FirstOrDefault().Amt, CultureInfo.InvariantCulture);
            exchangeRate.ToCurrency = ccyAmts.Where(x => !x.Ccy.Equals(currency)).FirstOrDefault().Ccy;

            return exchangeRate;
        }

        private List<FxRate> GetFxRatesFromWebService(DateTime date)
        {
            List<FxRate> fxRates = new List<FxRate>();

            string tp = DateTime.Compare(date, new DateTime(2015, 01, 01)) < 0 ? "LT" : "EU";
            var request = (HttpWebRequest)WebRequest.Create(string.Format(this.WebServiceUrl, tp, date.ToString()));
            var response = (HttpWebResponse)request.GetResponse();

            string xmlResponse;

            using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
            {
                xmlResponse = streamReader.ReadToEnd();
            }

            XDocument document = XDocument.Parse(xmlResponse);
            XNamespace ns = this.XMLNamespace;

            XmlSerializer serializer = new XmlSerializer(typeof(FxRate), this.XMLNamespace);
            var xmlFxRates = document.Descendants(ns + "FxRate").ToList();

            foreach (var xmlFxRate in xmlFxRates)
            {
                using (StringReader stringReader = new StringReader(xmlFxRate.ToString()))
                {
                    fxRates.Add((FxRate)serializer.Deserialize(stringReader));
                }
            }

            return fxRates;
        }
    }
}
