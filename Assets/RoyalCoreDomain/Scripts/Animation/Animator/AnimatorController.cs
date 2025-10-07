using RoyalCoreDomain.Scripts.Framework.RoyalFeature.MVC.Controller;

namespace RoyalCoreDomain.Scripts.Animation.Animator
{
    public class AnimatorController : BaseController, IAnimatable
    {
        private readonly IAnimatorView _view;

        public AnimatorController(IAnimatorView view)
        {
            _view = view;
        }

        public void PlayBool(string param, bool value)
        {
            if (_view.Animator) _view.Animator.SetBool(param, value);
        }

        public void PlayTrigger(string param)
        {
            if (_view.Animator) _view.Animator.SetTrigger(param);
        }
    }
}