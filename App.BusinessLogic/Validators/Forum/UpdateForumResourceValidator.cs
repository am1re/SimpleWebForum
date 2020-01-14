using App.BusinessLogic.Resources.Forum;
using FluentValidation;

namespace App.BusinessLogic.Validators.Forum
{
    public class UpdateForumResourceValidator : AbstractValidator<UpdateForumResource>
    {
        public UpdateForumResourceValidator()
        {
            RuleFor(m => m.Name).Length(2, 36).When(m => !string.IsNullOrEmpty(m.Name));
            RuleFor(m => m.Description).Length(2, 128).When(m => !string.IsNullOrEmpty(m.Description));
        }
    }
}
