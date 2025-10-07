using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Weapon.Scripts.Controllers;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Weapon.Scripts.Models;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Weapon.Scripts.Ports;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Weapon.Scripts.Views;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Weapon.Scripts.WeaponHolder;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Feature;
using RoyalCoreDomain.Scripts.Framework.Template.RoyalFeatureTemplate.Scripts.Models;
using RoyalCoreDomain.Scripts.Framework.Template.RoyalFeatureTemplate.Scripts.Ports;
using RoyalCoreDomain.Scripts.Framework.Template.RoyalFeatureTemplate.Scripts.Views;
using RoyalCoreDomain.Scripts.Services.Providers.ModelProvider;
using RoyalCoreDomain.Scripts.Services.Providers.ViewProvider;
using RoyalCoreDomain.Scripts.Services.UpdateService;

namespace RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Weapon.Scripts.Feature
{
    public class WeaponFeature : BaseFeature
    {
        private readonly string _modelKey;
        private readonly string _viewKey;
        private WeaponController _ctrl;

        private WeaponModel _model;
        private WeaponView _view;

        public WeaponFeature(string address, IFeature parent,
            string viewKey = "Weapons/Views/RifleView",
            string modelKey = "Weapons/Models/RifleModel")
            : base(address, parent)
        {
            _viewKey = viewKey;
            _modelKey = modelKey;
        }

        protected override void OnInstall()
        {
            var vp = Context.ImportService<IViewProvider>();
            var mp = Context.ImportService<IModelProvider>();

            _view = vp.LoadView<WeaponView>(_viewKey);
            Context.Views.Bind(_view);

            var so = mp.LoadSO<WeaponModelSO>(_modelKey);
            _model = new WeaponModel();
            _model.CopyFrom(so);
            Context.Models.Bind(_model);

            _ctrl = new WeaponController(Context, _model, _view);
            Context.Controllers.Bind(_ctrl);
            
            if (Context.TryImportPort<IWeaponHolder>(out var holder) && holder?.WeaponMount)
            {
                _view.transform.SetParent(holder.WeaponMount, false);
                _ctrl.AttachHolder(holder);
            }

            // Port export
            Context.Ports.Bind<IWeaponPort>(_ctrl);
            Context.Export<IWeaponPort>(_ctrl);

            // Tick
            Context.ImportService<IUpdateService<IUpdatable>>().RegisterUpdatable(_ctrl);
        }

        protected override void OnDispose()
        {
            if (Context.TryImportService<IViewProvider>(out var vp) && _view) vp.Release(_view);
        }
    }
}