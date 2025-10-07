using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Context;

namespace RoyalCoreDomain.Scripts.Framework.RoyalFeature.MVC.Controller
{
    public abstract class BaseController : IController, IResolvable
    {
        public virtual void Dispose()
        {
        }

        public virtual void Resolve(FeatureContext context)
        {
        }
    }
}