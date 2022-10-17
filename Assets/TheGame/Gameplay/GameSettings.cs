using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheGame
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings")]
    public class GameSettings : ScriptableObject
    {
        [field: SerializeField] public int TotalPlayers { get; set; }
        [field: SerializeField] public int TotalTeams { get; set; }
        [field: SerializeField] public ScenarioType Scenario { get; set; }
    }
}

