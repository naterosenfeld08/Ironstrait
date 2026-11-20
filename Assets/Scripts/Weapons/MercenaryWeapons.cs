using Ironstrait.Combat;
using Ironstrait.Config;
using Ironstrait.Player;
using UnityEngine;

namespace Ironstrait.Weapons
{
    public class MercenaryWeapons : MonoBehaviour
    {
        FirstPersonController _controller;
        ScoreProgression _score;
        Health _health;

        int _shotgunAmmo;
        int _grenadeAmmo;
        float _shotgunCooldown;
        float _grenadeCooldown;
        float _shotgunReloadTimer;
        float _grenadeReloadTimer;
        bool _shotgunReloading;
        bool _grenadeReloading;
        float _viewmodelRecoil;

        Transform _viewmodelRoot;
        Transform _shotgunVm;
        Transform _launcherVm;

        public int ShotgunAmmo => _shotgunAmmo;
        public int GrenadeAmmo => _grenadeAmmo;
        public int ShotgunMagSize => GameConfig.Mercenary.shotgun.magazineSize;
        public int GrenadeMagSize => GameConfig.Mercenary.grenadeLauncher.magazineSize;
        public bool ShotgunReloading => _shotgunReloading;
        public bool GrenadeReloading => _grenadeReloading;
        public bool InputEnabled { get; set; } = true;

        public void Initialize(FirstPersonController controller, ScoreProgression score, Health health,
            Transform viewmodelRoot)
        {
            _controller = controller;
            _score = score;
            _health = health;
            _viewmodelRoot = viewmodelRoot;
            ResetAmmo();
            BuildViewmodels();
        }

        public void ResetAmmo()
        {
            var cfg = GameConfig.Mercenary;
            _shotgunAmmo = cfg.shotgun.magazineSize;
            _grenadeAmmo = cfg.grenadeLauncher.magazineSize;
            _shotgunCooldown = 0f;
            _grenadeCooldown = 0f;
            _shotgunReloading = false;
            _grenadeReloading = false;
            _shotgunReloadTimer = 0f;
            _grenadeReloadTimer = 0f;
        }

        void Update()
        {
            if (!InputEnabled || _controller == null || !_controller.InputEnabled)
            {
                return;
            }

            var dt = Time.deltaTime;
            _shotgunCooldown = Mathf.Max(0f, _shotgunCooldown - dt);
            _grenadeCooldown = Mathf.Max(0f, _grenadeCooldown - dt);
            _viewmodelRecoil = Mathf.Lerp(_viewmodelRecoil, 0f, 1f - Mathf.Exp(-10f * dt));

            TickReload(dt);

            // Independent fire — both can fire same frame.
            if (Input.GetMouseButton(0))
            {
                TryFireShotgun();
            }

            if (Input.GetMouseButton(1))
            {
                TryFireGrenade();
            }

            UpdateViewmodelPose();
        }

        void TickReload(float dt)
        {
            var reloadMul = _score != null ? _score.ReloadTimeMultiplier : 1f;
            var cfg = GameConfig.Mercenary;

            if (!_shotgunReloading && _shotgunAmmo < cfg.shotgun.magazineSize && _shotgunCooldown <= 0f)
            {
                _shotgunReloading = true;
                var missing = cfg.shotgun.magazineSize - _shotgunAmmo;
                var perShell = (cfg.shotgun.reloadEmptySeconds * reloadMul) / cfg.shotgun.magazineSize;
                _shotgunReloadTimer = perShell * missing;
            }

            if (!_grenadeReloading && _grenadeAmmo < cfg.grenadeLauncher.magazineSize && _grenadeCooldown <= 0f)
            {
                _grenadeReloading = true;
                var missing = cfg.grenadeLauncher.magazineSize - _grenadeAmmo;
                var perRound = (cfg.grenadeLauncher.reloadEmptySeconds * reloadMul) /
                               cfg.grenadeLauncher.magazineSize;
                _grenadeReloadTimer = perRound * missing;
            }

            if (_shotgunReloading)
            {
                _shotgunReloadTimer -= dt;
                if (_shotgunReloadTimer <= 0f)
                {
                    _shotgunAmmo = cfg.shotgun.magazineSize;
                    _shotgunReloading = false;
                }
            }

            if (_grenadeReloading)
            {
                _grenadeReloadTimer -= dt;
                if (_grenadeReloadTimer <= 0f)
                {
                    _grenadeAmmo = cfg.grenadeLauncher.magazineSize;
                    _grenadeReloading = false;
                }
            }
        }

        void TryFireShotgun()
        {
            var cfg = GameConfig.Mercenary.shotgun;
            if (_shotgunCooldown > 0f || _shotgunAmmo <= 0)
            {
                return;
            }

            _shotgunAmmo--;
            _shotgunCooldown = cfg.fireInterval;
            _shotgunReloading = false;
            _viewmodelRecoil = cfg.visualRecoilDegrees;

            var cam = _controller.PlayerCamera;
            if (cam == null)
            {
                return;
            }

            var pelletCount = _score != null ? _score.ShotgunPelletCount : cfg.pelletCount;
            var dmgMul = _score != null ? _score.ShotgunPelletDamageMultiplier : 1f;
            var damage = cfg.pelletDamage * dmgMul;

            // Deterministic plus pattern with center pellet.
            var offsets = BuildPlusPattern(pelletCount, cfg.spreadHalfAngleDegrees);
            for (var i = 0; i < offsets.Length; i++)
            {
                var dir = Quaternion.Euler(offsets[i].x, offsets[i].y, 0f) * cam.transform.forward;
                FireHitscanPellet(cam.transform.position, dir, damage, cfg.maxRange);
            }
        }

