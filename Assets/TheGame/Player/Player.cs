using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using Zenject;
using TheGame.Events;


namespace TheGame
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerStats _stats;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private Transform _ballPosition;
        [field: SerializeField] public TargetingRay Ray { get; private set; }
        [field: SerializeField] public Rigidbody2D Rb { get; private set; }
        public ControlInputs Inputs { get; private set; }
        private Player player;
        private IIdentifiers identifiers;
        private IBallsFactory ballsFactory;
        private IPoolController pool;
        private IEventBus eventBus;
        private IPoolableBalls ball;
        [SerializeField] private InternalPlayer internalPlayer;
        private StateMachine<State> stateMachine;
        public PlayerTargetingState targetingState;
        public PlayerPlayingState playingState;
        public PlayerWaitingState waitingState;

        public IIdentifiers Identifiers { get; private set; }


        [Inject]
        public void Constructor(IBallsFactory ballsFactory, IPoolController pool, IEventBus eventBus)
        {
            internalPlayer.Inject(ballsFactory, pool, eventBus);
        }

        public void Initialize(IIdentifiers identifiers)
        {
            Identifiers = identifiers;
            internalPlayer.Initialize(this);
            stateMachine = new StateMachine<State>();
            targetingState = new PlayerTargetingState(internalPlayer);
            playingState = new PlayerPlayingState(internalPlayer);
            waitingState = new PlayerWaitingState(internalPlayer);
            stateMachine.Init(waitingState);
        }

        public void ChangeState(PlayPhase playPhase)
        {
            PlayerState state;
            switch (playPhase)
            {
                case PlayPhase.targeting:
                    state = targetingState;
                    break;
                case PlayPhase.active:
                    state = playingState;
                    break;
                default:
                    state = waitingState;
                    break;
            }
            stateMachine.ChangeState(state);
        }


        [System.Serializable]
        public class InternalPlayer
        {
            [field: SerializeField] public PlayerStats Stats { get; private set; }
            [field: SerializeField] public PlayerInput PlayerInput { get; private set; }
            [field: SerializeField] public Transform BallPosition { get; private set; }
            [field: SerializeField] public TargetingRay Ray { get; private set; }
            [field: SerializeField] public Rigidbody2D Rb { get; private set; }
            public ControlInputs Inputs { get; private set; }
            private Player player;
            private IIdentifiers identifiers;
            private IBallsFactory ballsFactory;
            private IPoolController pool;
            private IEventBus eventBus;
            private IPoolableBalls ball;

            public async void Initialize(Player player)
            {
                Inputs = new ControlInputs(PlayerInput);
                this.player = player;
                identifiers = player.Identifiers;
                Stats.TargetingSpeed.SetToBase();
                Stats.MoveSpeed.SetToBase();
                var waiter = ballsFactory.GetPrefab(BallType.simple);
                await waiter;
                var ballPrefab = waiter.Result;
                pool.InitPool(BallType.simple, ballPrefab, null);
                ball = (IPoolableBalls)pool.GetObject(BallType.simple, BallPosition.position, BallPosition.rotation, BallPosition);
                ball.transform.position = BallPosition.position;
            }

            public void Inject(IBallsFactory ballsFactory, IPoolController pool, IEventBus eventBus)
            {
                this.ballsFactory = ballsFactory;
                this.pool = pool;
                this.eventBus = eventBus;
            }

            public void Targeting(Vector2 direction, Phase phase)
            {
                var dir = BallPosition.rotation * Quaternion.FromToRotation(BallPosition.up, direction);
                var rotation = BallPosition.rotation;
                rotation = Quaternion.RotateTowards(BallPosition.rotation, dir, Stats.TargetingSpeed.Value);
                rotation.z = Mathf.Clamp(rotation.z, -0.6f, 0.6f);

                BallPosition.rotation = rotation;
                Ray.Targeting();
            }

            public void MoveLeft(Phase phase)
            {
                if (phase.Equals(Phase.complete))
                {
                    Rb.velocity = Vector2.zero;
                    return;
                }
                //Rb.velocity = (-Vector2.right * Stats.MoveSpeed);
                Rb.transform.Translate(-Vector2.right * Stats.MoveSpeed.Value);
                Ray.Targeting();
            }

            public void MoveRight(Phase phase)
            {
                if (phase.Equals(Phase.complete))
                {
                    Rb.velocity = Vector2.zero;
                    return;
                }
                //Rb.velocity = Vector2.right * Stats.MoveSpeed;
                Rb.transform.Translate(Vector2.right * Stats.MoveSpeed.Value);
                Ray.Targeting();
            }

            public void Shot(Phase phase)
            {
                ball.transform.parent = null;
                ball.Launch(identifiers, BallPosition.up);
                var args = new GameplayPhaseCompleteArgs(identifiers, PlayPhase.targeting);
                eventBus.GetEvent<GameplayPhaseCompleteEvent>().InvokeForGlobal(args);
            }

            public void DebugMassage(string massage)
            {
                Debug.Log(massage);
            }
        }
    }

    public abstract class PlayerState : State
    {
        protected Player.InternalPlayer player;
        public PlayerState(Player.InternalPlayer player)
        {
            this.player = player;
        }

        public override void Enter()
        {
            base.Enter();
            player.DebugMassage(debugMassage);
        }
    }

    public class PlayerTargetingState : PlayerState
    {
        public PlayerTargetingState(Player.InternalPlayer player) : base(player)
        {
        }

        public override void Enter()
        {
            base.Enter();
            player.Inputs.OnTargeting += player.Targeting;
            player.Inputs.OnMoveLeft += player.MoveLeft;
            player.Inputs.OnMoveRight += player.MoveRight;
            player.Inputs.OnShot += player.Shot;
            player.Ray.EnableRay();
        }

        public override void Exit()
        {
            base.Exit();
            player.Inputs.OnTargeting -= player.Targeting;
            player.Inputs.OnMoveLeft -= player.MoveLeft;
            player.Inputs.OnMoveRight -= player.MoveRight;
            player.Inputs.OnShot -= player.Shot;
            player.Ray.DisableRay();
        }
    }

    public class PlayerPlayingState : PlayerState
    {
        public PlayerPlayingState(Player.InternalPlayer player) : base(player)
        {
        }

        public override void Enter()
        {
            base.Enter();
            player.Inputs.OnMoveLeft += player.MoveLeft;
            player.Inputs.OnMoveRight += player.MoveRight;
        }

        public override void Exit()
        {
            base.Exit();
            player.Inputs.OnMoveLeft -= player.MoveLeft;
            player.Inputs.OnMoveRight -= player.MoveRight;
        }
    }

    public class PlayerWaitingState : PlayerState
    {
        public PlayerWaitingState(Player.InternalPlayer player) : base(player)
        {
        }
    }
}

