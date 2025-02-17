using FluentValidation;
using NZWalks.API.Models.DTO.Regions;

namespace NZWalks.API.Validator
{
    public class AddRegionRequestDtoValidator : AbstractValidator<AddRegionRequestDto>
    {
        public AddRegionRequestDtoValidator()
        {
            RuleFor(x=> x.Name).NotEmpty();
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
