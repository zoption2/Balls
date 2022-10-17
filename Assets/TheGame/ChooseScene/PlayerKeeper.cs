using System.Collections.Generic;
using UnityEngine;

namespace TheGame
{
    [CreateAssetMenu(fileName = "PlayersKeeper", menuName = "ScriptableObjects/PlayersKeeper")]
    public class PlayerKeeper : ScriptableObject
    {
        private List<PlayerCore> data = new List<PlayerCore>();
        public void AddPlayer(PlayerCore core)
        {
            if (data.Contains(core))
            {
                data.Remove(core);
            }
            data.Add(core);
        }

        public void RemovePlayer(PlayerCore core)
        {
            if (data.Contains(core))
            {
                data.Remove(core);
            }
        }
    }
}

