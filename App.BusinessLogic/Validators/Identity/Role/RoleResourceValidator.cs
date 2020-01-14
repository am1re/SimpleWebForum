using App.BusinessLogic.Resources.Identity.Role;
using FluentValidation;

namespace App.BusinessLogic.Validators.Identity.Role
{
    public class RoleResourceValidator : AbstractValidator<RoleResource>
    {
        public RoleResourceValidator()
        {
            RuleFor(m => m.Id).NotEmpty();
            RuleFor(m => m.Name).NotEmpty();
        }
    }
}
