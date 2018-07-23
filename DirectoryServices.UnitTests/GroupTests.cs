﻿using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using AutoFixture;
using AutoFixture.Idioms;
using Bstm.DirectoryServices;
using Bstm.UnitTesting;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bstm.DirectoryServices.UnitTests
{
    public class GroupTests : TestBase
    {
        public GroupTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            Fixture.Inject(new DirectoryEntry());
        }

        [Fact]
        public void PublicInterfaceThrowsExceptionForNullArguments()
        {
            // Fixture setup
            var assertion = new GuardClauseAssertion(Fixture, new NullReferenceBehaviorExpectation());

            // Exercise system and verify outcome
            assertion.Verify(typeof(Group));
        }

        [Theory]
        [InlineData(GroupScope.Global)]
        [InlineData(GroupScope.Local)]
        [InlineData(GroupScope.Universal)]
        public void GroupTypeShouldBeStored(GroupScope groupScope)
        {
            // Fixture setup
            var group = Fixture.Create<DirectoryServices.Group>();

            // Exercise system
            group.GroupScope = groupScope;

            // Verify outcome
            group.GroupScope.Should().Be(groupScope);
        }
    }
}