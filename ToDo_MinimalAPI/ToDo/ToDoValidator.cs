using FluentValidation;

namespace ToDo_MinimalAPI;

public class ToDoValidator : AbstractValidator<ToDo>
{
    public ToDoValidator()
    {
        RuleFor(x => x.Value)
            .NotEmpty()
            .WithMessage("Value is required.")
            .MaximumLength(100)
            .WithMessage("Value must be less than 100 characters.")
            .MinimumLength(5)
            .WithMessage("Value must be at least 5 characters long.");
    }
}
