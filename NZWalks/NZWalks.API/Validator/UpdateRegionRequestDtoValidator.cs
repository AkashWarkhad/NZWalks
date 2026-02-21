using FluentValidation;
using NZWalks.API.Models.DTO.Regions;

namespace NZWalks.API.Validator
{
    public class UpdateRegionRequestDtoValidator : AbstractValidator<UpdateRegionRequestDto>
    {
        public UpdateRegionRequestDtoValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();

            RuleFor(x => x.Code).NotNull().NotEmpty();
        }
    }
}
