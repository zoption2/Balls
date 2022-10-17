using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using Zenject;
using TMPro;

namespace TheGame
{
    public class PlayerAtLobby : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private MultiplayerEventSystem eventSystem;
        [SerializeField] private InputSystemUIInputModule uiModule;

        public bool IsActivated { get; private set; }
        private Lobby lobby;
        private PlayerInput playerInput;
        private PlayerCore core;

        public async void Init(PlayerInput input, Lobby lobby, ISelectPlayer selectPlayer)
        {
            IsActivated = true;
            playerInput = input;
            input.uiInputModule = uiModule;
            input.defaultActionMap = "UI";
            core = new PlayerCore();
            core.DeviceID = input.devices[0].deviceId;
            core.Account = Account.Guest;
            this.lobby = lobby;
            gameObject.SetActive(true);
            UpdateUI();
            var account = selectPlayer.SelectPlayer(core.Account, eventSystem, uiModule);
            await account;
            core.Account = account.Result;
            UpdateUI();
        }

        private void UpdateUI()
        {
            text.text = core.Account.name;
        }
    }

    [System.Serializable]
    public class PlayerCore
    {
        [field: SerializeField] public int DeviceID { get; set; }
        [field: SerializeField] public Identifiers Identifiers { get; set; }
        [field: SerializeField] public Account Account { get; set; }
    }

}

