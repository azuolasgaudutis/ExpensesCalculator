using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ExpensesCalculator.Models
{
    [XmlRoot("FxRate")]
    public class FxRate
    {
        [XmlElement("Tp")]
        public string Tp { get; set; }
        [XmlElement("Dt")]
        public string Dt { get; set; }
        [XmlElement("CcyAmt")]
        public List<CcyAmt> CcyAmt { get; set; }
    }

    public class CcyAmt
    {
        [XmlElement("Ccy")]
        public string Ccy { get; set; }
        [XmlElement("Amt")]
        public string Amt { get; set; }
    }
}
