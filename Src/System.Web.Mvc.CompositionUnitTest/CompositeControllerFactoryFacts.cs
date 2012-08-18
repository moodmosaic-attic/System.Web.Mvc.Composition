using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc.Composition;
using System.Web.Routing;
using Moq;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace System.Web.Mvc.CompositionUnitTest
{
    public sealed class CompositeControllerFactoryFacts
    {
        [Theory, AutoMvcData]
        public void SutIsControllerFactory(CompositeControllerFactory sut)
        {
            Assert.IsAssignableFrom<IControllerFactory>(sut);
        }

        [Theory, AutoMvcData]
        public void FactoriesWillNotBeNullWhenSutIsCreatedWithDefaultConstructor(
            CompositeControllerFactory sut)
        {
            IEnumerable<IControllerFactory> result = sut.Factories;
            Assert.NotNull(result);
        }

        [Theory, AutoMvcData]
        public void FactoriesWillMatchParamsArray(IControllerFactory[] expectedFactories)
        {
            // Fixture setup
            var sut = new CompositeControllerFactory(expectedFactories);
            // Exercise system
            var result = sut.Factories;
            // Verify outcome
            Assert.True(expectedFactories.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullArrayThrows()
        {
            Assert.Throws<ArgumentNullException>(
                () => new CompositeControllerFactory(null)
                );
        }

        [Fact]
        public void InitializeWithNullEnumerableThrows()
        {
            Assert.Throws<ArgumentNullException>(
                () => new CompositeControllerFactory((IEnumerable<IControllerFactory>)null)
                );
        }

        [Theory, AutoMvcData]
        public void CreateControllerWillInvokeFactoriesWithCorrectArguments(
            string controllerName, 
            RequestContext requestContext,
            [Frozen]Mock<IControllerFactory> factoryMock,
            CompositeControllerFactory sut)
        {
            sut.CreateController(requestContext, controllerName);
            factoryMock.Verify(x => x.CreateController(requestContext, controllerName));
        }

        [Theory, AutoMvcData]
        public void CreateControllerWillReturnFirstControllerResultFromFactories(
            string controllerName, 
            RequestContext requestContext,
            IController expectedResult,
            Mock<IControllerFactory>[] factoryStubs)
        {
            // Fixture Setup
            factoryStubs[0]
                .Setup(x => x.CreateController(requestContext, controllerName))
                .Returns((IController)null);
            factoryStubs[1]
                .Setup(x => x.CreateController(requestContext, controllerName))
                .Returns(expectedResult);
            factoryStubs[2]
                .Setup(x => x.CreateController(requestContext, controllerName))
                .Returns((IController)null);

            var sut = new CompositeControllerFactory(factoryStubs.Select(x => x.Object));
            // Exercise system
            var result = sut.CreateController(requestContext, controllerName);
            // Verify outcome
            Assert.Equal(expectedResult, result);
            // Teardown
        }

        [Theory, AutoMvcData]
        public void CreateControllerWillReturnNullIfAllFactoriesReturnNull(
            string controllerName, 
            RequestContext requestContext,
            Mock<IControllerFactory>[] factoryStubs)
        {
            // Fixture Setup
            factoryStubs[0]
                .Setup(x => x.CreateController(requestContext, controllerName))
                .Returns((IController)null);
            factoryStubs[1]
                .Setup(x => x.CreateController(requestContext, controllerName))
                .Returns((IController)null);
            factoryStubs[2]
                .Setup(x => x.CreateController(requestContext, controllerName))
                .Returns((IController)null);
            
            var sut = new CompositeControllerFactory(factoryStubs.Select(x => x.Object));
            // Exercise system
            var result = sut.CreateController(requestContext, controllerName);
            // Verify outcome
            Assert.Null(result);
            // Teardown
        }
    }
}
