using FluentValidation;

namespace TextualDatabaseApi.Application.Commands.SaveTextualData
{
    public class SaveTextualDataValidator : AbstractValidator<SaveTextualDataRequest>
    {
        public SaveTextualDataValidator()
        {
            RuleFor(v => v.Text).NotEmpty();
        }
    }
}