using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheGame
{
    public class FieldHelper : MonoBehaviour
    {
        [field: SerializeField] public Transform[] StartPositions { get; private set; }
    }
}

