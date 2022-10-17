using System.Collections.Generic;
using UnityEngine;

namespace TheGame
{
    public class Interactor : MonoBehaviour, IInteractable, IInteractorOperation
    {
        private List<IInteraction> interactions = new List<IInteraction>();
        private IEnemyStatsGetter stats;

        public void Initialize(IEnemyStatsGetter stats)
        {
            this.stats = stats;
        }

        public void Interact(InteractionArgs args)
        {
            for (int i = 0, j = args.Data.Count; i < j; i++)
            {
                args.Data[i].Execute(stats, this);
            }
        }

        public void AddInteraction(IInteraction interaction)
        {
            interactions.Add(interaction);
        }

        public void RemoveInteraction(IInteraction interaction)
        {
            if (interactions.Contains(interaction))
            {
                interactions.Remove(interaction);
            }
        }
    }

    public interface IInteractable
    {
        void Interact(InteractionArgs args);
    }

    public interface IInteractorOperation
    {
        void AddInteraction(IInteraction interaction);
        void RemoveInteraction(IInteraction interaction);
    }
}


