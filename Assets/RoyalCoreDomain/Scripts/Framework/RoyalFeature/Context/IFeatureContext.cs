using System;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Context.Port;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Services;

namespace RoyalCoreDomain.Scripts.Framework.RoyalFeature.Context
{
    public interface IFeatureContext : IDisposable
    {
        FeatureContext Parent { get; }
        bool TryImportService<T>(out T s) where T : class, IService;
        T ImportService<T>() where T : class, IService;
        bool TryImportPort<T>(out T p) where T : class, IPort;
        T ImportPort<T>() where T : class, IPort;
        bool TryImportKeyed<T>(string id, out T p) where T : class, IPort;
        void Export<T>(T port) where T : class, IPort;
        void ExportKeyed<T>(string id, T port) where T : class, IPort;
        IPortRegistry EnsureLocalPortRegistry();
    }
}