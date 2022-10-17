using TheGame.Events;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;

namespace TheGame
{
    public partial class GameplayController : IGameplayController
    {
        private Dictionary<IIdentifiers, IndividualSubcontroller> players = new Dictionary<IIdentifiers, IndividualSubcontroller>();
        private PlayPhase currentPlayPhase;
        private IEventBus eventBus;
        private IFieldFactory fieldFactory;
        private IPlayerFactory playerFactory;
        private IndividualSubcontroller.Factory subcontrolFactory;
        private GameSettings settings;
        private Dictionary<int, Team> teams = new Dictionary<int, Team>();
        private Field[] fields;

        private int round = 1;


        public int Round { get => round; }

        public Action<int> OnTeamCompletePhase;
        public Action OnAllCompletePhase;

        [Inject]
        private void Inject(IEventBus eventBus
                          , IndividualSubcontroller.Factory subcontrolFactory
                          , IFieldFactory fieldFactory
                          , IPlayerFactory playerFactory)
        {
            this.eventBus = eventBus;
            this.subcontrolFactory = subcontrolFactory;
            this.fieldFactory = fieldFactory;
            this.playerFactory = playerFactory;
            OnEnable();
        }

        public async void Initialize(GameSettings settings, Action onComplete)
        {
            this.settings = settings;
            int teams = settings.TotalTeams;
            fields = new Field[teams];
            var position = Vector2.zero;

            for (int i = 0, j = teams; i < j; i++)
            {
                var fieldWaiter = fieldFactory.Create(settings.Scenario, i, position);
                await fieldWaiter;
                fields[i] = fieldWaiter.Result;

                var nextPosition = position;
                nextPosition.x += 100;
                position = nextPosition;
            }
        }

        private void OnEnable()
        {
            eventBus.GetEvent<GameplayPhaseCompleteEvent>().Subscribe(CompletePhase);
        }

        public void StartGame()
        {
            foreach (var player in players)
            {
                player.Value.StartGame();
            }
        }

        public void AddPlayer(Player player)
        {
            var key = player.Identifiers;
            if (players.ContainsKey(key))
            {
                Debug.LogWarning("Player " + key.ID.ToString() + " is already added!");
                return;
            }

            var newPlayer = subcontrolFactory.Create(player, this);
            players.Add(key, newPlayer);

            if (!teams.ContainsKey(key.TeamID))
            {
                var newTeam = new Team(key.TeamID);
                teams.Add(key.TeamID, newTeam);
            }
            else
            {
                teams[key.TeamID].AddPlayer(newPlayer);
            }
        }

        private void CompletePhase(GameplayPhaseCompleteArgs args)
        {
            var key = args.Identifiers;
            if (players.ContainsKey(key))
            {
                players[key].SetCurrentPhaseCompleted();
            }

            TryCompletePhaseForAll();
        }

        private void TryCompletePhaseForAll()
        {
            int awaitingTeams = teams.Count;
            for (int i = 0, j = teams.Count; i < j; i++)
            {
                if (teams[i].IsAllTeamMembersCompletePhase())
                {
                    OnTeamCompletePhase?.Invoke(i);
                    awaitingTeams--;
                }
            }
            if (awaitingTeams == 0)
            {
                OnAllCompletePhase?.Invoke();
            }
        }
    }

    public interface IIndividualController
    {
        bool IsPhaseComplete { get; }
        IGameplayStatesAccessor States { get; }
        void ChangeState(GameplayState state);
        void CompleteStateImmediately();
    }

    public interface IGameplayStatesAccessor
    {
        EnterGameplayState EnterState { get; }
        PreparingGameplayState PreparingState { get; }
        TargetingGameplayState TargetingState { get; }
        ActiveGameplayState ActiveState { get; }
        RewardingGameplayerState RewardingState { get; }
        BoostingGameplayState BoostingState { get; }
        WaitingGameplayState WaitingState { get; }
    }

    public interface IGameplayController
    {
        public void Initialize(GameSettings settings, Action OnComplete);
        public void AddPlayer(Player player);
        public void StartGame();
    }


    public class Team
    {
        private List<IIndividualController> players = new List<IIndividualController>();
        private int teamID;
        public IReadOnlyList<IIndividualController> Players => Players;
        public int TeamID => teamID;

        public Team(int id)
        {
            teamID = id;
        }

        public void AddPlayer(IIndividualController player)
        {
            if (!players.Contains(player))
            {
                players.Add(player);
            }
        }

        public void RemovePlayer(IIndividualController player)
        {
            if (players.Contains(player))
            {
                players.Remove(player);
            }
        }

        public bool IsAllTeamMembersCompletePhase()
        {
            for (int i = 0, j = players.Count; i < j; i++)
            {
                if (!players[i].IsPhaseComplete)
                {
                    return false;
                }
            }
            return true;
        }
    }
}

