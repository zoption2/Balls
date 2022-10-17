using System.Collections;
using System.Collections.Generic;

namespace TheGame
{
    public class InteractionArgs
    {
        private List<IInteraction> data = new List<IInteraction>();
        public IReadOnlyList<IInteraction> Data => data;

        public void Add(IInteraction interaction)
        {
            data.Add(interaction);
        }
    }

    public interface IInteraction
    {
        IIdentifiers Identifiers { get; }
        void Execute(IEnemyStatsGetter statsGetter, IInteractorOperation interactorOperation);
    }
}


