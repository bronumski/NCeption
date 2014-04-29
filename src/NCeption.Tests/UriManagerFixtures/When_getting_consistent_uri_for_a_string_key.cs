using System;
using FluentAssertions;
using NUnit.Framework;

namespace NCeption.UriManagerFixtures
{
    [TestFixture]
    class When_getting_consistent_uri_for_a_string_key  
    {
        private Uri result;

        [SetUp]
        public void SetUp()
        {
            UriManager.Reset();
            result = UriManager.GetUriForKey("Foo");
        }

        [Test]
        public void Should_return_a_uri_with_an_http_schema_()
        {
            result.Scheme.Should().Be(Uri.UriSchemeHttp);
        }

        [Test]
        public void Should_return_a_uri_with_localhost_as_the_host_name()
        {
            result.Host.Should().Be("localhost");
        }

        [Test]
        public void Should_return_a_uri_with_a_valid_port_number()
        {
            result.Port.Should().BeGreaterThan(0);
        }

        [Test]
        public void Should_return_a_uri_where_the_key_name_is_part_of_the_path()
        {
            result.AbsolutePath.Should().Be("/");
        }

        [Test]
        public void Should_return_the_same_uri_everytime()
        {
            var secondUriGet = UriManager.GetUriForKey("Foo");

            secondUriGet.Should().Be(result);
        }

        [TearDown]
        public void TearDown()
        {
            UriManager.Reset();
        }
    }
}