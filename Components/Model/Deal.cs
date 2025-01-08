using System;
using System.ComponentModel.DataAnnotations;
namespace Budget_Manager.Components.Model
{
    public class Deal
    {
        public enum DealType
        {
            CashIn,
            CashOut
        }

        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DealType Type { get; set; }
        public string Tags { get; set; }
        public string Note { get; set; }
        public DateTime Date { get; set; }
    }
}
