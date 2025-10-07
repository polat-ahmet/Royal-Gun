using RoyalCoreDomain.Scripts.Framework.RoyalFeature.Context;

namespace RoyalCoreDomain.Scripts.Framework.RoyalFeature.Command
{
    public class CommandFactory : ICommandFactory
    {
        private readonly FeatureContext _context;

        public CommandFactory(FeatureContext context)
        {
            _context = context;
        }

        public TCommand CreateAndResolveCommand<TCommand>() where TCommand : BaseCommand, new()
        {
            var command = new TCommand();
            command.Resolve(_context);
            return command;
        }
        
        public TCommand CreateCommand<TCommand>() where TCommand : ICommand, new()
        {
            var command = new TCommand();
            return command;
        }
    }
}