using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace TheGame
{
    public class GameStart : MonoBehaviour
    {
        [SerializeField] private int playersCount = 1;
        [SerializeField] private Transform startPoint;

        [Inject] private IGameplayController gameplayController;
        [Inject] private PlayerView testPlayer;

        private void Start()
        {
            Identifiers identifiers = new Identifiers(0, 0);
            testPlayer.Initialize(identifiers);
            testPlayer.transform.position = startPoint.position;
            gameplayController.AddPlayer(testPlayer);
            gameplayController.StartGame();
        }
    }
}

