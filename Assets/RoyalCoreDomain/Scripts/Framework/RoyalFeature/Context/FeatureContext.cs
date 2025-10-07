using System;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Command;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Context.Locator;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Context.Port;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.MVC.Controller;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.MVC.Model;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.MVC.View;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Services;

namespace RoyalCoreDomain.Scripts.Framework.RoyalFeature.Context
{
    public sealed class FeatureContext : IFeatureContext
    {
        public readonly Locator<IController> Controllers = new();
        public readonly ListLocator<object> Lists = new(); // Multiple instances (ör. ITickable)
        public readonly Locator<IModel> Models = new();
        public readonly KeyedLocator<string, IPort> PortDirectory = new(); // Multiple port instances
        public readonly Locator<IPort> Ports = new(); // single instance port (facade)
        public readonly Locator<IService> Services = new();
        public readonly Locator<IView> Views = new();

        public FeatureContext(FeatureContext parent = null)
        {
            Parent = parent;
            CommandFactory = new CommandFactory(this);
        }

        public ICommandFactory CommandFactory { get; }

        public FeatureContext Parent { get; }

        // --- Import helpers (parent chain) ---
        public bool TryImportService<T>(out T s) where T : class, IService
        {
            return Services.TryGet(out s) || Parent?.TryImportService(out s) == true;
        }

        public T ImportService<T>() where T : class, IService
        {
            return TryImportService<T>(out var s)
                ? s
                : throw new InvalidOperationException($"Service {typeof(T).Name} not found");
        }

        public bool TryImportPort<T>(out T p) where T : class, IPort
        {
            return Ports.TryGet(out p) || Parent?.TryImportPort(out p) == true;
        }

        public T ImportPort<T>() where T : class, IPort
        {
            return TryImportPort(out T p)
                ? p
                : throw new InvalidOperationException($"Port {typeof(T).Name} not found");
        }

        // Keyed directory import
        public bool TryImportKeyed<T>(string id, out T p) where T : class, IPort
        {
            // First local
            if (PortDirectory.TryGet(id, out p)) return true;
            
            // Then parent chain
            var node = Parent;
            while (node != null)
            {
                if (node.PortDirectory.TryGet(id, out p)) return true;
                node = node.Parent;
            }

            return false;
        }

        // --- Export helpers (make visible to parent) ---
        public void Export<T>(T port) where T : class, IPort
        {
            Parent?.Ports.Bind(port);
        }

        public void ExportKeyed<T>(string id, T port) where T : class, IPort
        {
            Parent?.PortDirectory.Register(id, port);
        }

        // (optional) export IPortRegistry on the parent for dynamic discovery
        public IPortRegistry EnsureLocalPortRegistry()
        {
            if (!Ports.TryGet<IPortRegistry>(out var reg))
            {
                reg = new DynamicPortRegistry();
                Ports.Bind(reg);
            }

            return reg;
        }

        public void Dispose()
        {
        }
    }
}