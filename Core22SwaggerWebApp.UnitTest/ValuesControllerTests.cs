using System;
using System.Collections.Generic;
using AutoFixture;
using AutoFixture.AutoMoq;
using Core22SwaggerWebApp.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Core22SwaggerWebApp.UnitTest
{
    public class ValuesControllerTests
    {
        [Fact]
        public void Test1() => Assert.True(true);

        [Fact]
        public void Get_returns_correct_result()
        {
            // Arrange
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            // var sut = new ValuesController(new Mock<ILogger<ValuesController>>().Object);
            var sut = fixture.Create<ValuesController>();

            // Act
            var result = sut.Get();

            // Assert
            Assert.NotNull(result);
            result.Should().BeAssignableTo<ActionResult<IEnumerable<CurrencyGetViewModel>>>();

            var resultValue = result.Value;
            Assert.NotEmpty(resultValue);
        }
    }
}
