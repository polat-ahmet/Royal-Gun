namespace RoyalCoreDomain.Scripts.Animation.Animator
{
    public interface IAnimatable
    {
        void PlayBool(string param, bool value);
        void PlayTrigger(string param);
    }
}