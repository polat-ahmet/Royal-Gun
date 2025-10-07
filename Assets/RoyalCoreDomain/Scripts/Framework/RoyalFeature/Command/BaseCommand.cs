using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Context;

namespace RoyalCoreDomain.Scripts.Framework.RoyalFeature.Command
{
    public abstract class BaseCommand : IBaseCommand, IResolvable
    {
        public virtual void Resolve(FeatureContext context)
        {
        }
    }
}