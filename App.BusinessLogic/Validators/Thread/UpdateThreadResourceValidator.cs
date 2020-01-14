using App.BusinessLogic.Resources.Thread;
using FluentValidation;

namespace App.BusinessLogic.Validators.Thread
{
    public class UpdateThreadResourceValidator : AbstractValidator<UpdateThreadResource>
    {
        public UpdateThreadResourceValidator()
        {
            RuleFor(m => m.Subject).Length(2, 128).When(m => !string.IsNullOrEmpty(m.Subject));
            RuleFor(m => m.ParentForum).InjectValidator().When(m => m.ParentForum != null);
        }
    }
}
