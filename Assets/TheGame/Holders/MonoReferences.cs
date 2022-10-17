using UnityEngine;

namespace TheGame
{
    [CreateAssetMenu(fileName = "MonoReferences", menuName = "ScriptableObjects/MonoReferences")]
    public class MonoReferences : ScriptableObject
    {
        [field: SerializeField] public PlayerFactory PlayerFactory { get; private set; }
        [field: SerializeField] public BallsFactory BallsFactory { get; private set; }
        [field: SerializeField] public FieldFactory FieldsFactory { get; private set; }
    }
}



