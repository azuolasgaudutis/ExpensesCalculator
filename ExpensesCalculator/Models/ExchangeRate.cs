using System;
using System.Collections.Generic;
using System.Text;

namespace ExpensesCalculator.Models
{
    public class ExchangeRate
    {
        public DateTime Date { get; set; }
        public decimal ToExchangeRate { get; set; }
        public decimal FromExchangeRate { get; set; }
        public string ToCurrency { get; set; }
        public string FromCurrency { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;

            ExchangeRate other = (ExchangeRate)obj;

            return Date.Equals(other.Date) && ToExchangeRate.Equals(other.ToExchangeRate) && FromExchangeRate.Equals(other.FromExchangeRate) && ToCurrency.Equals(other.ToCurrency) && FromCurrency.Equals(other.FromCurrency);
        }

        public override int GetHashCode()
        {
            return new { Date, ToExchangeRate, FromExchangeRate, ToCurrency, FromCurrency }.GetHashCode();
        }
    }
}
