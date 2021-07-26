using System;

namespace ExpensesCalculator.Models
{
    public class Expense
    {
        public string EmployeeName { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;

            Expense other = (Expense)obj;

            return EmployeeName.Equals(other.EmployeeName) && Date.Equals(other.Date) && Amount.Equals(other.Amount) && Currency.Equals(other.Currency);
        }

        public override int GetHashCode()
        {
            return new { EmployeeName, Date, Amount, Currency }.GetHashCode();
        }
    }
}
