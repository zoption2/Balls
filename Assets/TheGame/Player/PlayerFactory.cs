using UnityEngine;

namespace TheGame
{
    public class PlayerFactory : MonoBehaviour, IPlayerFactory
    {
        [SerializeField] private PlayerView _playerPrefab;

        public IPlayerView GetPlayer(Vector2 position, Transform parent = null)
        {
            var player = Instantiate(_playerPrefab, position, Quaternion.identity, parent);
            return player;
        }
    }

    public interface IPlayerFactory
    {
        IPlayerView GetPlayer(Vector2 position, Transform parent = null);
    }
}

