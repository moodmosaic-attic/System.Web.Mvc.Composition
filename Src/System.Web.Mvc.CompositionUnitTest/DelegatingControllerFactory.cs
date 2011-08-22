using System.Web.Routing;
using System.Web.SessionState;

namespace System.Web.Mvc.CompositionUnitTest
{
    internal sealed class DelegatingControllerFactory : IControllerFactory
    {
        public DelegatingControllerFactory()
        {
            this.OnCreateController             = (r, c) => null;
            this.OnGetControllerSessionBehavior = (r, c) => SessionStateBehavior.ReadOnly;
            this.OnReleaseController            = delegate { };
        }

        public IController CreateController(RequestContext requestContext, string controllerName)
        {
            return this.OnCreateController(requestContext, controllerName);
        }

        public SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, string controllerName)
        {
            return this.OnGetControllerSessionBehavior(requestContext, controllerName);
        }

        public void ReleaseController(IController controller)
        {
            this.OnReleaseController(controller);
        }

        public Func<RequestContext, string, IController> OnCreateController { get; set; }

        public Func<RequestContext, string, SessionStateBehavior> OnGetControllerSessionBehavior { get; set; }
        
        public Action<IController> OnReleaseController { get; set; }
    }
}
