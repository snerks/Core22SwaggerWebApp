using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Core22SwaggerWebApp.Models;
using Core22SwaggerWebApp.Models.Core22SwaggerWebApp.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Xunit;

namespace Core22SwaggerWebApp.UnitTest.Models
{
    public class SampleDto
    {
        [Range(1, 100)]
        public int IntegerRange { get; set; }
    }

    public class ApiErrorTests
    {
        [Fact]
        public void Test()
        {
            // Arrange
            //var input = new ModelStateDictionary();
            //input.AddModelError("SamplePropertyName", "Sample ErrorMessage");

            var sut = new PagingOptions
            {
                PageSize = 2000
            };

            var context = new ValidationContext(sut, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(sut, context, validationResults, true);

            // Act
            var result = validationResults;

            foreach (var validationResult in validationResults)
            {
                foreach (var memberName in validationResult.MemberNames)
                {
                    var keyParts = memberName.Split('.');
                    var camelKeyParts = new List<string>();

                    foreach (var keyPart in keyParts)
                    {
                        camelKeyParts
                            .Add(
                                keyPart.First().ToString().ToLowerInvariant() +
                                (keyPart.Length > 1 ? keyPart.Substring(1) : ""));
                    }

                    var newKey = camelKeyParts.Aggregate((i, j) => i + "." + j);
                }
            }

            // Assert
            result.Should().NotBeNull();
        }
    }
}
