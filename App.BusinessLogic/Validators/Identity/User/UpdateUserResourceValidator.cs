using App.BusinessLogic.Resources.Identity.User;
using FluentValidation;

namespace App.BusinessLogic.Validators.Identity.User
{
    public class UpdateUserResourceValidator : AbstractValidator<UpdateUserResource>
    {
        public UpdateUserResourceValidator()
        {
            RuleFor(m => m.FirstName).Length(2, 32).When(m => string.IsNullOrEmpty(m.FirstName));
            RuleFor(m => m.LastName).Length(2, 32).When(m => string.IsNullOrEmpty(m.LastName));
            RuleFor(m => m.Email).EmailAddress().When(m => string.IsNullOrEmpty(m.Email));
        }
    }
}
