using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using TheGame.MVP;
using TheGame.Events;


namespace TheGame
{
    public interface IPlayerModel : IModel
    {
        public PlayerStats Stats { get; }
        public FieldHelper FieldHelper { get; }
    }

    public class PlayerModel : IPlayerModel
    {
        private PlayerStats _stats;


        public PlayerStats Stats => _stats;
        public FieldHelper FieldHelper { get; }
        public PlayerModel(PlayerStats stats)
        {
            _stats = stats;
            _stats.MoveSpeed.SetToBase();
            _stats.TargetingSpeed.SetToBase();
        }
    }


    public interface IPlayerPresenter : IPresenter<IPlayerModel, IPlayerView>
    {
        ControlInputs Inputs { get; }
        void ShowTargetingRay(bool isShow);
        void InvokeTargeting(Vector2 direction, Phase phase);
        void InvokeMoveLeft(Phase phase);
        void InvokeMoveRight(Phase phase);
        void InvokeShot(Phase phase);
    }

    public class PlayerPresenter : BasePresenter, IPlayerPresenter
    {
        public IPlayerModel Model { get; private set; }
        public IPlayerView View { get; private set; }

        private IPlayerStateMachine _stateMachine;
        private IPlayerFactory _factory;
        private IBallsFactory _ballsFactory;
        private IPoolController _pool;
        private IEventBus _eventBus;
        private ControlInputs _inputs;

        public ControlInputs Inputs => _inputs;

        public PlayerPresenter(IPlayerModel model
            , IPlayerFactory playerFactory
            , IBallsFactory ballsFactory
            , IPoolController pool)
        {
            Model = model;
            _factory = playerFactory;
            _ballsFactory = ballsFactory;
            _pool = pool;
        }

        public override void Hide()
        {
            throw new System.NotImplementedException();
        }

        public override void Init()
        {
            View = _factory.GetPlayer(Model.FieldHelper.StartPositions[0].position);
            View.InitPresenter(this);
            _inputs = new ControlInputs(View.PlayerInput);
            InitPlayerStates();

        }

        public override void Show()
        {
            throw new System.NotImplementedException();
        }

        public void ShowTargetingRay(bool isShow)
        {
            View.ShowRay(isShow);
        }

        private void InitPlayerStates()
        {
            _stateMachine = new PlayerStateMachine(this);
            var aimingState = new PlayerAimingState(_stateMachine);
            var moveOnlyState = new PlayerMoveOnlyState(_stateMachine);
            var inactiveState = new PlayerInactiveState(_stateMachine);
            _stateMachine.InitStates(aimingState, moveOnlyState, inactiveState);
        }

        public void InvokeTargeting(Vector2 direction, Phase phase)
        {
            var speed = Model.Stats.TargetingSpeed.Value;
            View.RotateAim(direction, phase, speed);
        }

        public void InvokeMoveLeft(Phase phase)
        {
            var speed = Model.Stats.MoveSpeed.Value;
            View.MoveLeft(phase, speed);
        }

        public void InvokeMoveRight(Phase phase)
        {
            var speed = Model.Stats.MoveSpeed.Value;
            View.MoveRight(phase, speed);
        }

        public void InvokeShot(Phase phase)
        {
            
        }
    }

    public interface IPlayerView : IView<IPlayerPresenter>
    {
        PlayerInput PlayerInput { get; }
        Vector2 GetBallPosition();
        void ShowRay(bool isShow);
        void MoveLeft(Phase phase, float speed);
        void MoveRight(Phase phase, float speed);
        void RotateAim(Vector2 direction, Phase phase, float rotatingSpeed);
    }

    public class PlayerView : MonoBehaviour, IPlayerView
    {
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private Transform _ballPosition;
        [SerializeField] private TargetingRay _ray;
        [SerializeField] private Rigidbody2D _rb;

        public IPlayerPresenter Presenter { get; private set; }

        public PlayerInput PlayerInput => throw new System.NotImplementedException();

