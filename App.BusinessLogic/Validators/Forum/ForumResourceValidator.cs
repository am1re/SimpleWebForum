using App.BusinessLogic.Resources.Forum;
using FluentValidation;

namespace App.BusinessLogic.Validators.Forum
{
    public class ForumResourceValidator : AbstractValidator<ForumResource>
    {
        public ForumResourceValidator()
        {
            RuleFor(m => m.Id).NotEmpty();
            RuleFor(m => m.Name).NotEmpty().Length(2, 64);
            RuleFor(m => m.Description).Length(2, 128).When(m => !string.IsNullOrEmpty(m.Description));
            RuleFor(m => m.IsActive).NotEmpty();

            RuleFor(m => m.LastPost).InjectValidator().When(m => m.LastPost != null);
            RuleFor(m => m.Moderators).InjectValidator().When(m => m.Moderators != null);
        }
    }
}
