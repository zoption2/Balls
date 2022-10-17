using UnityEngine;

namespace TheGame
{
    public class SimpleBall : BallBase
    {
        [SerializeField] private float ballDamage;
        public override BallType BallType => BallType.simple;
        private InteractionArgs interactionArgs;
        protected override InteractionArgs InteractionArgs => interactionArgs;
 

        public override void OnCreate()
        {
            base.OnCreate();
            BuildArgs();
        }

        public override void OnRestore()
        {
            base.OnRestore();
            BuildArgs();
        }

        private void BuildArgs()
        {
            var damage = new Damage(identifiers, ballDamage);
            var args = new InteractionArgs();
            args.Add(damage);
            interactionArgs = args;
        }
    }
}

