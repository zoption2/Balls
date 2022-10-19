using TheGame.Events;
using System.Threading.Tasks;

namespace TheGame
{
    public interface IGameplayStateMachine : IStateMachine<RoundStates>
    {
        IPersonalController GetController();
        IEventBus GetEventBus();
        void CompleteStateImmediately();
    }

    public class GameplayStateMachine : BaseStateMachine<RoundStates>, IGameplayStateMachine
    {
        private IPersonalController _individualController;
        private IEventBus _eventBus;
        public GameplayStateMachine(IPersonalController individualController, IEventBus eventBus)
        {
            _individualController = individualController;
            _eventBus = eventBus;
        }

        public void CompleteStateImmediately()
        {
            _individualController.CompleteStateImmediately();
        }

        public IPersonalController GetController()
        {
            return _individualController;
        }

        public IEventBus GetEventBus()
        {
            return _eventBus;
        }
    }

    public abstract class GameplayState : BaseState<RoundStates>
    {
        protected IGameplayStateMachine _machine;
        protected IPersonalController _personalController;
        protected IEventBus _eventBus;

        public GameplayState(IGameplayStateMachine machine)
        {
            _machine = machine;
            _personalController = _machine.GetController();
            _eventBus = _machine.GetEventBus();
        }

        public override void Enter()
        {
            base.Enter();
            UnityEngine.Debug.Log("Gameplay state " + this);
        }

        public abstract void CompletePhase();
    }

    public class PrepareRoundState : GameplayState
    {
        public PrepareRoundState(IGameplayStateMachine machine) : base(machine)
        {
        }

        public override RoundStates State => RoundStates.Prepare;

        public override void CompletePhase()
        {
            throw new System.NotImplementedException();
        }
    }

    public class ActionRoundState : GameplayState
    {
        public ActionRoundState(IGameplayStateMachine machine) : base(machine)
        {
        }

        public override RoundStates State => RoundStates.Action;

        public override void CompletePhase()
        {
            throw new System.NotImplementedException();
        }
    }

    public class RewardingRoundState : GameplayState
    {
        public RewardingRoundState(IGameplayStateMachine machine) : base(machine)
        {
        }

        public override RoundStates State => RoundStates.Rewarding;

        public override void CompletePhase()
        {
            throw new System.NotImplementedException();
        }
    }

    public class CompleteRoundState : GameplayState
    {
        public CompleteRoundState(IGameplayStateMachine machine) : base(machine)
        {
        }

        public override RoundStates State => RoundStates.Complete;

        public override void CompletePhase()
        {
            throw new System.NotImplementedException();
        }
    }
}

