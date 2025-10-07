using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Scripts.Modules.Health;
using RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Agent.Scripts.Modules.Movement;
using RoyalCoreDomain.Scripts.Framework.RoyalFeature.MVC.Model;
using UnityEngine;

namespace RoyalCoreDomain.RoyalGunDomain.GamePlayDomain.Player.Scripts.Models
{
    [CreateAssetMenu(fileName = "AgentModel", menuName = "Royal Gun/Agent Model", order = 0)]
    public class AgentModel : ScriptableObject, IModel, IMovementData, IHealthData
    {
        [field: SerializeField] public float MaxHealth { get; set; }
        [field: SerializeField] public float CurrentHealth { get; set; }
        [field: SerializeField] public float Speed { get; set; }
    }
}