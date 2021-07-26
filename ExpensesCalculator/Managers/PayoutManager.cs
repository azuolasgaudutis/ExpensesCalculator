using ExpensesCalculator.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;

namespace ExpensesCalculator.Managers
{
    public class PayoutManager
    {
        private readonly IFileSystem FileSystem;
        public PayoutManager() : this(new FileSystem()) { }

        public PayoutManager(IFileSystem fileSystem)
        {
            FileSystem = fileSystem;
        }

        public List<Payout> GetPayoutsFromExpenses(List<Expense> expenses, List<ExchangeRate> exchangeRates)
        {
            List<Payout> payouts = new List<Payout>();

            foreach (var expense in expenses)
            {
                ExchangeRate exchangeRate = exchangeRates.Where(x => x.Date.Equals(expense.Date) && x.FromCurrency.Equals(expense.Currency)).FirstOrDefault();
                payouts.Add(
                    new Payout()
                    {
                        EmployeeName = expense.EmployeeName,
                        PayoutSum = CalculatePayoutSum(expense.Amount, exchangeRate.ToExchangeRate, exchangeRate.FromExchangeRate),
                        Currency = exchangeRate.ToCurrency
                    });
            }

            return payouts;
        }

        private decimal CalculatePayoutSum(decimal amount, decimal toExchangeRate, decimal fromExchangeRate)
        {
            return Math.Round((amount * toExchangeRate) / fromExchangeRate, 2);
        }

        public void WritePayoutsToFile(List<Payout> payouts, string filename)
        {
            int longestName = payouts.OrderByDescending(s => s.EmployeeName.Length).FirstOrDefault().EmployeeName.Length;
            int longestPayoutSum = payouts.OrderByDescending(s => s.PayoutSum.ToString().Length).FirstOrDefault().PayoutSum.ToString().Length;

            using (StreamWriter streamWriter = FileSystem.File.CreateText(filename))
            {
                foreach (var payout in payouts)
                {
                    streamWriter.WriteLine(string.Format("{0,-"+ longestName + "} {1,"+ longestPayoutSum + "} {2,-3}", payout.EmployeeName, payout.PayoutSum.ToString(CultureInfo.InvariantCulture), payout.Currency));
                }
            }
        }
    }
}
