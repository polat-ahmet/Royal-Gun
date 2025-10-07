namespace RoyalCoreDomain.Scripts.Framework.RoyalFeature.Command
{
    public interface ICommandFactory
    {
        TCommand CreateAndResolveCommand<TCommand>() where TCommand : BaseCommand, new();
    }
}