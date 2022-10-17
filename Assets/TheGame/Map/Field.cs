using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheGame
{
    public class Field : MonoBehaviour
    {
        [field: SerializeField] public GridController GridController { get; private set; }
        [field: SerializeField] public FieldHelper Helper { get; private set; }

        public int TeamID { get; private set; } 

        public void Initialize(int teamID)
        {
            TeamID = teamID;
            GridController.Initialize();
        }
    }

    public enum ScenarioType
    {
        SinglePlayer,
        TwoTeams,
        AllThemselves,
        CoopBigMap
    }
}

