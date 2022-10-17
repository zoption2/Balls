using UnityEngine;

namespace TheGame
{
    public class PlayerStats : MonoBehaviour
    {
        [field: SerializeField] public Stat TargetingSpeed { get; private set; }
        [field: SerializeField] public Stat MoveSpeed { get; private set; }
    }
}

