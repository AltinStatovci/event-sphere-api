using System;
using System.Collections.Generic;

namespace EventSphere.API.Filters
{
    public class ValidationException : Exception
    {
        public List<string> Errors { get; set; }
        public ValidationException(List<string> errors) : base("Validation failed")
        {
            Errors = errors;
        }
    }
}
