using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace TheGame
{
    public class PlayerFactory : MonoBehaviour, IPlayerFactory
    {
        [field: SerializeField] public Player PlayerPrefab { get; private set; }
        [Inject] private IMonoInstantiator instantiator;

        public Player GetPlayer(int id, int teamID, Vector2 position)
        {
            var instance = instantiator.CreateObject(PlayerPrefab, position);
            var player = instance.GetComponent<Player>();
            Identifiers identifiers = new Identifiers(id, teamID);
            player.Initialize(identifiers);
            return player;
        }
    }

    public interface IPlayerFactory
    {
        Player GetPlayer(int id, int teamID, Vector2 position);
    }
}

