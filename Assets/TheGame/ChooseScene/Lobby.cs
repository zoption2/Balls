using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using System.Linq;

namespace TheGame
{
    public class Lobby : MonoBehaviour
    {
        [SerializeField] private PlayerKeeper keeper;
        [SerializeField] private PlayerAtLobby[] players;
        [SerializeField] private PlayerInputManager inputManager;
        [Inject] private ISelectPlayer selectPlayer;
        private int count;
        private float offset = 150;
        private Dictionary<PlayerInput, PlayerAtLobby> playersPairs = new Dictionary<PlayerInput, PlayerAtLobby>();
        //private PlayerInputManager InputManager => PlayerInputManager.instance;

        private void OnEnable()
        {
            inputManager.onPlayerJoined += AddPlayer;
        }

        private void OnDisable()
        {
            inputManager.onPlayerJoined -= AddPlayer;
        }

        public void AddPlayer(PlayerInput input)
        {
            if (!playersPairs.ContainsKey(input))
            {
                var bar = players.FirstOrDefault(x => !x.IsActivated);
                playersPairs.Add(input, bar);
                bar.Init(input, this, selectPlayer);
            }
        }

        public void SavePlayer(PlayerCore core)
        {
            keeper.AddPlayer(core);
        }
    }

    [System.Serializable]
    public class InternalPlayer
    {
        public PlayerInput PlayerInput { get; }
        public IIdentifiers Identifiers { get; private set; }
    }
}

