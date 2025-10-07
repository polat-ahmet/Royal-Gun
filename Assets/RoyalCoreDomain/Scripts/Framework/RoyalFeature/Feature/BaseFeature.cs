using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Context;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Services.FeatureRegistry;
using UnityEngine;

namespace RoyalCoreDomain.Scripts.Framework.RoyalFeature.Feature
{
    public abstract class BaseFeature : IFeature
    {
        private readonly List<IFeature> _children = new();

        protected BaseFeature(string address, IFeature parent = null)
        {
            Address = address ?? throw new ArgumentNullException(nameof(address));
            Parent = parent;
            Context = new FeatureContext((parent as BaseFeature)?.Context);
            Parent?.AddChild(this);
        }

        public string Address { get; protected set; }
        public IFeature Parent { get; private set; }
        public IReadOnlyList<IFeature> Children => _children;
        public FeatureState State { get; private set; } = FeatureState.New;
        public FeatureContext Context { get; }
        
        protected readonly List<(string name, Func<string, IFeature, IFeature> factory)> _plannedChildren = new();
        
        protected void PlanChild(string name, Func<string, IFeature, IFeature> factory)
            => _plannedChildren.Add((name, factory));

        public void PreInstall()
        {
            if (State != FeatureState.New) return;
            Debug.Log(Address + " Feature PreInstalling");
            
            OnPreInstall();
            
            if (Context.TryImportService<IFeatureRegistry>(out var reg))
                reg.Register(this);
            
            Debug.Log(Address + " Feature PreInstalled");
            State = FeatureState.PreInstalled;
            
            foreach (var (name, fac) in _plannedChildren)
            {
                if (fac == null) throw new ArgumentNullException(nameof(fac));
                var address = FeatureAddress.ChildOf(Address, name);
                
                var ch = fac(address, this);
                ch.PreInstall();
                
                _children.Add(ch);
            }
        }
        
        public void Install()
        {
            if (State != FeatureState.PreInstalled) return;
            Debug.Log(Address + " Feature Installing");
            OnInstall();
            Debug.Log(Address + " Feature Installed");
            State = FeatureState.Installed;
            
            foreach (var c in _children) c.Install();
        }
        
        public void Resolve()
        {
            if(State != FeatureState.Installed) return;
            Debug.Log(Address + " Feature Resolving");
            foreach (var c in _children) c.Resolve();
            OnResolve();
            Debug.Log(Address + " Feature Resolved");
            State = FeatureState.Resolved;
        }
        
        public async Task WarmupAsync(CancellationTokenSource ct)
        {
            Debug.Log(Address + " Feature Warming");
            await OnWarmupAsync(ct);
            Debug.Log(Address + " Feature Warmed up");
            // State = FeatureState.Warmed;
        }

        public void Start()
        {
            if (State != FeatureState.Installed && State != FeatureState.Resolved && State != FeatureState.Stopped) return;
            Debug.Log(Address + " Feature Starting");
            foreach (var c in _children) c.Start();
            OnStart();
            Debug.Log(Address + " Feature Started");
            State = FeatureState.Started;
        }

        public void Pause()
        {
            if (State != FeatureState.Started) return;
            Debug.Log(Address + " Feature Pausing");
            foreach (var c in _children) c.Pause();
            OnPause();
            Debug.Log(Address + " Feature Paused");
            State = FeatureState.Paused;
        }

        public void Resume()
        {
            if (State != FeatureState.Paused) return;
            Debug.Log(Address + " Feature Resuming");
            foreach (var c in _children) c.Resume();
            OnResume();
            Debug.Log(Address + " Feature Resumed");
            State = FeatureState.Started;
        }

        public void Stop()
        {
            if (State != FeatureState.Started && State != FeatureState.Paused) return;
            Debug.Log(Address + " Feature Stopping");
            foreach (var c in _children) c.Stop();
            OnStop();
            Debug.Log(Address + " Feature Stopped");
            State = FeatureState.Stopped;
        }

        public void Dispose()
        {
            if (State == FeatureState.Disposed) return;
            Debug.Log(Address + " Feature Disposing");
            Stop();
            for (var i = _children.Count - 1; i >= 0; i--) _children[i].Dispose();
            OnDispose();
            Debug.Log(Address + " Feature Disposed");

            // Context Clear
            Context.Services.Clear();
            Context.Controllers.Clear();
            Context.Models.Clear();
            Context.Views.Clear();
            Context.Ports.Clear();
            Context.PortDirectory.Clear();
            Context.Lists.Clear();

            if (Context.TryImportService<IFeatureRegistry>(out var reg))
                reg.Unregister(Address);

            State = FeatureState.Disposed;
        }

        public void AddChild(IFeature child)
        {
            if (child == null) throw new ArgumentNullException(nameof(child));
            if (_children.Contains(child)) return;
            (child as BaseFeature)!.Parent = this;
            _children.Add(child);
        }

        public void RemoveChild(IFeature child)
        {
            if (child == null) return;
            _children.Remove(child);
            (child as BaseFeature)!.Parent = null;
        }

        protected virtual void OnPreInstall()
        {
            
        }
        
        protected virtual void OnInstall()
        {
        }
        
        protected virtual Task OnWarmupAsync(CancellationTokenSource ct) => Task.CompletedTask;
        
        protected virtual void OnResolve()
        {
        }

        protected virtual void OnStart()
        {
        }

        protected virtual void OnPause()
        {
        }

        protected virtual void OnResume()
        {
        }

        protected virtual void OnStop()
        {
        }

        protected virtual void OnDispose()
        {
        }
    }
}