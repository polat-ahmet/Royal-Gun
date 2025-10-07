using RoyalCoreDomain.Scripts.Framework.RoyalFeature.MVC.Controller;
using UnityEngine;

namespace RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Scripts.Modules.SpriteRender
{
    public class RenderController : BaseController, IRenderController
    {
        private readonly ISpriteRendererView _view;

        public RenderController(ISpriteRendererView view)
        {
            _view = view;
        }

        public void FaceDirection(Vector2 moveVector)
        {
            var x = moveVector.x;

            if (x > 0)
                _view.Renderer.flipX = false;
            else if (x < 0) _view.Renderer.flipX = true;
        }
    }
}