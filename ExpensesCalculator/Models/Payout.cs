using System;
using System.Collections.Generic;
using System.Text;

namespace ExpensesCalculator.Models
{
    public class Payout
    {
        public string EmployeeName { get; set; }
        public decimal PayoutSum { get; set; }
        public string Currency { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;

            Payout other = (Payout)obj;

            return EmployeeName.Equals(other.EmployeeName) && PayoutSum.Equals(other.PayoutSum) && Currency.Equals(other.Currency);
        }

        public override int GetHashCode()
        {
            return new { EmployeeName, PayoutSum, Currency }.GetHashCode();
        }
    }
}
