using Ironstrait.Combat;
using Ironstrait.Config;
using UnityEngine;

namespace Ironstrait.Weapons
{
    public class GrenadeProjectile : MonoBehaviour
    {
        Vector3 _velocity;
        float _gravity;
        float _lifetime;
        float _age;
        float _directDamage;
        float _splashDamage;
        float _splashRadius;
        float _collisionRadius;
        float _selfDamageMultiplier = 1f;
        GameObject _owner;
        ScoreProgression _ownerScore;
        bool _detonated;
        TrailRenderer _trail;

        public void Launch(
            Vector3 origin,
            Vector3 direction,
            GameObject owner,
            ScoreProgression ownerScore,
            float directDamage,
            float splashDamage,
            float splashRadius,
            float speed,
            float gravity,
            float lifetime,
            float collisionRadius,
            float selfDamageMultiplier)
        {
            transform.position = origin;
            _velocity = direction.normalized * speed;
            _owner = owner;
            _ownerScore = ownerScore;
            _directDamage = directDamage;
            _splashDamage = splashDamage;
            _splashRadius = splashRadius;
            _gravity = gravity;
            _lifetime = lifetime;
            _collisionRadius = collisionRadius;
            _selfDamageMultiplier = selfDamageMultiplier;

            _trail = gameObject.AddComponent<TrailRenderer>();
            _trail.time = 0.35f;
            _trail.startWidth = 0.08f;
            _trail.endWidth = 0.01f;
            _trail.material = new Material(Shader.Find("Sprites/Default"));
            _trail.startColor = new Color(1f, 0.55f, 0.15f, 1f);
            _trail.endColor = new Color(1f, 0.2f, 0.05f, 0f);
        }

        void Update()
        {
            if (_detonated)
            {
                return;
            }

            _age += Time.deltaTime;
            if (_age >= _lifetime)
            {
                Detonate(null, transform.position);
                return;
            }

            _velocity.y += _gravity * Time.deltaTime;
            var delta = _velocity * Time.deltaTime;
            var dist = delta.magnitude;
            if (dist <= 0f)
            {
                return;
            }

            if (Physics.SphereCast(transform.position, _collisionRadius, delta.normalized, out var hit, dist,
                    ~0, QueryTriggerInteraction.Ignore))
            {
                transform.position = hit.point + hit.normal * 0.02f;
                Detonate(hit.collider, hit.point);
                return;
            }

            transform.position += delta;
        }

        void Detonate(Collider directHit, Vector3 point)
        {
            if (_detonated)
            {
                return;
            }

            _detonated = true;

            IDamageable directDamageable = null;
            if (directHit != null)
            {
                directDamageable = directHit.GetComponentInParent<IDamageable>();
            }

            // Direct hit: 50 total, no extra splash on that target.
            if (directDamageable != null && IsValidTarget(directDamageable))
            {
                var applied = directDamageable.ApplyDamage(_directDamage, _owner, point, false);
                if (_ownerScore != null && applied > 0f && !IsOwner(directDamageable))
                {
                    _ownerScore.AddDamageScore(applied);
                }
            }

            var radius = _splashRadius;
            var cols = Physics.OverlapSphere(point, radius);
            foreach (var col in cols)
            {
                var dmg = col.GetComponentInParent<IDamageable>();
                if (dmg == null || !IsValidTarget(dmg))
                {
                    continue;
                }

                if (dmg == directDamageable)
                {
                    continue; // already took direct
                }

                if (!HasBlastLos(point, col))
                {
                    continue;
                }

                var amount = _splashDamage;
                if (IsOwner(dmg))
                {
                    amount *= _selfDamageMultiplier;
                }

                var applied = dmg.ApplyDamage(amount, _owner, point, true);
                if (_ownerScore != null && applied > 0f && !IsOwner(dmg))
                {
                    _ownerScore.AddDamageScore(applied);
                }
            }

            SpawnBurst(point);
            Destroy(gameObject);
        }

        bool IsOwner(IDamageable dmg)
        {
            if (_owner == null)
            {
                return false;
            }

            var mb = dmg as MonoBehaviour;
            return mb != null && (mb.gameObject == _owner || mb.transform.IsChildOf(_owner.transform));
        }

        bool IsValidTarget(IDamageable dmg) => dmg != null && dmg.IsAlive;

        static bool HasBlastLos(Vector3 origin, Collider target)
        {
            var to = target.bounds.center;
            var dir = to - origin;
            var dist = dir.magnitude;
            if (dist < 0.01f)
            {
                return true;
            }

            if (Physics.Raycast(origin + dir.normalized * 0.05f, dir.normalized, out var hit, dist,
                    ~0, QueryTriggerInteraction.Ignore))
            {
                return hit.collider == target || hit.collider.transform.IsChildOf(target.transform);
            }

            return true;
        }

        static void SpawnBurst(Vector3 point)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.name = "GrenadeBurst";
            go.transform.position = point;
            go.transform.localScale = Vector3.one * 0.4f;
            Object.Destroy(go.GetComponent<Collider>());
            var r = go.GetComponent<Renderer>();
            r.material = new Material(Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard"));
            r.material.color = new Color(1f, 0.45f, 0.1f, 1f);
            Object.Destroy(go, 0.2f);
        }
    }
}
