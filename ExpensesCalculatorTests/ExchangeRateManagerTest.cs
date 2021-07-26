using ExpensesCalculator.Managers;
using ExpensesCalculator.Models;
using Microsoft.Extensions.Configuration;
using System;
using Xunit;

namespace ExpensesCalculatorTests
{
    public class ExchangeRateManagerTest
    {
        [Fact]
        public void TestGetExchangeRatesForCurrencyByDate_LTL()
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appSettings.test.json").Build();

            DateTime date = new DateTime(2013, 02, 20);
            string currency = "SEK";
            ExchangeRateManager exchangeRateManager =
                new ExchangeRateManager()
                {
                    WebServiceUrl = config.GetSection("fxRatesURL").Value,
                    XMLNamespace = config.GetSection("fxRatesNamespace").Value
                };

            ExchangeRate expected = new ExchangeRate()
            {
                Date = new DateTime(2013, 02, 20),
                FromCurrency = "SEK",
                ToCurrency = "LTL",
                FromExchangeRate = 10, 
                ToExchangeRate = 4.0724M
            };

            ExchangeRate actual = exchangeRateManager.GetExchangeRatesForCurrencyByDate(date, currency);

            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void TestGetExchangeRatesForCurrencyByDate_EUR()
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appSettings.test.json").Build();

            DateTime date = new DateTime(2017, 05, 10);
            string currency = "USD";
            ExchangeRateManager exchangeRateManager =
                new ExchangeRateManager()
                {
                    WebServiceUrl = config.GetSection("fxRatesURL").Value,
                    XMLNamespace = config.GetSection("fxRatesNamespace").Value
                };

            ExchangeRate expected = new ExchangeRate()
            {
                Date = new DateTime(2017, 05, 10),
                FromCurrency = "USD",
                ToCurrency = "EUR",
                FromExchangeRate = 1.0882M,
                ToExchangeRate = 1
            };

            ExchangeRate actual = exchangeRateManager.GetExchangeRatesForCurrencyByDate(date, currency);

            Assert.Equal(expected, actual);
        }
    }
}
