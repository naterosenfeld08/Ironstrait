using Ironstrait.Combat;
using UnityEngine;

namespace Ironstrait.Arena
{
    public static class GrayboxArena
    {
        public static Transform Root { get; private set; }
        public static Vector3 PlayerSpawn { get; private set; } = new Vector3(0f, 1f, -8f);

        public static void Build()
        {
            if (Root != null)
            {
                Object.Destroy(Root.gameObject);
            }

            var root = new GameObject("GrayboxArena");
            Root = root.transform;

            CreateBox("Floor", new Vector3(0f, -0.25f, 0f), new Vector3(40f, 0.5f, 40f),
                new Color(0.45f, 0.45f, 0.48f), root.transform);
            CreateBox("NorthWall", new Vector3(0f, 2f, 20f), new Vector3(40f, 4f, 0.5f),
                new Color(0.4f, 0.4f, 0.42f), root.transform);
            CreateBox("SouthWall", new Vector3(0f, 2f, -20f), new Vector3(40f, 4f, 0.5f),
                new Color(0.4f, 0.4f, 0.42f), root.transform);
            CreateBox("EastWall", new Vector3(20f, 2f, 0f), new Vector3(0.5f, 4f, 40f),
                new Color(0.4f, 0.4f, 0.42f), root.transform);
            CreateBox("WestWall", new Vector3(-20f, 2f, 0f), new Vector3(0.5f, 4f, 40f),
                new Color(0.4f, 0.4f, 0.42f), root.transform);

            // Cover blocks
            CreateBox("CoverA", new Vector3(-6f, 1f, 2f), new Vector3(3f, 2f, 1.2f),
                new Color(0.5f, 0.5f, 0.52f), root.transform);
            CreateBox("CoverB", new Vector3(5f, 0.75f, 6f), new Vector3(2f, 1.5f, 2f),
                new Color(0.48f, 0.48f, 0.5f), root.transform);
            CreateBox("RampBase", new Vector3(8f, 0.5f, -4f), new Vector3(4f, 1f, 4f),
                new Color(0.42f, 0.42f, 0.45f), root.transform);
            CreateBox("Hill", new Vector3(0f, 0.75f, 10f), new Vector3(6f, 1.5f, 6f),
                new Color(0.38f, 0.42f, 0.4f), root.transform);

            // Health packs (simple triggers)
            CreateHealthPack(new Vector3(-3f, 0.4f, -2f), root.transform);
            CreateHealthPack(new Vector3(10f, 1.4f, -4f), root.transform);

            SpawnDummy(new Vector3(0f, 1f, 8f), 150f, new Color(0.55f, 0.35f, 0.55f), root.transform);
            SpawnDummy(new Vector3(-8f, 1f, 4f), 125f, new Color(0.4f, 0.55f, 0.4f), root.transform);
            SpawnDummy(new Vector3(7f, 1f, 12f), 150f, new Color(0.55f, 0.4f, 0.35f), root.transform);
            SpawnDummy(new Vector3(3f, 2.5f, 10f), 150f, new Color(0.5f, 0.5f, 0.35f), root.transform);

            PlayerSpawn = new Vector3(0f, 1.1f, -8f);

            // Lighting
            if (Object.FindFirstObjectByType<Light>() == null)
            {
                var lightGo = new GameObject("Directional Light");
                lightGo.transform.SetParent(root.transform);
                lightGo.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
                var light = lightGo.AddComponent<Light>();
                light.type = LightType.Directional;
                light.intensity = 1.1f;
                light.color = new Color(1f, 0.98f, 0.94f);
            }

            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.35f, 0.36f, 0.38f);
        }

        static void CreateBox(string name, Vector3 pos, Vector3 scale, Color color, Transform parent)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = name;
            go.transform.SetParent(parent, false);
            go.transform.position = pos;
            go.transform.localScale = scale;
            var r = go.GetComponent<Renderer>();
            r.material = new Material(Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard"));
            r.material.color = color;
        }

        static void CreateHealthPack(Vector3 pos, Transform parent)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            go.name = "HealthPack";
            go.transform.SetParent(parent, false);
            go.transform.position = pos;
            go.transform.localScale = new Vector3(0.6f, 0.25f, 0.6f);
            Object.Destroy(go.GetComponent<Collider>());
            var col = go.AddComponent<SphereCollider>();
            col.isTrigger = true;
            col.radius = 0.8f;
            var r = go.GetComponent<Renderer>();
            r.material = new Material(Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard"));
            r.material.color = new Color(0.2f, 0.85f, 0.35f);
            go.AddComponent<HealthPackPickup>();
        }

        static void SpawnDummy(Vector3 pos, float hp, Color color, Transform parent)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            go.name = "DummyTarget";
            go.transform.SetParent(parent, false);
            go.transform.position = pos;
            go.transform.localScale = new Vector3(0.8f, 1f, 0.8f);
            var dummy = go.AddComponent<DummyTarget>();
            dummy.Initialize(hp, color);
        }
    }

    public class HealthPackPickup : MonoBehaviour
    {
        float _readyAt;
        Renderer _renderer;
        const float RespawnSeconds = 5f;

        void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (Time.time < _readyAt)
            {
                return;
            }

            var health = other.GetComponentInParent<Health>();
            if (health == null || !health.IsAlive)
            {
                return;
            }

            if (health.CurrentHealth >= health.MaxHealth)
            {
                return;
            }

            health.ApplyHeal(health.MaxHealth * 0.5f);
            _readyAt = Time.time + RespawnSeconds;
            if (_renderer != null)
            {
                _renderer.enabled = false;
            }

            Invoke(nameof(ShowAgain), RespawnSeconds);
        }

        void ShowAgain()
        {
            if (_renderer != null)
            {
                _renderer.enabled = true;
            }
        }
    }
}
