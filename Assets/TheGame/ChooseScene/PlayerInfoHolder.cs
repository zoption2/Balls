using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TheGame
{
    [CreateAssetMenu(fileName = "NewPlayerInfoHolder", menuName = "ScriptableObjects/PlayerInfoHolder")]
    public class PlayerInfoHolder : ScriptableObject
    {
        [SerializeField] private List<Account> data;
        public IReadOnlyList<Account> Data => data;

    }

    [Serializable]
    public class Account
    {
        public string name;
        public Sprite icon;

        public static Account Guest => new Account() { name = "Guest", icon = null};
    }

}

