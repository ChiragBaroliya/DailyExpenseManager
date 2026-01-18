namespace DailyExpenseManager.Domain.Entities;

public class FamilyMember
{
    public string UserId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.User;
}

public class FamilyGroup
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public List<FamilyMember> Members { get; set; } = new();
    public List<string> PendingInvitations { get; set; } = new(); // List of invited emails
}
