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
                Name = "Some dummy name",
                Location = "Some dummy location to validate",
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
                Name = "Some dummy name",
                Location = "Some dummy location to validate",
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
                Name = "Some dummy name",
                Location = "Some dummy location to validate",
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

        [Fact]
        public void Validate_NameIsRequired_ReturnsValidationError()
        {
            // Arrange
            var eventInstance = new Event
            {
                Name = null,
                Location = "A valid location with more than 20 characters",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddHours(3)
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(eventInstance);
            Validator.TryValidateObject(eventInstance, validationContext, validationResults, true);

            // Assert
            Assert.Contains(validationResults, v => v.ErrorMessage == "The Name field is required.");
        }

        [Fact]
        public void Validate_NameMinLength_ReturnsValidationError()
        {
            // Arrange
            var eventInstance = new Event
            {
                Name = "AB",
                Location = "A valid location with more than 20 characters",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddHours(3)
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(eventInstance);
            Validator.TryValidateObject(eventInstance, validationContext, validationResults, true);

            // Assert
            Assert.Contains(validationResults, v => v.ErrorMessage == "The field Name must be a string or array type with a minimum length of '3'.");
        }

        [Fact]
        public void Validate_LocationIsRequired_ReturnsValidationError()
        {
            // Arrange
            var eventInstance = new Event
            {
                Name = "Valid Name",
                Location = null,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddHours(3)
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(eventInstance);
            Validator.TryValidateObject(eventInstance, validationContext, validationResults, true);

            // Assert
            Assert.Contains(validationResults, v => v.ErrorMessage == "The Location field is required.");
        }

        [Fact]
        public void Validate_LocationMinLength_ReturnsValidationError()
        {
            // Arrange
            var eventInstance = new Event
            {
                Name = "Valid Name",
                Location = "Short location",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddHours(3)
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(eventInstance);
            Validator.TryValidateObject(eventInstance, validationContext, validationResults, true);

            // Assert
            Assert.Contains(validationResults, v => v.ErrorMessage == "The field Location must be a string or array type with a minimum length of '20'.");
        }

    }
}
