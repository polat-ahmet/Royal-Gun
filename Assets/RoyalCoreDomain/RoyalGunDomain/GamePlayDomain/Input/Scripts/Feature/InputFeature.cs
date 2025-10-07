using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Input.Scripts.Controllers;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Input.Scripts.Models;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Input.Scripts.Ports;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Input.Scripts.Views;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Feature;
using RoyalCoreDomain.Scripts.Framework.Template.RoyalFeatureTemplate.Scripts.Models;
using RoyalCoreDomain.Scripts.Framework.Template.RoyalFeatureTemplate.Scripts.Views;
using RoyalCoreDomain.Scripts.Services.Providers.ModelProvider;
using RoyalCoreDomain.Scripts.Services.UI;
using RoyalCoreDomain.Scripts.Services.UpdateService;

namespace RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Input.Scripts.Feature
{
    public class InputFeature : BaseFeature
    {
        private readonly string _modelKey;
        private readonly string _viewKey;
        private InputController _ctrl;

        private InputModel _model;
        // private JoystickInputPortAdapter _port;

        private JoystickView _view;

        public InputFeature(string address, IFeature parent,
            string viewKey = "JoystickView",
            string modelKey = "Configs/Input/InputModel")
            : base(address, parent)
        {
            _viewKey = viewKey;
            _modelKey = modelKey;
        }

        protected override void OnInstall()
        {
            var mp = Context.ImportService<IModelProvider>();

            var ui = Context.ImportService<IUIService>();

            // var vp = Context.ImportService<IViewProvider>();
            // _view = vp.LoadView<JoystickView>(_viewKey);
            _view = ui.Show<JoystickView>(_viewKey);
            Context.Views.Bind(_view);

            var so = mp.LoadSO<InputCongfigSO>(_modelKey);
            _model = new InputModel(so);
            Context.Models.Bind(_model);

            _ctrl = new InputController(_view, _model, Context.CommandFactory);
            Context.Controllers.Bind(_ctrl);

            // _port = new JoystickInputPortAdapter(_model);
            Context.Ports.Bind<IJoystickInputPort>(_ctrl);
            Context.Export(Context.Ports.Require<IJoystickInputPort>());
        }

        protected override void OnStart()
        {
            Context.ImportService<IUpdateService<IUpdatable>>().RegisterUpdatable(_ctrl);
        }

        protected override void OnDispose()
        {
            Context.ImportService<IUpdateService<IUpdatable>>().UnregisterUpdatable(_ctrl);
            
            if (Context.TryImportService<IUIService>(out var uiService) && _view) uiService.Close(_view);
            _view = null;
        }
    }
}