﻿using FluentAssertions;
using NCeption.ServiceManagerFixtures;
using NUnit.Framework;

namespace NCeption.NUnit.RequiresServiceAttributeFixtures
{
    [RequireServices(typeof(StartableService1), typeof(StartableService2))]
    class When_running_a_test_fixture_that_requires_multiple_services
    {
        [Test]
        public void Should_start_up_service()
        {
            StartableService1.IsRunning.Should().BeTrue();
        }
    }
}