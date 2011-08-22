using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;

namespace System.Web.Mvc.Composition
{
    public class CompositeControllerFactory : DefaultControllerFactory
    {
        private readonly IEnumerable<IControllerFactory> factories;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeControllerFactory"/> class.
        /// </summary>
        /// <param name="factories">The controller factories.</param>
        public CompositeControllerFactory(IEnumerable<IControllerFactory> factories) 
            :this(factories.ToArray())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeControllerFactory"/> class.
        /// </summary>
        /// <param name="factories">The controller factories.</param>
        public CompositeControllerFactory(params IControllerFactory[] factories)
        {
            if (factories == null)
            {
                throw new ArgumentNullException("factories");
            }

            this.factories = factories;
        }

        /// <summary>
        /// Gets the factories supplied through one of the constructors.
        /// </summary>
        public IEnumerable<IControllerFactory> Factories
        {
            get { return this.factories; }
        }

        /// <summary>
        /// Creates the specified controller by using the specified request context.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <returns>
        /// The controller.
        /// </returns>
        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            return (from factory in this.Factories
                    let controller = factory.CreateController(requestContext, controllerName)
                    where controller != null
                    select controller).FirstOrDefault();
        }
    }
}
