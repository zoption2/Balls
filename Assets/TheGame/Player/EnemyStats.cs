using UnityEngine;

namespace TheGame
{

    public class EnemyStats : MonoBehaviour, IEnemyStatsGetter
    {
        [field: SerializeField] public Stat health { get; private set; }
        [SerializeField] public BooleanStat isFrozen { get; private set; }
        [SerializeField] public BooleanStat isBurning { get; private set; }

        public System.Action<float> OnHealthChangeRequest;
        public System.Action<float> OnMaxHealthChangeRequest;
        public System.Action OnFrozeRequest;
        public System.Action OnBurnRequest;

        public float CurrentHealth => health.Value;
        public float MaxHealth => health.MaxValue;
        public bool IsFrozen => isFrozen.Value;
        public bool IsBurning => isBurning.Value;

        public void ChangeHealthRequest(float value)
        {
            OnHealthChangeRequest?.Invoke(value);
        }

        public void ChangeMaxHealthRequest(float value)
        {
            OnMaxHealthChangeRequest?.Invoke(value);
        }

        public void FrozeRequest()
        {
            OnFrozeRequest?.Invoke();
        }

        public void BurnReques()
        {
            OnBurnRequest?.Invoke();
        }
    }

    public interface IEnemyStatsGetter
    {
        float CurrentHealth { get; }
        float MaxHealth { get; }
        bool IsFrozen { get; }
        bool IsBurning { get; }

        void ChangeHealthRequest(float value);
        void ChangeMaxHealthRequest(float value);
        void FrozeRequest();
    }


    [System.Serializable]
    public class Stat
    {
        private float _value;
        [SerializeField] private float minValue;
        [SerializeField] private float maxValue;
        [SerializeField] private float baseValue;

        public float MaxValue
        {
            get => maxValue;
            set => maxValue = value;
        }

        public float Value 
        {
            get { return _value; } 
            set 
            { 
                _value = Mathf.Clamp(value, minValue, maxValue);
                OnValueChanged?.Invoke(_value);
            }
        }

        public System.Action<float> OnValueChanged;

        public Stat(float baseValue, float maxValue = int.MaxValue, float minValue = 0)
        {
            this.maxValue = maxValue;
            this.minValue = minValue;
            this.baseValue = baseValue;
            Value = baseValue;
        }

        public void SetToBase()
        {
            Value = baseValue;
        }

        public void SetToMax()
        {
            Value = maxValue;
        }
    }

    [System.Serializable]
    public class BooleanStat
    {
        private bool _value;
        private bool baseValue;

        public bool Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnValueChanged?.Invoke();
            }
        }

        public System.Action OnValueChanged;

        public BooleanStat(bool baseValue)
        {
            this.baseValue = baseValue;
            Value = baseValue;
        }

        public void SetToBase()
        {
            Value = baseValue;
        }
    }
}

