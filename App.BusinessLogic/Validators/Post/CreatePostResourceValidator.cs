using App.BusinessLogic.Resources.Post;
using FluentValidation;

namespace App.BusinessLogic.Validators.Post
{
    public class CreatePostResourceValidator : AbstractValidator<CreatePostResource>
    {
        public CreatePostResourceValidator()
        {
            RuleFor(m => m.Content).NotEmpty().Length(2, 512);
            RuleFor(m => m.ThreadId).NotEmpty();
            RuleFor(m => m.User).NotEmpty().InjectValidator();
        }
    }
}
