using ExpensesCalculator.Managers;
using ExpensesCalculator.Models;
using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using Xunit;

namespace ExpensesCalculatorTests
{
    public class PayoutManagerTest
    {
        [Fact]
        public void TestPayoutsFromExpenses()
        {
            PayoutManager payoutManager = new PayoutManager();
            List<Expense> expenses = new List<Expense>()
            {
                new Expense()
                {
                    EmployeeName = "JONAS",
                    Date = new DateTime(2013, 02, 20),
                    Amount = 333.21M,
                    Currency = "SEK"
                },
                new Expense()
                {
                    EmployeeName = "ANTANAS",
                    Date = new DateTime(2017, 05, 10),
                    Amount = 300.00M,
                    Currency = "USD"
                }
            };
            List<ExchangeRate> exchangeRates = new List<ExchangeRate>()
            { 
                new ExchangeRate()
                {
                    Date = new DateTime(2013, 02, 20),
                    FromCurrency = "SEK",
                    ToCurrency = "LTL",
                    FromExchangeRate = 10,
                    ToExchangeRate = 4.0724M
                },
                new ExchangeRate()
                {
                    Date = new DateTime(2017, 05, 10),
                    FromCurrency = "USD",
                    ToCurrency = "EUR",
                    FromExchangeRate = 1.0882M,
                    ToExchangeRate = 1
                }
            };

            List<Payout> expected = new List<Payout>()
            {
                new Payout()
                {
                    EmployeeName = "JONAS",
                    PayoutSum = 135.7M,
                    Currency = "LTL"

                },
                new Payout()
                {
                    EmployeeName = "ANTANAS",
                    PayoutSum = 275.68M,
                    Currency = "EUR"
                }
            };

            List<Payout> actual = payoutManager.GetPayoutsFromExpenses(expenses, exchangeRates);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestPayoutWriteToFile()
        {
            var mockFileSystem = new MockFileSystem();
            List<Payout> payouts = new List<Payout>()
            { 
                new Payout()
                {
                    EmployeeName = "ANTANAS",
                    PayoutSum = 276.97M,
                    Currency = "EUR"
                },
                new Payout()
                {
                    EmployeeName = "BONIFACIJUS",
                    PayoutSum = 2.04M,
                    Currency = "EUR"
                }
            };

            PayoutManager payoutManager = new PayoutManager(mockFileSystem);
            payoutManager.WritePayoutsToFile(payouts, @"C:\temp\payouts.txt");
            MockFileData mockFileData = mockFileSystem.GetFile(@"C:\temp\payouts.txt");
            string[] payoutLines = mockFileData.TextContents.SplitLines();

            Assert.Equal("ANTANAS     276.97 EUR", payoutLines[0]);
            Assert.Equal("BONIFACIJUS   2.04 EUR", payoutLines[1]);
        }
    }
}
