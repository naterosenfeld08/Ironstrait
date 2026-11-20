using Ironstrait.Audio;
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
        AudioSource _audio;

        int _shotgunAmmo;
        int _grenadeAmmo;
        float _shotgunCooldown;
        float _grenadeCooldown;
        float _shotgunReloadTimer;
        float _grenadeReloadTimer;
        bool _shotgunReloading;
        bool _grenadeReloading;

        // Per-weapon recoil — never applied to the shared viewmodel root.
        float _shotgunRecoil;
        float _grenadeRecoil;
        Vector3 _shotgunKick;
        Vector3 _grenadeKick;

        Transform _viewmodelRoot;
        Transform _shotgunVm;
        Transform _launcherVm;
        Vector3 _shotgunRestPos;
        Vector3 _launcherRestPos;
        Quaternion _shotgunRestRot;
        Quaternion _launcherRestRot;

        bool _shotgunWasReloading;
        bool _grenadeWasReloading;

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
            _audio = GetComponent<AudioSource>();
            if (_audio == null)
            {
                _audio = gameObject.AddComponent<AudioSource>();
                _audio.playOnAwake = false;
                _audio.spatialBlend = 0f;
            }

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
            _shotgunRecoil = 0f;
            _grenadeRecoil = 0f;
            _shotgunKick = Vector3.zero;
            _grenadeKick = Vector3.zero;
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
            _shotgunRecoil = Mathf.Lerp(_shotgunRecoil, 0f, 1f - Mathf.Exp(-10f * dt));
            _grenadeRecoil = Mathf.Lerp(_grenadeRecoil, 0f, 1f - Mathf.Exp(-10f * dt));
            _shotgunKick = Vector3.Lerp(_shotgunKick, Vector3.zero, 1f - Mathf.Exp(-12f * dt));
            _grenadeKick = Vector3.Lerp(_grenadeKick, Vector3.zero, 1f - Mathf.Exp(-12f * dt));

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

            if ((_shotgunWasReloading && !_shotgunReloading) || (_grenadeWasReloading && !_grenadeReloading))
            {
                PlaceholderSfx.Play(_audio, PlaceholderSfx.Reload, 0.4f, 1f);
            }

            _shotgunWasReloading = _shotgunReloading;
            _grenadeWasReloading = _grenadeReloading;
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
            _shotgunRecoil = cfg.visualRecoilDegrees;
            _shotgunKick = new Vector3(0f, 0.01f, -0.04f);
            PlaceholderSfx.Play(_audio, PlaceholderSfx.ShotgunFire, 0.7f, 1f);

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
            _grenadeRecoil = cfg.visualRecoilDegrees;
            _grenadeKick = new Vector3(0f, 0.012f, -0.035f);
            PlaceholderSfx.Play(_audio, PlaceholderSfx.GrenadeFire, 0.65f, 0.95f);

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

                    PlaceholderSfx.Play(_audio, PlaceholderSfx.Hit, 0.45f, 1.05f);
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

            // Keep root neutral — recoil is applied per gun only.
            _viewmodelRoot.localRotation = Quaternion.identity;
            _viewmodelRoot.localPosition = Vector3.zero;

            _shotgunVm = BuildShotgunViewmodel();
            _launcherVm = BuildGrenadeLauncherViewmodel();

            _shotgunRestPos = _shotgunVm.localPosition;
            _launcherRestPos = _launcherVm.localPosition;
            _shotgunRestRot = _shotgunVm.localRotation;
            _launcherRestRot = _launcherVm.localRotation;
        }

        Transform BuildShotgunViewmodel()
        {
            var root = new GameObject("ShotgunVM");
            root.transform.SetParent(_viewmodelRoot, false);
            root.transform.localPosition = new Vector3(-0.22f, -0.2f, 0.42f);
            root.transform.localRotation = Quaternion.Euler(2f, 4f, -2f);

            var body = new Color(0.22f, 0.22f, 0.25f);
            var wood = new Color(0.4f, 0.28f, 0.16f);
            var metal = new Color(0.35f, 0.36f, 0.38f);

            // Receiver
            AddPart(root.transform, "Receiver", PrimitiveType.Cube,
                new Vector3(0f, 0f, 0.05f), new Vector3(0.07f, 0.08f, 0.22f), body);
            // Barrel
            AddPart(root.transform, "Barrel", PrimitiveType.Cylinder,
                new Vector3(0f, 0.01f, 0.28f), new Vector3(0.035f, 0.14f, 0.035f), metal,
                Quaternion.Euler(90f, 0f, 0f));
            // Magazine tube
            AddPart(root.transform, "MagTube", PrimitiveType.Cylinder,
                new Vector3(0f, -0.03f, 0.22f), new Vector3(0.028f, 0.1f, 0.028f), metal,
                Quaternion.Euler(90f, 0f, 0f));
            // Pump
            AddPart(root.transform, "Pump", PrimitiveType.Cube,
                new Vector3(0f, -0.02f, 0.16f), new Vector3(0.055f, 0.05f, 0.1f), wood);
            // Stock
            AddPart(root.transform, "Stock", PrimitiveType.Cube,
                new Vector3(0f, -0.02f, -0.14f), new Vector3(0.05f, 0.07f, 0.18f), wood);
            // Pistol grip
            AddPart(root.transform, "Grip", PrimitiveType.Cube,
                new Vector3(0f, -0.07f, -0.02f), new Vector3(0.04f, 0.09f, 0.05f), wood,
                Quaternion.Euler(18f, 0f, 0f));
            // Front sight
            AddPart(root.transform, "Sight", PrimitiveType.Cube,
                new Vector3(0f, 0.04f, 0.38f), new Vector3(0.012f, 0.03f, 0.012f), metal);

            return root.transform;
        }

        Transform BuildGrenadeLauncherViewmodel()
        {
            var root = new GameObject("LauncherVM");
            root.transform.SetParent(_viewmodelRoot, false);
            root.transform.localPosition = new Vector3(0.26f, -0.22f, 0.38f);
            root.transform.localRotation = Quaternion.Euler(4f, -6f, 3f);

            var body = new Color(0.18f, 0.32f, 0.18f);
            var dark = new Color(0.15f, 0.18f, 0.15f);
            var metal = new Color(0.3f, 0.33f, 0.28f);

            // Thick launcher tube
            AddPart(root.transform, "Tube", PrimitiveType.Cylinder,
                new Vector3(0f, 0.02f, 0.12f), new Vector3(0.08f, 0.16f, 0.08f), body,
                Quaternion.Euler(90f, 0f, 0f));
            // Breech / drum
            AddPart(root.transform, "Breech", PrimitiveType.Cube,
                new Vector3(0f, 0f, -0.02f), new Vector3(0.12f, 0.12f, 0.1f), dark);
            // Stock
            AddPart(root.transform, "Stock", PrimitiveType.Cube,
                new Vector3(0f, -0.02f, -0.16f), new Vector3(0.05f, 0.06f, 0.14f), dark);
            // Grip
            AddPart(root.transform, "Grip", PrimitiveType.Cube,
                new Vector3(0f, -0.08f, -0.04f), new Vector3(0.04f, 0.1f, 0.05f), metal,
                Quaternion.Euler(20f, 0f, 0f));
            // Muzzle ring
            AddPart(root.transform, "Muzzle", PrimitiveType.Cylinder,
                new Vector3(0f, 0.02f, 0.28f), new Vector3(0.09f, 0.02f, 0.09f), metal,
                Quaternion.Euler(90f, 0f, 0f));
            // Ladder sight
            AddPart(root.transform, "Sight", PrimitiveType.Cube,
                new Vector3(0f, 0.08f, 0.02f), new Vector3(0.02f, 0.06f, 0.04f), metal);

            return root.transform;
        }

        static void AddPart(Transform parent, string name, PrimitiveType type, Vector3 localPos,
            Vector3 localScale, Color color, Quaternion? localRot = null)
        {
            var go = GameObject.CreatePrimitive(type);
            go.name = name;
            go.transform.SetParent(parent, false);
            go.transform.localPosition = localPos;
            go.transform.localRotation = localRot ?? Quaternion.identity;
            go.transform.localScale = localScale;
            Object.Destroy(go.GetComponent<Collider>());
            var r = go.GetComponent<Renderer>();
            r.material = new Material(Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard"));
            r.material.color = color;
            go.layer = 0;
        }

        void UpdateViewmodelPose()
        {
            // Shared root stays identity so one gun's recoil never rotates the other.
            if (_viewmodelRoot != null)
            {
                _viewmodelRoot.localRotation = Quaternion.identity;
                _viewmodelRoot.localPosition = Vector3.zero;
            }

            if (_shotgunVm != null)
            {
                _shotgunVm.localPosition = _shotgunRestPos + _shotgunKick;
                _shotgunVm.localRotation = _shotgunRestRot * Quaternion.Euler(-_shotgunRecoil, 0f, _shotgunRecoil * 0.15f);
            }

            if (_launcherVm != null)
            {
                _launcherVm.localPosition = _launcherRestPos + _grenadeKick;
                _launcherVm.localRotation = _launcherRestRot * Quaternion.Euler(-_grenadeRecoil, 0f, -_grenadeRecoil * 0.12f);
            }
        }
    }
}
