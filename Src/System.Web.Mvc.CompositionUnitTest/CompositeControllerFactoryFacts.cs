using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc.Composition;
using System.Web.Routing;
using Xunit;
using Xunit.Extensions;

namespace System.Web.Mvc.CompositionUnitTest
{
    public sealed class CompositeControllerFactoryFacts
    {
        [Theory, AutoMvcData]
        public void SutIsControllerFactory(CompositeControllerFactory sut)
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.IsAssignableFrom<IControllerFactory>(sut);
            // Teardown
        }

        [Fact]
        public void FactoriesWillNotBeNullWhenSutIsCreatedWithDefaultConstructor()
        {
            // Fixture setup
            var sut = new CompositeControllerFactory();
            // Exercise system
            IEnumerable<IControllerFactory> result = sut.Factories;
            // Verify outcome
            Assert.NotNull(result);
            // Teardown
        }

        [Fact]
        public void FactoriesWillMatchParamsArray()
        {
            // Fixture setup
            var expectedFactories = new IControllerFactory[]
            {
                new DelegatingControllerFactory(),
                new DelegatingControllerFactory(),
                new DelegatingControllerFactory()
            };
            var sut = new CompositeControllerFactory(expectedFactories[0], expectedFactories[1], expectedFactories[2]);
            // Exercise system
            var result = sut.Factories;
            // Verify outcome
            Assert.True(expectedFactories.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullArrayThrows()
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.Throws<ArgumentNullException>(
                () => new CompositeControllerFactory(null)
                );
            // Teardown
        }

        [Fact]
        public void FactoriesWillMatchListParameter()
        {
            // Fixture setup
            var expectedFactories = new IControllerFactory[]
            {
                new DelegatingControllerFactory(),
                new DelegatingControllerFactory(),
                new DelegatingControllerFactory()
            };
            var sut = new CompositeControllerFactory(expectedFactories);
            // Exercise system
            var result = sut.Factories;
            // Verify outcome
            Assert.True(expectedFactories.SequenceEqual(result));
            // Teardown
        }

        [Fact]
        public void InitializeWithNullEnumerableThrows()
        {
            // Fixture setup
            // Exercise system
            // Verify outcome
            Assert.Throws<ArgumentNullException>(
                () => new CompositeControllerFactory((IEnumerable<IControllerFactory>)null)
                );
            // Teardown
        }

        [Theory, AutoMvcData]
        public void CreateControllerWillInvokeFactoriesWithCorrectRequestContext(RequestContext requestContext, string controllerName)
        {
            // Fixture setup
            var verified = false;
            var factory  = new DelegatingControllerFactory
                               {
                OnCreateController = (r, c) =>
                {
                    Assert.Equal(requestContext, r);
                    verified = true;
                    return null;
                }
            };
            var sut = new CompositeControllerFactory(factory);
            // Exercise system
            sut.CreateController(requestContext, controllerName);
            // Verify outcome
            Assert.True(verified);
            // Teardown
        }

        [Theory, AutoMvcData]
        public void CreateControllerWillInvokeFactoriesWithCorrectControllerName(RequestContext requestContext, string controllerName)
        {
            // Fixture setup
            var verified = false;
            var factory  = new DelegatingControllerFactory
                               {
                OnCreateController = (r, c) =>
                {
                    Assert.Equal(controllerName, c);
                    verified = true;
                    return null;
                }
            };
            var sut = new CompositeControllerFactory(factory);
            // Exercise system
            sut.CreateController(requestContext, controllerName);
            // Verify outcome
            Assert.True(verified);
            // Teardown
        }

        [Theory, AutoMvcData]
        public void CreateControllerWillReturnFirstControllerResultFromFactories(RequestContext requestContext, string controllerName, IController controller)
        {
            // Fixture setup
            var factories = new IControllerFactory[]
            {
                new DelegatingControllerFactory { OnCreateController = (r, c) => null       },
                new DelegatingControllerFactory { OnCreateController = (r, c) => controller },
                new DelegatingControllerFactory { OnCreateController = (r, c) => null       }
            };
            var sut = new CompositeControllerFactory(factories);
            // Exercise system
            var result = sut.CreateController(requestContext, controllerName);
            // Verify outcome
            Assert.Equal(controller, result);
            // Teardown
        }

        [Theory, AutoMvcData]
        public void CreateControllerWillReturnNullIfAllFactoriesReturnNull(RequestContext requestContext, string controllerName)
        {
            // Fixture setup
            var factories = new IControllerFactory[]
            {
                new DelegatingControllerFactory { OnCreateController = (r, c) => null },
                new DelegatingControllerFactory { OnCreateController = (r, c) => null },
                new DelegatingControllerFactory { OnCreateController = (r, c) => null }
            };
            var sut = new CompositeControllerFactory(factories);
            // Exercise system
            var result = sut.CreateController(requestContext, controllerName);
            // Verify outcome
            Assert.Null(result);
            // Teardown
        }
    }
}
