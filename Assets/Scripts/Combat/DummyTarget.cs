using UnityEngine;

namespace Ironstrait.Combat
{
    public class DummyTarget : MonoBehaviour
    {
        Health _health;
        Renderer _renderer;
        Color _baseColor;
        float _flashUntil;
        Vector3 _spawnPos;
        float _respawnAt = -1f;
        float _respawnDelay = 2f;
        TextMesh _label;

        public float MaxHealthOverride = 150f;

        public void Initialize(float maxHealth, Color color)
        {
            MaxHealthOverride = maxHealth;
            _spawnPos = transform.position;
            _health = gameObject.AddComponent<Health>();
            _health.Configure(maxHealth, 0f, 999f);
            _health.HealthChanged += OnHealthChanged;
            _health.Died += OnDied;

            _renderer = GetComponentInChildren<Renderer>();
            if (_renderer != null)
            {
                _renderer.material = new Material(Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard"));
                _renderer.material.color = color;
                _baseColor = color;
            }

            var labelGo = new GameObject("Label");
            labelGo.transform.SetParent(transform, false);
            labelGo.transform.localPosition = Vector3.up * 1.3f;
            _label = labelGo.AddComponent<TextMesh>();
            _label.characterSize = 0.08f;
            _label.fontSize = 48;
            _label.anchor = TextAnchor.LowerCenter;
            _label.alignment = TextAlignment.Center;
            _label.color = Color.white;
            RefreshLabel();
        }

        void Update()
        {
            if (_renderer != null && Time.time < _flashUntil)
            {
                _renderer.material.color = Color.Lerp(_baseColor, Color.white, 0.7f);
            }
            else if (_renderer != null && _health != null && _health.IsAlive)
            {
                _renderer.material.color = _baseColor;
            }

            if (_respawnAt > 0f && Time.time >= _respawnAt)
            {
                Respawn();
            }

            if (_label != null && Camera.main != null)
            {
                _label.transform.rotation = Quaternion.LookRotation(
                    _label.transform.position - Camera.main.transform.position);
            }
        }

        void OnHealthChanged(float current, float max)
        {
            _flashUntil = Time.time + 0.08f;
            RefreshLabel();
        }

        void OnDied(GameObject source)
        {
            if (_renderer != null)
            {
                _renderer.material.color = new Color(0.15f, 0.15f, 0.15f);
            }

            RefreshLabel();
            _respawnAt = Time.time + _respawnDelay;
        }

        void Respawn()
        {
            _respawnAt = -1f;
            transform.position = _spawnPos;
            _health.ResetFull();
            if (_renderer != null)
            {
                _renderer.material.color = _baseColor;
            }

            RefreshLabel();
        }

        void RefreshLabel()
        {
            if (_label == null || _health == null)
            {
                return;
            }

            _label.text = _health.IsAlive
                ? $"{Mathf.CeilToInt(_health.CurrentHealth)}"
                : "DOWN";
        }

        public void ForceReset()
        {
            Respawn();
        }
    }
}
