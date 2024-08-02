using EventSphere.Domain.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Business.Validator
{
    public class RCEValidator : AbstractValidator<RCEventDTO>
    {
        public RCEValidator() 
        {
            RuleFor(x => x.UserId)
                    .NotEmpty().WithMessage("UserId is required.");
            RuleFor(x => x.EventId)
                    .NotEmpty().WithMessage("EventId is required.");
            RuleFor(x => x.Ecount)
                    .NotEmpty().WithMessage("Ecount is required.");
        }
    }
}
