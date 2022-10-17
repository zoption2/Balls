using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace TheGame
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private GameSettings settings;
        private IGameplayController gameController;


        [Inject]
        private void Inject(IGameplayController gameController)
        {
            this.gameController = gameController;
        }

        private void Start()
        {
            gameController.Initialize(settings, DoOnLoadingComplete);
        }

        private void DoOnLoadingComplete()
        {

        }
    }
}

