using App.BusinessLogic.Resources.Thread;
using FluentValidation;

namespace App.BusinessLogic.Validators.Thread
{
    public class CreateThreadResourceValidator : AbstractValidator<CreateThreadResource>
    {
        public CreateThreadResourceValidator()
        {
            RuleFor(m => m.Subject).NotEmpty().Length(2, 128);
            RuleFor(m => m.ParentForum).NotEmpty().InjectValidator();
            RuleFor(m => m.StartedBy).NotEmpty().InjectValidator();
        }
    }
}
