using System;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Feature;

namespace RoyalCoreDomain.Scripts.Framework.Template.RoyalFeatureTemplate.Scripts.Feature
{
    public class DummyFeature : BaseFeature
    {
        public DummyFeature(string address, IFeature parent = null) : base(address, parent)
        {
        }

        protected override void OnInstall()
        {
            throw new NotImplementedException();
        }
    }
}