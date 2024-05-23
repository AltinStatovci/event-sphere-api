using EventSphere.Domain.DTOs.EventSphere.API.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Business.Validator
{
    public class EventCategoryValidator : AbstractValidator<EventCategoryDto>
    {
        public EventCategoryValidator()
        {
            RuleFor(x => x.CategoryName)
                .NotEmpty().WithMessage("CategoryName is required.")
                .MaximumLength(50).WithMessage("CategoryName cannot be longer than 50 characters.");
        }
    }
}
