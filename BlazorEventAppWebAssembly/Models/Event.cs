﻿using System.ComponentModel.DataAnnotations;

namespace BlazorEventAppWebAssembly.Models
{
    public class Event : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        [MinLength(20)]
        public string Location { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Description { get; set; }

        public EventType EventType { get; set; }

        public bool IsConfirmed { get; set; } // P32d3

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();

            if (EndDate <= StartDate)
            {
                errors.Add(new ValidationResult("End date must be later than start date.", new[] { nameof(EndDate) }));
            }

            if ((EndDate - StartDate).TotalHours < 2)
            {
                errors.Add(new ValidationResult("There must be at least a 2-hour difference between start date and end date.", new[] { nameof(EndDate), nameof(StartDate) }));
            }

            return errors;
        }
    }
}
