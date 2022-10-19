using TheGame.Events;
using System;

namespace TheGame
{
    public class IndividualPlayController : IPersonalController
    {
        private GameplayController controller;
        private IGameplayStateMachine _stateMachine;
        private IEventBus _eventBus;
        private PlayerView _player;

        public PlayerView Player => _player;
        public bool IsPhaseComplete { get; private set; }
        private RoundStates LocalPhase => _stateMachine.State;

        public Action OnCurrentPhaseComplete;


        public IndividualPlayController(GameplayController controller)
        {
            this.controller = controller;

            _stateMachine = new GameplayStateMachine(this, _eventBus);
            var prepareState = new PrepareRoundState(_stateMachine);
            var actionState = new ActionRoundState(_stateMachine);
            var rewardingState = new RewardingRoundState(_stateMachine);
            var completeState = new CompleteRoundState(_stateMachine);

            _stateMachine.InitStates(prepareState, actionState, rewardingState, completeState);
        }

        public void Initialize(PlayerView player)
        {
            _player = player;
        }

        public void StartGame()
        {
            _stateMachine.ChangeState(RoundStates.Prepare);
        }

        public void ChangeState(GameplayState state)
        {
            IsPhaseComplete = false;
            Player.ChangeState(state.PlayPhase);
            _stateMachine.ChangeState(state);
        }

        public void CompleteStateImmediately()
        {
            var args = new GameplayPhaseCompleteArgs(Player.Identifiers, _stateMachine.State.PlayPhase);
            controller.CompletePhase(args);
        }
    }
    
}

