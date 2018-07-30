using System;
using System.DirectoryServices;
using ActiveDs;
using AutoFixture;
using AutoFixture.Idioms;
using Bstm.UnitTesting;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using static Bstm.DirectoryServices.DirectoryProperty;

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

        [Theory]
        [LocalTestData]
        public void DepartmentShouldBeStored(IUser user)
        {
            // Fixture setup
            var department = Fixture.Create<string>();

            // Exercise system
            user.Department = department;

            // Verify outcome
            user.Department.Should().Be(department);
            user.GetPropertyValue<string>(Department).Should().Be(department);
        }

        [Theory]
        [LocalTestData]
        public void DescriptionShouldBeStored(IUser user)
        {
            // Fixture setup
            var description = Fixture.Create<string>();

            // Exercise system
            user.Description = description;

            // Verify outcome
            user.Description.Should().Be(description);
            user.GetPropertyValue<string>(Description).Should().Be(description);
        }

        [Theory]
        [LocalTestData]
        public void DivisionShouldBeStored(IUser user)
        {
            // Fixture setup
            var division = Fixture.Create<string>();

            // Exercise system
            user.Division = division;

            // Verify outcome
            user.Division.Should().Be(division);
            user.GetPropertyValue<string>(Division).Should().Be(division);
        }

        [Theory]
        [LocalTestData]
        public void EmailAddressShouldBeStored(IUser user)
        {
            // Fixture setup
            var emailAddress = Fixture.Create<string>();

            // Exercise system
            user.EmailAddress = emailAddress;

            // Verify outcome
            user.EmailAddress.Should().Be(emailAddress);
            user.GetPropertyValue<string>(Mail).Should().Be(emailAddress);
        }

        [Theory]
        [LocalTestData]
        public void EmployeeIdShouldBeStored(IUser user)
        {
            // Fixture setup
            var employeeId = Fixture.Create<string>();

            // Exercise system
            user.EmployeeId = employeeId;

            // Verify outcome
            user.EmployeeId.Should().Be(employeeId);
            user.GetPropertyValue<string>(EmployeeId).Should().Be(employeeId);
        }

        [Theory]
        [LocalTestData]
        public void FaxNumberShouldBeStored(IUser user)
        {
            // Fixture setup
            var faxNumber = Fixture.Create<string>();

            // Exercise system
            user.FaxNumber = faxNumber;

            // Verify outcome
            user.FaxNumber.Should().Be(faxNumber);
            user.GetPropertyValue<string>(FacsimileTelephoneNumber).Should().Be(faxNumber);
        }

        [Theory]
        [LocalTestData]
        public void FirstNameShouldBeStored(IUser user)
        {
            // Fixture setup
            var firstName = Fixture.Create<string>();

            // Exercise system
            user.FirstName = firstName;

            // Verify outcome
            user.FirstName.Should().Be(firstName);
            user.GetPropertyValue<string>(GivenName).Should().Be(firstName);
        }

        [Theory]
        [LocalTestData]
        public void HomeDirectoryShouldBeStored(IUser user)
        {
            // Fixture setup
            var homeDirectory = Fixture.Create<string>();

            // Exercise system
            user.HomeDirectory = homeDirectory;

            // Verify outcome
            user.HomeDirectory.Should().Be(homeDirectory);
            user.GetPropertyValue<string>(HomeDirectory).Should().Be(homeDirectory);
        }

        [Theory]
        [LocalTestData]
        public void HomePageShouldBeStored(IUser user)
        {
            // Fixture setup
            var homePage = Fixture.Create<string>();

            // Exercise system
            user.HomePage = homePage;

            // Verify outcome
            user.HomePage.Should().Be(homePage);
            user.GetPropertyValue<string>(WwwHomePage).Should().Be(homePage);
        }

        [Theory]
        [LocalTestData]
        public void FullNameShouldBeStored(IUser user)
        {
            // Fixture setup
            var fullName = Fixture.Create<string>();

            // Exercise system
            user.FullName = fullName;

            // Verify outcome
            user.FullName.Should().Be(fullName);
            user.GetPropertyValue<string>(DisplayName).Should().Be(fullName);
        }

        [Theory]
        [LocalTestData]
        public void AccountDisabledShoulBeCorrectRead(IUser user)
        {
            // Fixture setup
            user.SetPropertyValue(UserAccountControl, ADS_USER_FLAG.ADS_UF_ACCOUNTDISABLE);

            // Exercise system and verify outcome
            user.AccountDisabled.Should().BeTrue();
        }

        [Theory]
        [LocalTestData]
        public void AccountDisabledShoulBeCorrectWritten(IUser user)
        {
            // Fixture setup
            user.SetPropertyValue(UserAccountControl, ADS_USER_FLAG.ADS_UF_NORMAL_ACCOUNT);

            // Exercise system
            user.AccountDisabled = true;

            // Verify outcome
            user.GetPropertyValue<ADS_USER_FLAG>(UserAccountControl)
                .HasFlag(ADS_USER_FLAG.ADS_UF_ACCOUNTDISABLE)
                .Should()
                .BeTrue();
        }

        [Theory]
        [LocalTestData]
        public void AccountDisabledShoulBeCorrectCleared(IUser user)
        {
            // Fixture setup
            user.SetPropertyValue(UserAccountControl, ADS_USER_FLAG.ADS_UF_ACCOUNTDISABLE);

            // Exercise system
            user.AccountDisabled = false;

            // Verify outcome
            user.GetPropertyValue<ADS_USER_FLAG>(UserAccountControl)
                .HasFlag(ADS_USER_FLAG.ADS_UF_ACCOUNTDISABLE)
                .Should()
                .BeFalse();
        }

        [Theory]
        [LocalTestData]
        public void AccountExpiresShouldBeStored(IUser user)
        {
            // Fixture setup
            var accountExpires = Fixture.Create<DateTime?>();

            // Exercise system
            user.AccountExpires = accountExpires;

            // Verify outcome
            user.AccountExpires.Should().Be(accountExpires);
        }

        [Theory]
        [LocalTestData]
        public void BadLoginCountShouldBeRead(IUser user)
        {
            // Fixture setup
            user.SetPropertyValue(BadPwdCount, 2);

            // Exercise system and verify outcome
            user.BadLoginCount.Should().Be(2);
        }

        [Theory]
        [LocalTestData]
        public void AccountLockedShouldBeRead(IUser user)
        {
            // Fixture setup
            user.SetPropertyValue(LockoutTime, 2L);

            // Exercise system and verify outcome
            user.AccountLocked.Should().BeTrue();
        }

        [Theory]
        [LocalTestData]
        public void LastFailedLoginAtShouldBeRead(IUser user)
        {
            // Fixture setup
            var lastFailedLogin = Fixture.Create<DateTime?>();
            user.SetPropertyValue(BadPasswordTime, lastFailedLogin);

            // Exercise system and verify outcome
            user.LastFailedLogin.Should().Be(lastFailedLogin);
        }

        [Theory]
        [LocalTestData]
        public void LastLoginShouldBeRead(IUser user)
        {
            // Fixture setup
            var lastLogin = Fixture.Create<DateTime?>();
            user.SetPropertyValue(LastLogon, lastLogin);

            // Exercise system and verify outcome
            user.LastLogin.Should().Be(lastLogin);
        }

        [Theory]
        [LocalTestData]
        public void LastLogoffShouldBeRead(IUser user)
        {
            // Fixture setup
            var lastLogoff = Fixture.Create<DateTime?>();
            user.SetPropertyValue(LastLogoff, lastLogoff);

            // Exercise system and verify outcome
            user.LastLogoff.Should().Be(lastLogoff);
        }

        [Theory]
        [LocalTestData]
        public void LastNameShouldBeStored(IUser user)
        {
            // Fixture setup
            var lastName = Fixture.Create<string>();

            // Exercise system
            user.LastName = lastName;

            // Verify outcome
            user.LastName.Should().Be(lastName);
            user.GetPropertyValue<string>(Sn).Should().Be(lastName);
        }

        private class LocalTestDataAttribute : AutoMoqDataAttribute
        {
            public LocalTestDataAttribute() : base(CreateFixture)
            {
            }

            private new static IFixture CreateFixture()
            {
                var fixture = AutoMoqDataAttribute.CreateFixture();
                var user = fixture.Build<User>().Without(u => u.AccountDisabled).Create();
                fixture.Inject((IUser) user);

                return fixture;
            }
        }
    }
}