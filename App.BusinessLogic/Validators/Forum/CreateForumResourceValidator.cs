using App.BusinessLogic.Resources.Forum;
using FluentValidation;

namespace App.BusinessLogic.Validators.Forum
{
    public class CreateForumResourceValidator : AbstractValidator<CreateForumResource>
    {
        public CreateForumResourceValidator()
        {
            RuleFor(m => m.Name).NotEmpty().Length(2, 36);
            RuleFor(m => m.Description).Length(2, 128).When(m => !string.IsNullOrEmpty(m.Description));
        }
    }
}
