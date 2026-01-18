namespace DailyExpenseManager.API.Models;

public class APIResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public object? Data { get; set; }
    public List<string>? Errors { get; set; }

    public static APIResponse SuccessResponse(object? data = null, string? message = null)
        => new() { Success = true, Data = data, Message = message };

    public static APIResponse ErrorResponse(string? message = null, List<string>? errors = null)
        => new() { Success = false, Message = message, Errors = errors };
}
