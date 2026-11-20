using System;
using UnityEngine;

namespace Ironstrait.Config
{
    [Serializable]
    public class WeaponShotgunConfig
    {
        public int magazineSize = 6;
        public int pelletCount = 5;
        public float pelletDamage = 15f;
        public float fireInterval = 0.65f;
        public float reloadEmptySeconds = 3.5f;
        public float spreadHalfAngleDegrees = 2.2f;
        public float maxRange = 80f;
        public float visualRecoilDegrees = 4f;
    }

    [Serializable]
    public class WeaponGrenadeConfig
    {
        public int magazineSize = 3;
        public float directDamage = 50f;
        public float splashDamage = 15f;
        public float splashRadius = 2.25f;
        public float fireInterval = 0.75f;
        public float reloadEmptySeconds = 4f;
        public float projectileSpeed = 28f;
        public float gravity = -12f;
        public float lifetimeSeconds = 10f;
        public bool selfDamageEqualsSplash = true;
        public float collisionRadius = 0.18f;
    }

    [Serializable]
    public class ScoreConfig
    {
        public float damagePerPoint = 100f;
        public float[] levelThresholds = { 0, 2, 4, 7, 10, 14, 18, 22, 26, 30 };
    }

    [Serializable]
    public class MercenaryConfig
    {
        public int schemaVersion = 1;
        public string classId = "mercenary";
        public float maxHealth = 150f;
        public float regenPerSecond = 10f;
        public float regenDelaySeconds = 10f;
        public float moveSpeed = 8.5f;
        public float crouchSpeedMultiplier = 0.5f;
        public float jumpSpeed = 7.5f;
        public float gravity = -22f;
        public float groundAcceleration = 80f;
        public float airAcceleration = 28f;
        public float groundFriction = 14f;
        public float airStrafeControl = 1f;
        public float mouseSensitivity = 0.12f;
        public float defaultFov = 90f;
        public float slideLeapDistance = 6f;
        public float slideLeapChargeDelay = 3f;
        public float slideLeapRefillSeconds = 3f;
        public WeaponShotgunConfig shotgun = new WeaponShotgunConfig();
        public WeaponGrenadeConfig grenadeLauncher = new WeaponGrenadeConfig();
        public ScoreConfig score = new ScoreConfig();
    }

    public static class GameConfig
    {
        const string MercenaryResourcePath = "Config/mercenary_baseline";

        static MercenaryConfig _mercenary;

        public static MercenaryConfig Mercenary
        {
            get
            {
                if (_mercenary == null)
                {
                    _mercenary = LoadMercenary();
                }

                return _mercenary;
            }
        }

        public static void Reload()
        {
            _mercenary = LoadMercenary();
        }

        static MercenaryConfig LoadMercenary()
        {
            var asset = Resources.Load<TextAsset>(MercenaryResourcePath);
            if (asset == null)
            {
                Debug.LogWarning("Missing mercenary_baseline.json — using code defaults.");
                return new MercenaryConfig();
            }

            try
            {
                var cfg = JsonUtility.FromJson<MercenaryConfig>(asset.text);
                return cfg ?? new MercenaryConfig();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to parse mercenary config: {ex.Message}");
                return new MercenaryConfig();
            }
        }
    }
}
