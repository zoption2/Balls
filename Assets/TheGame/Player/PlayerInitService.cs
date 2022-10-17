using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

namespace TheGame
{
    public class PlayerInitService
    {
        private List<PlayerSettings> data = new List<PlayerSettings>();
        public IReadOnlyList<PlayerSettings> Data => data;

        public void AddPlayer(PlayerInput playerInput, out int id)
        {
            var newSettings = new PlayerSettings();
            newSettings.PlayerInput = playerInput;
            id = data.Count;
            data.Add(newSettings);
        }

        [Serializable]
        public class PlayerSettings
        {
            public PlayerInput PlayerInput { get; set; }
        }
    }
}

