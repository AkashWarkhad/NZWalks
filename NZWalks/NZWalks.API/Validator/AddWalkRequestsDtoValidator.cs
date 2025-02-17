using FluentValidation;
using NZWalks.API.Models.DTO.Walks;

namespace NZWalks.API.Validator
{
    public class AddWalkRequestsDtoValidator : AbstractValidator<AddWalkRequestsDto>
    {
        public AddWalkRequestsDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x=> x.LengthInKm).GreaterThan(0);
        }
    }
}
