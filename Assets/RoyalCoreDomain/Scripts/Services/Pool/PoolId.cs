namespace RoyalCoreDomain.Scripts.Services.Pool
{
    public readonly struct PoolId
    {
        public readonly string Category; // "feature", "view", "bullet", ...
        public readonly string Key;      // "EnemyFeature", "Player/Views/Bullet"
        public PoolId(string category, string key) { Category=category; Key=key; }
        public override string ToString() => $"{Category}:{Key}";
    }
}