using System;

namespace BudgetManager.Components.Model
{
    public class Deal
    {
        public enum RecordType
        {
            CashIn,
            CashOut,
            Loan
        }

        public Guid DealID { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public RecordType Type { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Tags { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public bool? IsSettled { get; set; } = false;

        public void SetSettledForOnlyDebt(bool? settled)
        {
            if (Type == RecordType.Loan)
            {
                IsSettled = settled;
            }
            else
            {
                Console.WriteLine("IsSettled is only for debt operations");
            }
        }
    }
}