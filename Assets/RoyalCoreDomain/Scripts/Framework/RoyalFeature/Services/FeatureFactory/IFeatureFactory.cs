using System;
using System.Threading;
using System.Threading.Tasks;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Feature;

namespace RoyalCoreDomain.Scripts.Framework.RoyalFeature.Services.FeatureFactory
{
    public interface IFeatureFactory : IService
    {
        T Create<T>(IFeature parent, string name, Func<string, IFeature, T> create, string id = null)
            where T : IFeature;
        Task<T> CreateAsync<T>(IFeature parent, string name, Func<string, IFeature, T> create, CancellationTokenSource ct,
            string id = null)
            where T : IFeature;
        
        T CreateAndStart<T>(IFeature parent, string name, Func<string, IFeature, T> create, string id = null)
            where T : IFeature;

        Task<T> CreateAsyncAndStart<T>(IFeature parent, string name, Func<string, IFeature, T> create,
            CancellationTokenSource ct,
            string id = null)
            where T : IFeature;
        
        void Remove(IFeature feature);
        
    }
}