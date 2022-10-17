using TheGame.Events;
using System.Threading.Tasks;

namespace TheGame
{
    public abstract class GameplayState : State
    {
        public abstract PlayPhase playPhase { get; }
        public abstract bool NeedWaitTeamCompletedPhase { get; }
        public abstract bool NeedWaitAllCompletedPhase { get; }

        protected IIndividualController playerController;
        protected IEventBus eventBus;

        public GameplayState(IIndividualController playerController, IEventBus eventBus)
        {
            this.playerController = playerController;
            this.eventBus = eventBus;
        }

        public override void Enter()
        {
            base.Enter();
            UnityEngine.Debug.Log("Gameplay state " + this);
        }

        public abstract void CompletePhase();
    }

    public class EnterGameplayState : GameplayState
    {
        public override PlayPhase playPhase => PlayPhase.enter;
        public override bool NeedWaitTeamCompletedPhase => true;
        public override bool NeedWaitAllCompletedPhase => true;

        public EnterGameplayState(IIndividualController playerController, IEventBus eventBus) : base(playerController, eventBus)
        {
        }

        public override async void Enter()
        {
            base.Enter();
            //await Task.Delay(2000);
            playerController.CompleteStateImmediately();
        }

        public override void CompletePhase()
        {
            playerController.ChangeState(playerController.States.PreparingState);
        }
    }

    public class PreparingGameplayState : GameplayState
    {
        public PreparingGameplayState(IIndividualController playerController, IEventBus eventBus) : base(playerController, eventBus)
        {
        }

        public override PlayPhase playPhase => PlayPhase.preparing;
        public override bool NeedWaitTeamCompletedPhase => true;
        public override bool NeedWaitAllCompletedPhase => true;

        public override async void Enter()
        {
            base.Enter();
            //await Task.Delay(2000);
            playerController.CompleteStateImmediately();
        }

        public override void CompletePhase()
        {
            playerController.ChangeState(playerController.States.TargetingState);
        }
    }

    public class TargetingGameplayState : GameplayState
    {
        public TargetingGameplayState(IIndividualController playerController, IEventBus eventBus) : base(playerController, eventBus)
        {
        }

        public override PlayPhase playPhase => PlayPhase.targeting;
        public override bool NeedWaitTeamCompletedPhase => false;
        public override bool NeedWaitAllCompletedPhase => false;

        public override void CompletePhase()
        {
            playerController.ChangeState(playerController.States.ActiveState);
        }
    }

    public class ActiveGameplayState : GameplayState
    {
        public ActiveGameplayState(IIndividualController playerController, IEventBus eventBus) : base(playerController, eventBus)
        {
        }

        public override PlayPhase playPhase => PlayPhase.active;
        public override bool NeedWaitTeamCompletedPhase => true;
        public override bool NeedWaitAllCompletedPhase => false;

        public override void CompletePhase()
        {
            playerController.ChangeState(playerController.States.RewardingState);
        }
    }

    public class RewardingGameplayerState : GameplayState
    {
        public RewardingGameplayerState(IIndividualController playerController, IEventBus eventBus) : base(playerController, eventBus)
        {
        }

        public override PlayPhase playPhase => PlayPhase.rewarding;
        public override bool NeedWaitTeamCompletedPhase => true;
        public override bool NeedWaitAllCompletedPhase => false;

        public override void CompletePhase()
        {
            playerController.ChangeState(playerController.States.BoostingState);
        }
    }

    public class BoostingGameplayState : GameplayState
    {
        public BoostingGameplayState(IIndividualController playerController, IEventBus eventBus) : base(playerController, eventBus)
        {
        }

        public override PlayPhase playPhase => PlayPhase.boosting;

        public override bool NeedWaitTeamCompletedPhase => false;
        public override bool NeedWaitAllCompletedPhase => false;
        public override void CompletePhase()
        {
            playerController.ChangeState(playerController.States.WaitingState);
        }
    }

    public class WaitingGameplayState : GameplayState
    {
        public WaitingGameplayState(IIndividualController playerController, IEventBus eventBus) : base(playerController, eventBus)
        {
        }

        public override PlayPhase playPhase => PlayPhase.waiting;
        public override bool NeedWaitTeamCompletedPhase => false;
        public override bool NeedWaitAllCompletedPhase => true;

        public override void CompletePhase()
        {
            playerController.ChangeState(playerController.States.PreparingState);
        }
    }
}

