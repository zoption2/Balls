using UnityEngine;
using TMPro;

namespace TheGame
{
    public class EnemyHealthUI : MonoBehaviour
    {
        [SerializeField] private TextMeshPro textMP;
        private Stat health;

        public void Initialize(Stat health)
        {
            this.health = health;
            ChangeHealth(health.Value);
            health.OnValueChanged += ChangeHealth;
        }

        private void ChangeHealth(float value)
        {
            textMP.text = value.ToString();
        }
    }
}

