using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DebitOrderProcessor.Models
{
    [XmlRoot("debitorders", Namespace = "")]
    public class DebitOrders
    {
        [XmlElement("deduction")]
        public List<DebitOrder> Deductions { get; set; }
    }
    public class DebitOrder
    {
        [XmlElement("accountholder")]
        public string AccountHolder { get; set; }

        [XmlElement("accountnumber")]
        public string AccountNumber { get; set; }

        [XmlElement("accounttype")]
        public string AccountType { get; set; }

        [XmlElement("bankname")]
        public string BankName { get; set; }

        [XmlElement("branch")]
        public string Branch { get; set; }

        [XmlElement("amount")]
        public decimal Amount { get; set; }

        [XmlElement("date")]
        public string Date { get; set; }
    }
}
