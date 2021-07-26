using ExpensesCalculator.Managers;
using ExpensesCalculator.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ExpensesCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appSettings.json").Build();

            // Read file
            ExpenseManager expenseManager = new ExpenseManager();
            ExchangeRateManager exchangeRateManager =
                new ExchangeRateManager()
                {
                    WebServiceUrl = config.GetSection("fxRatesURL").Value,
                    XMLNamespace = config.GetSection("fxRatesNamespace").Value
                };
            List<Expense> expenses = expenseManager.LoadExpensesFromFile(config.GetSection("expenseFileLocation").Value);

            // Remove any expenses before 1993-06-25
            expenses.RemoveAll(x => DateTime.Compare(x.Date, new DateTime(1993, 06, 25)) < 0);

            // Load currency rates
            List<ExchangeRate> exchangeRates = new List<ExchangeRate>();
            foreach (var expense in expenses)
            {
                exchangeRates.Add(exchangeRateManager.GetExchangeRatesForCurrencyByDate(expense.Date, expense.Currency));
            }

            // Calculate payouts and aggregate by employee
            PayoutManager payoutManager = new PayoutManager();
            List<Payout> payouts = payoutManager.GetPayoutsFromExpenses(expenses, exchangeRates);

            List<Payout> aggregatedPayouts = payouts.GroupBy(x => new { x.EmployeeName, x.Currency }).Select(x => new Payout { EmployeeName = x.Key.EmployeeName, PayoutSum = x.Sum(x => x.PayoutSum), Currency = x.Key.Currency }).OrderBy(x => x.EmployeeName).ThenBy(x => x.Currency).ToList();

            // Write to file
            payoutManager.WritePayoutsToFile(aggregatedPayouts, config.GetSection("payoutFileLocation").Value);
        }
    }
}