        public void RotateAim(Vector2 direction, Phase phase, float rotatingSpeed)
        {
            var dir = _ballPosition.rotation * Quaternion.FromToRotation(_ballPosition.up, direction);
            var rotation = _ballPosition.rotation;
            rotation = Quaternion.RotateTowards(_ballPosition.rotation, dir, rotatingSpeed);
            rotation.z = Mathf.Clamp(rotation.z, -0.6f, 0.6f);

            _ballPosition.rotation = rotation;
            _ray.Targeting();
        }

        public void MoveLeft(Phase phase, float speed)
        {
            if (phase.Equals(Phase.complete))
            {
                _rb.velocity = Vector2.zero;
                return;
            }

            _rb.transform.Translate(-Vector2.right * speed);
            _ray.Targeting();
        }

        public void MoveRight(Phase phase, float speed)
        {
            if (phase.Equals(Phase.complete))
            {
                _rb.velocity = Vector2.zero;
                return;
            }

            _rb.transform.Translate(Vector2.right * speed);
            _ray.Targeting();
        }

        public void DebugMassage(string massage)
        {
            Debug.Log(massage);
        }

        public void InitPresenter(IPlayerPresenter presenter)
        {
            Presenter = presenter;
        }

        public void Show()
        {
            throw new System.NotImplementedException();
        }

        public void Hide()
        {
            throw new System.NotImplementedException();
        }

        public void Init()
        {
            throw new System.NotImplementedException();
        }

        public Vector2 GetBallPosition()
        {
            return _ballPosition.position;
        }

        public void ShowRay(bool isShow)
        {
            _ray.EnableRay();
        }
    }

    public enum PlayerStates
    {
        Aiming,
        MoveOnly,
        Inactive
    }

    public interface IPlayerStateMachine : IStateMachine<PlayerStates>
    {
        ControlInputs GetInputs();
        IPlayerPresenter GetPresenter();
    }

    public class PlayerStateMachine : BaseStateMachine<PlayerStates>, IPlayerStateMachine
    {
        private PlayerPresenter _presenter;

        public PlayerStateMachine(PlayerPresenter presenter)
        {
            _presenter = presenter;
        }

        public ControlInputs GetInputs()
        {
            return _presenter.Inputs;
        }

        public IPlayerPresenter GetPresenter()
        {
            return _presenter;
        }
    }

    public abstract class PlayerState : BaseState<PlayerStates>
    {
        protected IPlayerStateMachine _stateMachine;
        protected IPlayerPresenter _presenter;
        protected ControlInputs _inputs;

        public PlayerState(IPlayerStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _inputs = _stateMachine.GetInputs();
            _presenter = _stateMachine.GetPresenter();
        }
    }

    public class PlayerAimingState : PlayerState
    {
        public PlayerAimingState(IPlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override PlayerStates State => PlayerStates.Aiming;

        public override void Enter()
        {
            base.Enter();
            _inputs.OnTargeting += _presenter.InvokeTargeting;
            _inputs.OnMoveLeft += _presenter.InvokeMoveLeft;
            _inputs.OnMoveRight += _presenter.InvokeMoveRight;
            _inputs.OnShot += _presenter.InvokeShot;
            _presenter.ShowTargetingRay(true);
        }

        public override void Exit()
        {
            base.Exit();
            _inputs.OnTargeting -= _presenter.InvokeTargeting;
            _inputs.OnMoveLeft -= _presenter.InvokeMoveLeft;
            _inputs.OnMoveRight -= _presenter.InvokeMoveRight;
            _inputs.OnShot -= _presenter.InvokeShot;
            _presenter.ShowTargetingRay(false);
        }
    }

    public class PlayerMoveOnlyState : PlayerState
    {
        public PlayerMoveOnlyState(IPlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override PlayerStates State => PlayerStates.MoveOnly;

        public override void Enter()
        {
            base.Enter();
            _inputs.OnMoveLeft += _presenter.InvokeMoveLeft;
            _inputs.OnMoveRight += _presenter.InvokeMoveRight;
        }

        public override void Exit()
        {
            base.Exit();
            _inputs.OnMoveLeft -= _presenter.InvokeMoveLeft;
            _inputs.OnMoveRight -= _presenter.InvokeMoveRight;
        }
    }

    public class PlayerInactiveState : PlayerState
    {
        public PlayerInactiveState(IPlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override PlayerStates State => PlayerStates.Inactive;
    }

}

