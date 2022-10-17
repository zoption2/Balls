using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheGame
{
    public class EnemyHealthController : MonoBehaviour
    {
        [SerializeField] private EnemyHealthUI healthUI;
        private EnemyStats stats;

        public void Initialize(EnemyStats stats)
        {
            this.stats = stats;
            this.stats.OnHealthChangeRequest += OperateHealthChange;
            healthUI.Initialize(stats.health);
            ResetHealth();
        }

        public void ResetHealth()
        {
            stats.health.SetToBase();
        }

        public void SetMaxHealth(float value)
        {
            stats.health.MaxValue = value;
            stats.health.SetToMax();
        }

        private void OperateHealthChange(float value)
        {
            var tempHealth = stats.health.Value;
            tempHealth += value;
            if (tempHealth <= 0)
            {
                //destroy
            }
            else
            {
                stats.health.Value = tempHealth;
            }
        }
    }
}

