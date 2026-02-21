using FluentValidation;
using NZWalks.API.Models.DTO.Walks;

namespace NZWalks.API.Validator
{
    public class UpdateWalkRequestsDtoValidator : AbstractValidator<UpdateWalkRequestsDto>
    {
        public UpdateWalkRequestsDtoValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();

            RuleFor(x=> x.Description).NotNull().NotEmpty();

            RuleFor(x => x.LengthInKm).GreaterThan(0);
        }
    }
}