        void TryFireGrenade()
        {
            var cfg = GameConfig.Mercenary.grenadeLauncher;
            if (_grenadeCooldown > 0f || _grenadeAmmo <= 0)
            {
                return;
            }

            _grenadeAmmo--;
            _grenadeCooldown = cfg.fireInterval;
            _grenadeReloading = false;

            var cam = _controller.PlayerCamera;
            if (cam == null)
            {
                return;
            }

            var origin = cam.transform.position + cam.transform.forward * 0.4f + cam.transform.right * 0.15f;
            // Muzzle obstruction check
            if (Physics.Linecast(cam.transform.position, origin, out var block, ~0, QueryTriggerInteraction.Ignore))
            {
                origin = block.point - cam.transform.forward * 0.05f;
            }

            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.name = "Grenade";
            go.transform.localScale = Vector3.one * 0.22f;
            Object.Destroy(go.GetComponent<Collider>());
            var rend = go.GetComponent<Renderer>();
            rend.material = new Material(Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard"));
            rend.material.color = new Color(0.25f, 0.55f, 0.2f);

            var proj = go.AddComponent<GrenadeProjectile>();
            var direct = cfg.directDamage * (_score != null ? _score.GrenadeDirectDamageMultiplier : 1f);
            var radius = cfg.splashRadius * (_score != null ? _score.GrenadeSplashRadiusMultiplier : 1f);
            var selfMul = _score != null ? _score.GrenadeSelfDamageMultiplier : 1f;

            proj.Launch(
                origin,
                cam.transform.forward,
                gameObject,
                _score,
                direct,
                cfg.splashDamage,
                radius,
                cfg.projectileSpeed,
                cfg.gravity,
                cfg.lifetimeSeconds,
                cfg.collisionRadius,
                selfMul);
        }

        void FireHitscanPellet(Vector3 origin, Vector3 direction, float damage, float range)
        {
            if (Physics.Raycast(origin, direction, out var hit, range, ~0, QueryTriggerInteraction.Ignore))
            {
                Debug.DrawLine(origin, hit.point, Color.yellow, 0.05f);
                var dmg = hit.collider.GetComponentInParent<IDamageable>();
                if (dmg != null && dmg.IsAlive)
                {
                    var applied = dmg.ApplyDamage(damage, gameObject, hit.point, false);
                    if (_score != null && applied > 0f)
                    {
                        _score.AddDamageScore(applied);
                    }

                    SpawnHitMarker(hit.point, hit.normal);
                }
                else
                {
                    SpawnHitMarker(hit.point, hit.normal);
                }
            }
            else
            {
                Debug.DrawRay(origin, direction * range, Color.gray, 0.05f);
            }
        }

        static Vector2[] BuildPlusPattern(int count, float halfAngle)
        {
            // Center + four cardinals; level 10 adds three weaker outer (still same aim dirs for v0)
            if (count <= 1)
            {
                return new[] { Vector2.zero };
            }

            var list = new System.Collections.Generic.List<Vector2>
            {
                Vector2.zero,
                new Vector2(-halfAngle, 0f),
                new Vector2(halfAngle, 0f),
                new Vector2(0f, -halfAngle),
                new Vector2(0f, halfAngle)
            };

            if (count >= 8)
            {
                var outer = halfAngle * 1.7f;
                list.Add(new Vector2(-outer, -outer));
                list.Add(new Vector2(outer, -outer));
                list.Add(new Vector2(0f, outer * 1.2f));
            }

            while (list.Count > count)
            {
                list.RemoveAt(list.Count - 1);
            }

            return list.ToArray();
        }

        static void SpawnHitMarker(Vector3 point, Vector3 normal)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = "HitSpark";
            go.transform.position = point + normal * 0.02f;
            go.transform.localScale = Vector3.one * 0.08f;
            Object.Destroy(go.GetComponent<Collider>());
            var r = go.GetComponent<Renderer>();
            r.material = new Material(Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard"));
            r.material.color = Color.white;
            Object.Destroy(go, 0.12f);
        }

        void BuildViewmodels()
        {
            if (_viewmodelRoot == null)
            {
                return;
            }

            _shotgunVm = CreateGunMesh("ShotgunVM", new Vector3(-0.22f, -0.18f, 0.45f),
                new Vector3(0.12f, 0.1f, 0.4f), new Color(0.25f, 0.25f, 0.28f));
            _launcherVm = CreateGunMesh("LauncherVM", new Vector3(0.28f, -0.22f, 0.4f),
                new Vector3(0.14f, 0.12f, 0.35f), new Color(0.2f, 0.35f, 0.2f));
        }

        Transform CreateGunMesh(string name, Vector3 localPos, Vector3 scale, Color color)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = name;
            go.transform.SetParent(_viewmodelRoot, false);
            go.transform.localPosition = localPos;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = scale;
            Object.Destroy(go.GetComponent<Collider>());
            var r = go.GetComponent<Renderer>();
            r.material = new Material(Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard"));
            r.material.color = color;
            // Viewmodels should not be lit by world oddly — keep simple
            go.layer = 0;
            return go.transform;
        }

        void UpdateViewmodelPose()
        {
            if (_viewmodelRoot == null)
            {
                return;
            }

            _viewmodelRoot.localRotation = Quaternion.Euler(-_viewmodelRecoil, 0f, 0f);
        }
    }
}
