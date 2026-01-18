namespace DailyExpenseManager.Domain.Entities;

public class MonthlyBudget
{
    public string Id { get; set; } = string.Empty;
    public string FamilyGroupId { get; set; } = string.Empty;
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal Amount { get; set; }
}
