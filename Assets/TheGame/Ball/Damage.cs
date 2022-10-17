using UnityEngine;

namespace TheGame
{
    public class Damage : IInteraction
    {
        private float damage;
        public IIdentifiers Identifiers { get; }

        public Damage(IIdentifiers identifiers, float damage)
        {
            Identifiers = identifiers;
            this.damage = damage;
        }

        public void Execute(IEnemyStatsGetter statsGetter, IInteractorOperation interactorOperation)
        {
            statsGetter.ChangeHealthRequest(-damage);
        }
    }
}


