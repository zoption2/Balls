using TheGame.Events;
using System;
using Zenject;

namespace TheGame
{
    public partial class GameplayController
    {
        [Serializable]
        public class IndividualSubcontroller : IIndividualController
        {
            private GameplayController controller;
            private StateMachine<GameplayState> stateMachine;
            private StatesHolder states;
            [Inject] private IEventBus eventBus;

            public IGameplayStatesAccessor States => states;
            public Player Player { get; }
            public bool IsPhaseComplete { get; private set; }
            private PlayPhase LocalPhase => stateMachine.State.playPhase;

            public Action OnCurrentPhaseComplete;


            public IndividualSubcontroller(Player player, GameplayController controller)
            {
                Player = player;
                this.controller = controller;

                states = new StatesHolder();
                stateMachine = new StateMachine<GameplayState>();
                states.EnterState = new EnterGameplayState(this, eventBus);
                states.PreparingState = new PreparingGameplayState(this, eventBus);
                states.TargetingState = new TargetingGameplayState(this, eventBus);
                states.ActiveState = new ActiveGameplayState(this, eventBus);
                states.RewardingState = new RewardingGameplayerState(this, eventBus);
                states.BoostingState = new BoostingGameplayState(this, eventBus);
                states.WaitingState = new WaitingGameplayState(this, eventBus);
            }

            public void StartGame()
            {
                stateMachine.Init(states.EnterState);
            }

            public void ChangeState(GameplayState state)
            {
                IsPhaseComplete = false;
                Player.ChangeState(state.playPhase);
                stateMachine.ChangeState(state);
            }

            public void CompleteStateImmediately()
            {
                var args = new GameplayPhaseCompleteArgs(Player.Identifiers, stateMachine.State.playPhase);
                controller.CompletePhase(args);
            }

            public void SetCurrentPhaseCompleted()
            {
                IsPhaseComplete = true;
                if (stateMachine.State.NeedWaitAllCompletedPhase)
                {
                    controller.OnAllCompletePhase += CompletePhaseForAll;
                }
                else if (stateMachine.State.NeedWaitTeamCompletedPhase)
                {
                    controller.OnTeamCompletePhase += CompleteTeamPhase;
                }
                else
                {
                    ApproveCompletedPhase();
                }
            }

            private void CompleteTeamPhase(int team)
            {
                controller.OnTeamCompletePhase -= CompleteTeamPhase;
                ApproveCompletedPhase();
            }

            private void CompletePhaseForAll()
            {
                controller.OnAllCompletePhase -= CompletePhaseForAll;
                ApproveCompletedPhase();
            }

            private void ApproveCompletedPhase()
            {
                stateMachine.State.CompletePhase();
            }

            private class StatesHolder : IGameplayStatesAccessor
            {
                public EnterGameplayState EnterState { get; set; }
                public PreparingGameplayState PreparingState { get; set; }
                public TargetingGameplayState TargetingState { get; set; }
                public ActiveGameplayState ActiveState { get; set; }
                public RewardingGameplayerState RewardingState { get; set; }
                public BoostingGameplayState BoostingState { get; set; }
                public WaitingGameplayState WaitingState { get; set; }
            }

            public class Factory : PlaceholderFactory<Player, GameplayController, IndividualSubcontroller>
            {

            }
        }
    }
}

