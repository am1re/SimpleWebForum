using App.BusinessLogic.Resources.Post;
using FluentValidation;

namespace App.BusinessLogic.Validators.Post
{
    public class PostResourceValidator : AbstractValidator<PostResource>
    {
        public PostResourceValidator()
        {
            RuleFor(m => m.Id).NotEmpty();
            RuleFor(m => m.Content).NotEmpty().Length(2, 512);
            RuleFor(m => m.CreatedAt).NotEmpty();
            RuleFor(m => m.User).NotEmpty().InjectValidator();
        }
    }
}
