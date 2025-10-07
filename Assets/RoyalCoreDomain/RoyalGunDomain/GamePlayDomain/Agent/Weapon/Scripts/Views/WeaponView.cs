using RoyalCoreDomain.Scripts.Framework.RoyalFeature.MVC.View;
using UnityEngine;

namespace RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Weapon.Scripts.Views
{
    public class WeaponView : MonoBehaviour, IView
    {
        [SerializeField] private Transform muzzle;
        [SerializeField] private Transform root;
        [SerializeField] private SpriteRenderer sr;

        public Transform Muzzle => muzzle ? muzzle : transform;
        public Transform Root => root ? root : transform;
        public SpriteRenderer SR => sr;

        private void Awake()
        {
            if (!sr) sr = GetComponent<SpriteRenderer>();
            if (!root) root = transform;
        }

        public void LookAt(Vector2 dir, bool flipXByDirX = true)
        {
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            FlipSprite(angle > 90 || angle < -90);

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        public void FlipSprite(bool val)
        {
            var flipModifier = val ? -1 : 1;
            transform.localScale = new Vector3(transform.localScale.x, flipModifier * Mathf.Abs(transform.localScale.y),
                transform.localScale.z);
        }
    }
}