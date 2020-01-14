using App.BusinessLogic.Resources.Post;
using FluentValidation;

namespace App.BusinessLogic.Validators.Post
{
    public class UpdatePostResourceValidator : AbstractValidator<UpdatePostResource>
    {
        public UpdatePostResourceValidator()
        {
            RuleFor(m => m.Content).NotEmpty().Length(2, 512);
        }
    }
}
