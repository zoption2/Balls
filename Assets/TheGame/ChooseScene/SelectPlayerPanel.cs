using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.InputSystem.UI;

namespace TheGame
{
    public class SelectPlayerPanel : MonoBehaviour, ISelectPlayer, IPlayerInfoHandler
    {
        [SerializeField] private PlayerInfoHolder infoHolder;
        [SerializeField] private PlayerInfoBar infoBarPrefab;
        [SerializeField] private Transform parent;
        private List<PlayerInfoBar> allBars = new List<PlayerInfoBar>();
        private bool isOpened;
        private bool isBarFilled;
        private Account origin;
        private Account selected;
        private int firstPosition = -200;
        private int step = 120;
        private TaskCompletionSource<State> tcs;
        private MultiplayerEventSystem eventSystem;
        private InputSystemUIInputModule uiModule;

        private enum State
        {
            confirmed,
            canceled
        }

        private State ? state;

        public async Task<Account> SelectPlayer(Account origin, MultiplayerEventSystem eventSystem, InputSystemUIInputModule uiModule)
        {
            if (isOpened)
            {
                return origin;
            }

            state = null;
            tcs = new TaskCompletionSource<State>();
            isOpened = true;
            this.origin = origin;
            gameObject.SetActive(true);
            DisplayAllPlayers();
            EnableEventSystem(eventSystem);
            uiModule.ActivateModule();
            await tcs.Task;
            gameObject.SetActive(false);
            isOpened = false;
            DisableEventSystem();
            return this.origin;
        }

        private void EnableEventSystem(MultiplayerEventSystem eventSystem)
        {
            this.eventSystem = eventSystem;
            eventSystem.playerRoot = gameObject;
            eventSystem.firstSelectedGameObject = allBars[0].gameObject;
            eventSystem.UpdateModules();
        }

        private void DisableEventSystem()
        {
            eventSystem.playerRoot = null;
            eventSystem.firstSelectedGameObject = null;
        }

        private void DisplayAllPlayers()
        {
            if (!isBarFilled)
            {
                int currentStep = firstPosition;
                for (int i = 0, j = infoHolder.Data.Count; i < j; i++)
                {
                    var info = Instantiate(infoBarPrefab, parent);
                    allBars.Add(info);
                    bool isSelected = i == 0;
                    info.Init(infoHolder.Data[i], currentStep, this, isSelected);
                    currentStep -= step;
                }
                isBarFilled = true;
            }
        }

        public void ClosePanel()
        {
            tcs.SetResult(State.canceled);
        }

        public void SelectInfo(Account info)
        {
            origin = info;
            tcs.SetResult(State.confirmed);
        }
    }

    public interface IPlayerInfoHandler
    {
        void SelectInfo(Account info);
    }

    public interface ISelectPlayer
    {
        Task<Account> SelectPlayer(Account origin, MultiplayerEventSystem eventSystem, InputSystemUIInputModule uiModule);
    }
}

