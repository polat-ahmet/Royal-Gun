using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Scripts.Modules.Health;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Scripts.Modules.Movement;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Scripts.Modules.SpriteRender;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Weapon.Scripts.Services;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Enemy.Scripts.Controllers;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Player.Scripts.Models;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Player.Scripts.Views;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Scripts.Services.ControlledAgentService;
using RoyalCoreDomain.Scripts.Animation.Animator;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Feature;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Services.FeatureFactory;
using RoyalCoreDomain.Scripts.Framework.Template.RoyalFeatureTemplate.Scripts.Models;
using RoyalCoreDomain.Scripts.Framework.Template.RoyalFeatureTemplate.Scripts.Services;
using RoyalCoreDomain.Scripts.Framework.Template.RoyalFeatureTemplate.Scripts.Views;
using RoyalCoreDomain.Scripts.Services.Audio;
using RoyalCoreDomain.Scripts.Services.Pool;
using RoyalCoreDomain.Scripts.Services.Providers.ModelProvider;
using RoyalCoreDomain.Scripts.Services.Providers.ViewProvider;
using RoyalCoreDomain.Scripts.Services.UpdateService;
using UnityEngine;

namespace RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Enemy.Scripts.Feature
{
    public class EnemyFeature : BaseFeature
    {
        private readonly string _modelKey;
        private readonly Vector2 _spawnPos;
        private readonly string _viewKey;

        private EnemyController _controller;
        private AgentModel _model;
        private AgentView _view;

        public EnemyFeature(string address, IFeature parent, Vector2 spawnPos,
            string viewKey = "Enemy/Views/EnemyView",
            string modelKey = "Enemy/Models/EnemyModel")
            : base(address, parent)
        {
            _spawnPos = spawnPos;
            _viewKey = viewKey;
            _modelKey = modelKey;
        }

        protected override void OnInstall()
        {
            var models = Context.ImportService<IModelProvider>();
            var views = Context.ImportService<IViewProvider>();

            _view = views.LoadView<AgentView>(_viewKey);
            _view.transform.position = _spawnPos;
            _model = models.LoadSO<AgentModel>(_modelKey);

            var animation = new AnimatorController(_view);
            var health = new HealthController(_model);
            var movement = new MovementController(_view, _model);
            var render = new RenderController(_view);

            Context.Models.Bind(_model);
            Context.Views.Bind(_view);

            _controller = new EnemyController(_model, _view, animation, health, movement, render,
                Context.ImportService<IControlledAgentService>().TryGetTransform(),
                Context.ImportService<ITargetRegistry>(), Context.ImportService<IUpdateService<IFixedUpdatable>>(),
                Context.ImportService<IAudioService>());
            _controller.SetupCallbacks(OnEnemyDied);
            Context.Controllers.Bind(_controller);
        }

        protected override void OnStart()
        {
            Context.ImportService<IUpdateService<IUpdatable>>().RegisterUpdatable(_controller);
        }

        private void OnEnemyDied()
        {
            Context.ImportService<IFeatureFactory>().Remove(this);
        }

        protected override void OnDispose()
        {
            Debug.Log("Enemy Feature OnDispose");
            
            _controller.Dispose();
            Context.ImportService<IUpdateService<IUpdatable>>().UnregisterUpdatable(_controller);

            if (Context.TryImportService<IViewProvider>(out var vp))
            {
                Debug.Log("Enemy Feature View Destroyed");
                vp.Release(_view.gameObject);
            }
            _view = null;

        }
        
    }
}