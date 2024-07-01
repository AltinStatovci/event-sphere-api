using FluentValidation;
using EventSphere.Domain.DTOs;
using System.ComponentModel.DataAnnotations;

namespace EventSphere.Business.Validator
{
    public class EventValidator : AbstractValidator<EventDTO>
    {
        public EventValidator()
        {
            
            RuleFor(x => x.EventName)
                .NotEmpty().WithMessage("Event name is required.")
                .MaximumLength(50).WithMessage("Event name cannot be longer than 50 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description cannot be longer than 500 characters.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Location is required.")
                .MaximumLength(50).WithMessage("Location cannot be longer than 50 characters.");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required.");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("End date is required.")
                .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("End date must be after start date.");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Category ID is required.");

            RuleFor(x => x.OrganizerId)
                .NotEmpty().WithMessage("Organizer ID is required.");

            RuleFor(x => x.MaxAttendance)
                .NotEmpty().WithMessage("Max attendance is required.")
                .GreaterThanOrEqualTo(0).WithMessage("Max attendance cannot be negative.");

            RuleFor(x => x.AvailableTickets)
                .NotEmpty().WithMessage("Available tickets count is required.")
                .GreaterThanOrEqualTo(0).WithMessage("Available tickets count cannot be negative.")
                .LessThanOrEqualTo(x => x.MaxAttendance).WithMessage("Available tickets count cannot be greater than max attendance.");

            RuleFor(x => x.DateCreated)
                .NotEmpty().WithMessage("Date created is required.");
        }
    }
}
