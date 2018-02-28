using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NEdifis.Attributes;
using NUnit.Framework;

namespace NEdifis.Conventions
{
    [TestFixtureFor(typeof(ExcludeFromCodeCoverageClassHasBecauseAttribute))]
    // ReSharper disable InconsistentNaming
    internal class ExcludeFromCodeCoverageClassHasBecauseAttribute_Should
    {
        [ExcludeFromCodeCoverage]
        private class Excluded_From_Code_Without_Because { }

        [ExcludeFromCodeCoverage]
        [Because("this is a test")]
        private class Excluded_From_Code_With_Because { }

        [Test]
        public void Be_Creatable()
        {
            var sut = new ExcludeFromCodeCoverageClassHasBecauseAttribute();

            sut.Verify(typeof(Excluded_From_Code_With_Because));
            sut.Invoking(x => x.Verify(typeof(Excluded_From_Code_Without_Because))).Should().Throw<AssertionException>();
        }

        [Test, Issue("#6", Title = "convention implementations are private")]
        public void Be_Public()
        {
            var sut = new ExcludeFromCodeCoverageClassHasBecauseAttribute();
            sut.GetType().IsPublic.Should().BeTrue();
        }
    }
}
