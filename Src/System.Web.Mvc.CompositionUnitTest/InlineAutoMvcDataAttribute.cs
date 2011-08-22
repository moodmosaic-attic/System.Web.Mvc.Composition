using Ploeh.AutoFixture.Xunit;

namespace System.Web.Mvc.CompositionUnitTest
{
    internal sealed class InlineAutoMvcDataAttribute : InlineAutoDataAttribute
    {
        public InlineAutoMvcDataAttribute(params object[] values)
            : base(new AutoMvcDataAttribute(), values)
        {
        }
    }
}
