using FluentValidation;
using RecipeService.Models;

namespace RecipeService.Validators;

public class RecipeValidator : AbstractValidator<RecipeEntity>
{
    public RecipeValidator()
    {
        RuleFor(recipe => recipe.Title)
            .NotEmpty()
            .WithMessage("Title cannot be empty");

        RuleFor(recipe => recipe.Description)
            .NotEmpty()
            .WithMessage("Description cannot be empty");
    }
}