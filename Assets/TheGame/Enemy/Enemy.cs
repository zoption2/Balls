using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheGame
{
    public abstract class Enemy : MonoBehaviour, IPoolableEnemy
    {
        [SerializeField] protected EnemyHealthController healthController;
        [SerializeField] protected EnemyStats stats;
        [SerializeField] protected Interactor interactor;

        public abstract EnemyMoveType MoveType { get; }

        private void Start()
        {
            OnCreate();
        }

        public void Initialize(float health)
        {
            healthController.SetMaxHealth(health);
        }

        public void OnCreate()
        {
            healthController.Initialize(stats);
            interactor.Initialize(stats);
        }

        public void OnRestore()
        {
            throw new System.NotImplementedException();
        }

        public void OnStore()
        {
            throw new System.NotImplementedException();
        }
    }

    public enum EnemyMoveType
    {
        nextCell,
        overNextCell,
        randomeEmpty,
        randomeColumnNext
    }
}

