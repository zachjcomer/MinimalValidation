using System.ComponentModel.DataAnnotations;

namespace Zach.MinimalValidators.Library.Validators;

public class InvalidAttribute : ValidationAttribute
{
    private const string message = "{0} is invalid.";
    public override bool IsValid(object? value) => false;

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext) => new ValidationResult(string.Format(message, nameof(value)));
}