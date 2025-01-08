using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using Budget_Manager.Components.Model;

namespace Budget_Manager.Components.Services
{
    public class DealService
    {
        private List<Deal> _deals;

        public DealService()
        {
            _deals = new List<Deal>();  // Initialize the collection
        }

        public List<Deal> GetByType(Deal.DealType type)
        {
            return _deals.Where(d => d.Type == type).ToList();
        }

        public decimal GetTotalAmount(Deal.DealType type)
        {
            return _deals.Where(d => d.Type == type).Sum(d => d.Amount);
        }

        public void AddDeal(Deal deal)
        {
            _deals.Add(deal);
        }

        public List<Deal> DealsSortedByDate(Deal.DealType type, bool ascending)
        {
            var sortedDeals = GetByType(type).OrderBy(d => d.Date);
            return ascending ? sortedDeals.ToList() : sortedDeals.Reverse().ToList();
        }

        public List<Deal> GetDeals()
        {
            return _deals;
        }
    }
}
