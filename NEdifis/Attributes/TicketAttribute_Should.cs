using System;
using FluentAssertions;
using NUnit.Framework;

namespace NEdifis.Attributes
{
    [TestFixtureFor(typeof(TicketAttribute))]
    // ReSharper disable once InconsistentNaming
    class TicketAttribute_Should
    {
        [Test]
        public void Be_Creatable()
        {
            var ctx = new ContextFor<TicketAttribute>();
            var sut = ctx.BuildSut();

            sut.Should().NotBeNull();
            sut.Should().BeAssignableTo<Attribute>();
        }

        [Test]
        public void Have_Id()
        {
            var sut = new TicketAttribute(23);
            sut.Id.Should().Be(23);
            sut.Reference.Should().BeNull();
        }

        [Test]
        public void Have_Reference()
        {
            var sut = new TicketAttribute("#42");
            sut.Reference.Should().Be("#42");
            sut.Id.Should().NotHaveValue();
        }

        [Test]
        public void Have_Id_And_Reference()
        {
            var sut = new TicketAttribute(23, "#42");
            sut.Reference.Should().Be("#42");
            sut.Id.Should().Be(23);
        }
    }
}