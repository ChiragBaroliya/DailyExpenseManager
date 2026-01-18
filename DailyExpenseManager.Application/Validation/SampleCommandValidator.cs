using FluentValidation;

namespace DailyExpenseManager.Application.Validation;

public class SampleCommandValidator : AbstractValidator<SampleCommand>
{
    public SampleCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class SampleCommand : CQRS.ICommand<bool>
{
    public string Name { get; set; } = string.Empty;
}
