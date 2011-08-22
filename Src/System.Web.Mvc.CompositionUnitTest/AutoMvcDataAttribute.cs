using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Xunit;

namespace System.Web.Mvc.CompositionUnitTest
{
    internal sealed class AutoMvcDataAttribute : AutoDataAttribute
    {
        public AutoMvcDataAttribute()
            : base(new Fixture().Customize(new MvcCustomization()))
        {
        }

        private sealed class MvcCustomization : ICustomization
        {
            public void Customize(IFixture fixture)
            {
                fixture.Customize(
                    new CompositeCustomization(
                        new MultipleCustomization(),
                        new AutoMoqCustomization()));
            }
        }
    }
}