using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using BudgetManager.Components.Model;


namespace BudgetManager.Components.Services
{
    public class DealService
    {
        private List<Deal> deals = new();
        private readonly string filepath;
        private readonly string leftCashFlowPath;
        private decimal leftCashFlow;
        public string Message { get; private set; } = string.Empty;

        public DealService()
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            filepath = Path.Combine(desktopPath, "deals.json");
            leftCashFlowPath = Path.Combine(desktopPath, "leftCashFlow.json");
            deals = LoadDeals();
            leftCashFlow = LoadLeftCashFlow();
        }

        public decimal GetLeftCashFlow()
        {
            return leftCashFlow;
        }

        public void UpdateLeftCashFlow(decimal newLeftCashFlow)
        {
            leftCashFlow = newLeftCashFlow;
            SaveLeftCashFlow();
        }

        private void SaveLeftCashFlow()
        {
            try
            {
                var json = JsonSerializer.Serialize(leftCashFlow);
                File.WriteAllText(leftCashFlowPath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving left cash flow: {ex.Message}");
            }
        }

        private decimal LoadLeftCashFlow()
        {
            if (File.Exists(leftCashFlowPath))
            {
                var json = File.ReadAllText(leftCashFlowPath);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    try
                    {
                        return JsonSerializer.Deserialize<decimal>(json);
                    }
                    catch (JsonException)
                    {
                        Console.WriteLine("Invalid JSON in the file. Falling back to total cash flow.");
                        return GetTotalAmount(Deal.RecordType.CashIn) - GetTotalAmount(Deal.RecordType.CashOut);
                    }
                }
            }
            return GetTotalAmount(Deal.RecordType.CashIn) - GetTotalAmount(Deal.RecordType.CashOut);
        }

        public void AddDeal(Deal deal)
        {
            deals.Add(deal);
            SaveDeals();
        }

        private void SaveDeals()
        {
            try
            {
                var json = JsonSerializer.Serialize(deals);
                File.WriteAllText(filepath, json);
                Message = "Deal data is saved!";
            }
            catch (Exception ex)
            {
                Message = "Error saving the deal!";
                Console.WriteLine(ex.Message);
            }
        }

        private List<Deal> LoadDeals()
        {
            try
            {
                if (File.Exists(filepath))
                {
                    var json = File.ReadAllText(filepath);
                    Message = "Deals loaded successfully from the Desktop!";
                    return JsonSerializer.Deserialize<List<Deal>>(json) ?? new List<Deal>();
                }
                else
                {
                    Message = "No saved deals found.";
                    return new List<Deal>();
                }
            }
            catch (Exception ex)
            {
                Message = $"Error loading deals: {ex.Message}";
                return new List<Deal>();
            }
        }

        public List<Deal> GetByType(Deal.RecordType type)
        {
            return deals.Where(d => d.Type == type).ToList();
        }

        public decimal GetTotalAmount(Deal.RecordType type)
        {
            return deals.Where(d => d.Type == type).Sum(d => d.Amount);
        }

        public List<Deal> GetAllDeals()
        {
            return deals;
        }

        public List<Deal> DealsSortedByDate(Deal.RecordType type, bool isDateAscending)
        {
            var dealList = GetByType(type);
            return isDateAscending
                ? dealList.OrderBy(d => d.Date).ToList()
                : dealList.OrderByDescending(d => d.Date).ToList();
        }

        public void DeleteDeal(Guid dealID)
        {
            var dealToDelete = deals.FirstOrDefault(d => d.DealID == dealID);
            if (dealToDelete != null)
            {
                deals.Remove(dealToDelete);
                SaveDeals();
            }
        }

        public List<Deal> GetCashOutflowDeals()
        {
            return deals.Where(d => d.Type == Deal.RecordType.CashOut).ToList();
        }

        public decimal CalculateRemainingIncome()
        {
            return GetTotalAmount(Deal.RecordType.CashIn) - GetTotalAmount(Deal.RecordType.CashOut);
        }

        public List<Deal> FilterDeals(string searchText, string tags, DateTime? date, Deal.RecordType type)
        {
            return GetByType(type).Where(d =>
                (string.IsNullOrEmpty(searchText) || d.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(tags) || d.Tags.Contains(tags, StringComparison.OrdinalIgnoreCase)) &&
                (!date.HasValue || d.Date.Date == date.Value.Date)
            ).ToList();
        }
    }
}