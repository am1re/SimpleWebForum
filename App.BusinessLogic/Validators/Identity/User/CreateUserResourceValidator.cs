using App.BusinessLogic.Resources.Identity.User;
using FluentValidation;

namespace App.BusinessLogic.Validators.Identity.User
{
    public class CreateUserResourceValidator : AbstractValidator<CreateUserResource>
    {
        public CreateUserResourceValidator()
        {
            RuleFor(m => m.UserName).NotEmpty().Length(2, 16);
            RuleFor(m => m.Password).NotEmpty().Length(2, 32);
            RuleFor(m => m.FirstName).Length(2, 32).When(m => string.IsNullOrEmpty(m.FirstName));
            RuleFor(m => m.LastName).Length(2, 32).When(m => string.IsNullOrEmpty(m.LastName));
            RuleFor(m => m.Email).EmailAddress().When(m => string.IsNullOrEmpty(m.Email));
        }
    }
}
