using FluentValidation;
using NZWalks.API.Models.DTO.Regions;

namespace NZWalks.API.Validator
{
    public class UpdateRegionRequestDtoValidator : AbstractValidator<UpdateRegionRequestDto>
    {
        public UpdateRegionRequestDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
