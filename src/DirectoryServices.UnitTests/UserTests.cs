using System.DirectoryServices;
using AutoFixture;
using AutoFixture.Idioms;
using Bstm.UnitTesting;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bstm.DirectoryServices.UnitTests
{
    public class UserTests : TestBase
    {
        public UserTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            Fixture.Inject(new DirectoryEntry());
        }

        [Theory]
        [AutoMoqData]
        public void PublicInterfaceThrowsExceptionForNullArguments(GuardClauseAssertion assertion)
        {
            // Fixture setup

            // Exercise system and verify outcome
            assertion.Verify(typeof(User).GetConstructors());
        }

        [Fact]
        public void DepartmentShouldBeStored()
        {
            // Fixture setup
            var user = Fixture.Create<User>();
            var department = Fixture.Create<string>();

            // Exercise system
            user.Department = department;

            // Verify outcome
            user.Department.Should().Be(department);
            user.GetPropertyValue<string>(DirectoryProperty.Department).Should().Be(department);
        }

        [Fact]
        public void DescriptionShouldBeStored()
        {
            // Fixture setup
            var user = Fixture.Create<User>();
            var description = Fixture.Create<string>();

            // Exercise system
            user.Description = description;

            // Verify outcome
            user.Description.Should().Be(description);
            user.GetPropertyValue<string>(DirectoryProperty.Description).Should().Be(description);
        }

        [Fact]
        public void DivisionShouldBeStored()
        {
            // Fixture setup
            var user = Fixture.Create<User>();
            var division = Fixture.Create<string>();

            // Exercise system
            user.Division = division;

            // Verify outcome
            user.Division.Should().Be(division);
            user.GetPropertyValue<string>(DirectoryProperty.Division).Should().Be(division);
        }

        [Fact]
        public void EmailAddressShouldBeStored()
        {
            // Fixture setup
            var user = Fixture.Create<User>();
            var emailAddress = Fixture.Create<string>();

            // Exercise system
            user.EmailAddress = emailAddress;

            // Verify outcome
            user.EmailAddress.Should().Be(emailAddress);
            user.GetPropertyValue<string>(DirectoryProperty.Mail).Should().Be(emailAddress);
        }

        [Fact]
        public void EmployeeIdShouldBeStored()
        {
            // Fixture setup
            var user = Fixture.Create<User>();
            var employeeId = Fixture.Create<string>();

            // Exercise system
            user.EmployeeId = employeeId;

            // Verify outcome
            user.EmployeeId.Should().Be(employeeId);
            user.GetPropertyValue<string>(DirectoryProperty.EmployeeId).Should().Be(employeeId);
        }

        [Fact]
        public void FaxNumberShouldBeStored()
        {
            // Fixture setup
            var user = Fixture.Create<User>();
            var faxNumber = Fixture.Create<string>();

            // Exercise system
            user.FaxNumber = faxNumber;

            // Verify outcome
            user.FaxNumber.Should().Be(faxNumber);
            user.GetPropertyValue<string>(DirectoryProperty.FacsimileTelephoneNumber).Should().Be(faxNumber);
        }

        [Fact]
        public void FirstNameShouldBeStored()
        {
            // Fixture setup
            var user = Fixture.Create<User>();
            var firstName = Fixture.Create<string>();

            // Exercise system
            user.FirstName = firstName;

            // Verify outcome
            user.FirstName.Should().Be(firstName);
            user.GetPropertyValue<string>(DirectoryProperty.GivenName).Should().Be(firstName);
        }

        [Fact]
        public void HomeDirectoryShouldBeStored()
        {
            // Fixture setup
            var user = Fixture.Create<User>();
            var homeDirectory = Fixture.Create<string>();

            // Exercise system
            user.HomeDirectory = homeDirectory;

            // Verify outcome
            user.HomeDirectory.Should().Be(homeDirectory);
            user.GetPropertyValue<string>(DirectoryProperty.HomeDirectory).Should().Be(homeDirectory);
        }

        [Fact]
        public void HomePageShouldBeStored()
        {
            // Fixture setup
            var user = Fixture.Create<User>();
            var homePage = Fixture.Create<string>();

            // Exercise system
            user.HomePage = homePage;

            // Verify outcome
            user.HomePage.Should().Be(homePage);
            user.GetPropertyValue<string>(DirectoryProperty.WwwHomePage).Should().Be(homePage);
        }

        [Fact]
        public void FullNameShouldBeStored()
        {
            // Fixture setup
            var user = Fixture.Create<User>();
            var fullName = Fixture.Create<string>();

            // Exercise system
            user.FullName = fullName;

            // Verify outcome
            user.FullName.Should().Be(fullName);
            user.GetPropertyValue<string>(DirectoryProperty.DisplayName).Should().Be(fullName);
        }
    }
}