namespace RoyalCoreDomain.Scripts.Framework.RoyalFeature.Feature
{
    public static class FeatureAddress
    {
        public static string ChildOf(string parent, string childName, string instanceId = null)
        {
            var basePath = string.IsNullOrWhiteSpace(parent) ? childName : $"{parent}/{childName}";
            return string.IsNullOrWhiteSpace(instanceId) ? basePath : $"{basePath}:{instanceId}";
        }
    }
}