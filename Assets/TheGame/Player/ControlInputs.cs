using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

namespace TheGame
{
    public class ControlInputs
    {
        private IControlActions input;
        private PlayerInput playerInput;

        public Action<Phase> OnMoveLeft;
        public Action<Phase> OnMoveRight;
        public Action<Phase> OnShot;
        public Action<Vector2, Phase> OnTargeting;

        public ControlInputs(PlayerInput playerInput)
        {
            this.playerInput = playerInput;
            this.playerInput.ActivateInput();
            input = new ControlActions(playerInput);
            input.MoveLeft.Subscribe(DoMoveLeft);
            input.MoveRight.Subscribe(DoMoveRight);
            input.Shot.Subscribe(DoOnShot);
            input.Targeting.Subscribe(DoOnTargeting);
        }

        private void DoMoveLeft(Phase phase)
        {
            OnMoveLeft?.Invoke(phase);
        }

        private void DoMoveRight(Phase phase)
        {
            OnMoveRight?.Invoke(phase);
        }

        private void DoOnShot(Phase phase)
        {
            OnShot?.Invoke(phase);
        }

        private void DoOnTargeting(Vector2 direction, Phase phase)
        {
            OnTargeting?.Invoke(direction, phase);
        }
    }

    public interface IControlActions
    {
        public PressingRoutine MoveLeft { get; }
        public PressingRoutine MoveRight { get; }
        public PressingRoutine Shot { get; }
        public Vector2Routine Targeting { get; }
    }

    public class ControlActions : IControlActions
    {
        private PlayerInput playerInput;

        private InputAction moveLeftAction;
        private InputAction moveRightAction;
        private InputAction shotACtion;
        private InputAction targetingAction;

        private const string moveLeft = "MoveLeft";
        private const string moveRight = "MoveRight";
        private const string shot = "Shot";
        private const string targeting = "Targeting";

        public PressingRoutine MoveLeft { get;}
        public PressingRoutine MoveRight { get; }
        public PressingRoutine Shot { get; }
        public Vector2Routine Targeting { get; }

        public ControlActions(PlayerInput playerInput)
        {
            this.playerInput = playerInput;

            MoveLeft = new PressingRoutine(playerInput, moveLeft);
            MoveRight = new PressingRoutine(playerInput, moveRight);
            Shot = new PressingRoutine(playerInput, shot);
            Targeting = new Vector2Routine(playerInput, targeting);
        }
    }

    public class PressingRoutine
    {
        private bool isWorking;
        private Action<Phase> onPerforme;
        private PlayerInput input;

        private UnityEngine.WaitForEndOfFrame waitFor = new UnityEngine.WaitForEndOfFrame();

        public PressingRoutine(PlayerInput input, string actionName)
        {
            this.input = input;
            var action = input.actions[actionName];
            action.started += ctx => Start();
            action.canceled += ctx => Cancel();
            action.Enable();
        }

        public void Subscribe(Action<Phase> action)
        {
            onPerforme += action;
        }

        public void Unsubscribe(Action<Phase> action)
        {
            onPerforme -= action;
        }

        private void Start()
        {
            isWorking = true;
            input.StartCoroutine(WorkRoutine());
        }

        private void Cancel()
        {
            isWorking = false;
        }

        private IEnumerator WorkRoutine()
        {
            onPerforme?.Invoke(Phase.started);
            while (isWorking)
            {
                onPerforme?.Invoke(Phase.inProgress);
                yield return waitFor;
            }
            onPerforme?.Invoke(Phase.complete);
        }
    }

    public class Vector2Routine
    {
        private bool isWorking;
        private Action<Vector2, Phase> onPerforme;
        private PlayerInput input;
        private InputAction action;

        private UnityEngine.WaitForEndOfFrame waitFor = new UnityEngine.WaitForEndOfFrame();

        public Vector2Routine(PlayerInput input, string actionName)
        {
            this.input = input;
            action = input.actions[actionName];
            action.started += ctx => Start();
            action.canceled += ctx => Cancel();
            action.Enable();
        }

        public void Subscribe(Action<Vector2, Phase> action)
        {
            onPerforme += action;
        }

        public void Unsubscribe(Action<Vector2, Phase> action)
        {
            onPerforme -= action;
        }

        private void Start()
        {
            isWorking = true;
            input.StartCoroutine(WorkRoutine());
        }

        private void Cancel()
        {
            isWorking = false;
        }

        private IEnumerator WorkRoutine()
        {
            onPerforme?.Invoke(action.ReadValue<Vector2>(), Phase.started);
            while (isWorking)
            {
                onPerforme?.Invoke(action.ReadValue<Vector2>(), Phase.inProgress);
                yield return waitFor;
            }
            onPerforme?.Invoke(action.ReadValue<Vector2>(), Phase.complete);
        }
    }

    public enum Phase
    {
        started,
        inProgress,
        complete
    }
}

