using App.BusinessLogic.Resources.Thread;
using FluentValidation;

namespace App.BusinessLogic.Validators.Thread
{
    public class ThreadResourceValidator : AbstractValidator<ThreadResource>
    {
        public ThreadResourceValidator()
        {
            RuleFor(m => m.Id).NotEmpty();
            RuleFor(m => m.Subject).NotEmpty().Length(2, 128);
            RuleFor(m => m.StartedAt).NotEmpty();

            RuleFor(m => m.ParentForum).NotEmpty().InjectValidator();
            RuleFor(m => m.StartedBy).NotEmpty().InjectValidator();

            RuleFor(m => m.LastPost).InjectValidator().When(m => m.LastPost != null);
        }
    }
}
