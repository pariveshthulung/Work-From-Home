using NetArchTest.Rules;
using Xunit;

namespace Clean.Test
{
    public class ArchitectureTest
    {
        private const string DomainNamespace = "Clean.Domain";
        private const string InfrastructureNamespace = "Clean.Infrastructure";
        private const string ApplicationNamespace = "Clean.Application";
        private const string ApiNamespace = "Clean.Api";

        [Fact]
        public void Domain_Should_Not_HaveDependencyOnOtherProject()
        {
            // arrange
            var assembly = typeof(Clean.Domain.AssemblyReference).Assembly;
            var otherProject = new[]
            {
                ApplicationNamespace,
                InfrastructureNamespace,
                ApiNamespace
            };
            // act
            var testResult = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAll(otherProject)
                .GetResult();

            // assert
            Assert.True(testResult.IsSuccessful);
        }

        [Fact]
        public void Application_Should_Not_HaveDependencyOnOtherProject()
        {
            //arrange
            var assembly = typeof(Clean.Application.AssemblyReference).Assembly;

            var otherProject = new[] { ApiNamespace, InfrastructureNamespace };

            //act
            var testResult = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAll(otherProject)
                .GetResult();

            //assert
            Assert.True(testResult.IsSuccessful);
        }

        [Fact]
        public void Infrastructure_Should_Not_HaveDependencyOnOtherProject()
        {
            //arrange
            var assembly = typeof(Clean.Infrastructure.AssemblyReference).Assembly;

            var otherProject = new[] { ApiNamespace, DomainNamespace };

            //act
            var testResult = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAll(otherProject)
                .GetResult();

            //assert
            Assert.True(testResult.IsSuccessful);
        }

        [Fact]
        public void Api_Should_Not_HaveDependencyOnOtherProject()
        {
            //arrange
            var assembly = typeof(Clean.Api.AssemblyReference).Assembly;

            //act
            var testResult = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOn(DomainNamespace)
                .GetResult();

            //assert
            Assert.True(testResult.IsSuccessful);
        }

        [Fact]
        public void Handlers_Should_HaveDependencyOnDomain()
        {
            //arrange
            var assembly = typeof(Clean.Application.AssemblyReference).Assembly;
            //act
            var testResult = Types
                .InAssembly(assembly)
                .That()
                .HaveNameEndingWith("Handler")
                .Should()
                .HaveDependencyOn(DomainNamespace)
                .GetResult();
            //assert
            Assert.True(testResult.IsSuccessful);
        }

        [Fact]
        public void Controllers_Should_HaveDependencyOnMediatR()
        {
            //arrange
            var assembly = typeof(Clean.Api.AssemblyReference).Assembly;
            //act
            var testResult = Types
                .InAssembly(assembly)
                .That()
                .HaveNameEndingWith("Controller")
                .Should()
                .HaveDependencyOn("MediatR")
                .GetResult();
            //assert
            Assert.True(testResult.IsSuccessful);
        }
    }
}
