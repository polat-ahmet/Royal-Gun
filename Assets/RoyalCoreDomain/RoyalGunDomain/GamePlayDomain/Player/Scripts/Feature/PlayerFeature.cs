using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Scripts.Modules.Health;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Scripts.Modules.Movement;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Scripts.Modules.SpriteRender;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Weapon.Scripts.Feature;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Weapon.Scripts.WeaponHolder;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Player.Scripts.Controllers;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Player.Scripts.Models;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Player.Scripts.Views;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Scripts.Services.ControlledAgentService;
using RoyalCoreDomain.Scripts.Animation.Animator;
using RoyalCoreDomain.Scripts.CameraFollow;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Feature;
using RoyalCoreDomain.Scripts.Framework.Template.RoyalFeatureTemplate.Scripts.Feature;
using RoyalCoreDomain.Scripts.Framework.Template.RoyalFeatureTemplate.Scripts.Models;
using RoyalCoreDomain.Scripts.Framework.Template.RoyalFeatureTemplate.Scripts.Ports;
using RoyalCoreDomain.Scripts.Framework.Template.RoyalFeatureTemplate.Scripts.Views;
using RoyalCoreDomain.Scripts.Services.Providers.ModelProvider;
using RoyalCoreDomain.Scripts.Services.Providers.ViewProvider;
using RoyalCoreDomain.Scripts.Services.UpdateService;

namespace RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Player.Scripts.Feature
{
    public class PlayerFeature : BaseFeature
    {
        private readonly string _modelKey;
        private readonly string _viewKey;

        private PlayerController _controller;
        private AgentModel _model;
        private AgentView _view;

        public PlayerFeature(string address, IFeature parent, string viewKey = "Player/Views/AgentView",
            string modelKey = "Player/Models/PlayerModel")
            : base(address, parent)
        {
            _viewKey = viewKey;
            _modelKey = modelKey;
        }

        protected override void OnPreInstall()
        {
            PlanChild("Weapon", (addr, parent) => new WeaponFeature(addr, parent));
        }

        protected override void OnInstall()
        {
            var fixedUpdateService = Context.ImportService<IUpdateService<IFixedUpdatable>>();
            var models = Context.ImportService<IModelProvider>();
            var views = Context.ImportService<IViewProvider>();

            _view = views.LoadView<AgentView>(_viewKey);
            _model = models.LoadSO<AgentModel>(_modelKey);

            var animation = new AnimatorController(_view);
            var health = new HealthController(_model);
            var movement = new MovementController(_view, _model);
            var render = new RenderController(_view);

            Context.Models.Bind(_model);
            Context.Views.Bind(_view);

            _controller = new PlayerController(_model, _view, animation, health, movement, render, fixedUpdateService,
                Context.CommandFactory);
            Context.Controllers.Bind(_controller);

            // Context.Ports.Bind<IPlayerPort>(_controller);
            // Context.Export<IPlayerPort>(_controller);

            // TODO concrete type export fine for this example but if dynamic port derived, export dynamic all ports
            Context.Ports.Bind<IHittable>(_controller);
            Context.Export<IHittable>(_controller);

            Context.Ports.Bind<IMovable>(_controller);
            Context.Export<IMovable>(_controller);

            Context.Ports.Bind<IWeaponHolder>(_controller);
            Context.Export<IWeaponHolder>(_controller);
        }

        protected override void OnStart()
        {
            var controlledAgentService = Context.ImportService<IControlledAgentService>();
            var cameraFollowService = Context.ImportService<ICameraFollowService>();

            cameraFollowService.Follow(_view.transform);
            controlledAgentService.Set(_controller);

            // var factory = Context.ImportService<IFeatureFactory>();
            // var weapon = factory.Add(this, "weapon",
            //     (addr, parent) => new WeaponFeature(addr, parent));
        }


        protected override void OnDispose()
        {
            if (Context.TryImportService<IViewProvider>(out var vp)) vp.Release(_view);
            _view = null;

            _controller.Dispose();
        }
    }
}