using System;
using Ironstrait.Config;
using UnityEngine;

namespace Ironstrait.Combat
{
    public class Health : MonoBehaviour, IDamageable
    {
        public event Action<float, float> HealthChanged;
        public event Action<GameObject> Died;

        float _maxHealth = 150f;
        float _current;
        float _regenPerSecond = 10f;
        float _regenDelay = 10f;
        float _lastDamageTime = float.NegativeInfinity;
        bool _dead;

        public bool IsAlive => !_dead && _current > 0f;
        public float CurrentHealth => _current;
        public float MaxHealth => _maxHealth;

        public void ConfigureFromMercenary()
        {
            var cfg = GameConfig.Mercenary;
            Configure(cfg.maxHealth, cfg.regenPerSecond, cfg.regenDelaySeconds);
        }

        public void Configure(float maxHealth, float regenPerSecond, float regenDelay)
        {
            _maxHealth = Mathf.Max(1f, maxHealth);
            _regenPerSecond = Mathf.Max(0f, regenPerSecond);
            _regenDelay = Mathf.Max(0f, regenDelay);
            _current = _maxHealth;
            _dead = false;
            HealthChanged?.Invoke(_current, _maxHealth);
        }

        public void ResetFull()
        {
            _dead = false;
            _current = _maxHealth;
            _lastDamageTime = float.NegativeInfinity;
            HealthChanged?.Invoke(_current, _maxHealth);
        }

        public void SetHealth(float value)
        {
            if (_dead && value <= 0f)
            {
                return;
            }

            _dead = false;
            _current = Mathf.Clamp(value, 0f, _maxHealth);
            HealthChanged?.Invoke(_current, _maxHealth);
            if (_current <= 0f)
            {
                HandleDeath(null);
            }
        }

        public float ApplyDamage(float amount, GameObject source, Vector3 hitPoint, bool isSplash)
        {
            if (!IsAlive || amount <= 0f)
            {
                return 0f;
            }

            var applied = Mathf.Min(amount, _current);
            _current -= applied;
            _lastDamageTime = Time.time;
            HealthChanged?.Invoke(_current, _maxHealth);

            if (_current <= 0f)
            {
                HandleDeath(source);
            }

            return applied;
        }

        public float ApplyHeal(float amount)
        {
            if (!IsAlive || amount <= 0f)
            {
                return 0f;
            }

            var before = _current;
            _current = Mathf.Min(_maxHealth, _current + amount);
            var healed = _current - before;
            if (healed > 0f)
            {
                HealthChanged?.Invoke(_current, _maxHealth);
            }

            return healed;
        }

        void Update()
        {
            if (!IsAlive || _regenPerSecond <= 0f)
            {
                return;
            }

            if (Time.time - _lastDamageTime < _regenDelay)
            {
                return;
            }

            if (_current >= _maxHealth)
            {
                return;
            }

            ApplyHeal(_regenPerSecond * Time.deltaTime);
        }

        void HandleDeath(GameObject source)
        {
            if (_dead)
            {
                return;
            }

            _dead = true;
            _current = 0f;
            Died?.Invoke(source);
        }
    }
}
