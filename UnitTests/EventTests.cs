using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BlazorEventAppWebAssembly.Models;
using Xunit;

namespace UnitTests
{
    public class EventTests
    {
        [Fact]
        public void Validate_EndDateBeforeStartDate_ReturnsValidationError()
        {
            // Arrange
            var eventInstance = new Event
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddHours(-1)
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(eventInstance);
            Validator.TryValidateObject(eventInstance, validationContext, validationResults, true);

            // Assert
            Assert.Contains(validationResults, v => v.ErrorMessage == "End date must be later than start date.");
        }

        [Fact]
        public void Validate_LessThanTwoHoursDifference_ReturnsValidationError()
        {
            // Arrange
            var eventInstance = new Event
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddHours(1)
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(eventInstance);
            Validator.TryValidateObject(eventInstance, validationContext, validationResults, true);

            // Assert
            Assert.Contains(validationResults, v => v.ErrorMessage == "There must be at least a 2-hour difference between start date and end date.");
        }

        [Fact]
        public void Validate_ValidEvent_ReturnsNoValidationErrors()
        {
            // Arrange
            var eventInstance = new Event
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddHours(3)
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(eventInstance);
            Validator.TryValidateObject(eventInstance, validationContext, validationResults, true);

            // Assert
            Assert.Empty(validationResults);
        }
    }
}
