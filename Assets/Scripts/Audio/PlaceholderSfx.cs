using UnityEngine;

namespace Ironstrait.Audio
{
    /// <summary>
    /// Tiny procedural placeholder one-shots (no external packs / attribution needed).
    /// Generated at runtime so the sandbox stays editor-openable without import steps.
    /// </summary>
    public static class PlaceholderSfx
    {
        static AudioClip _shotgunFire;
        static AudioClip _grenadeFire;
        static AudioClip _reload;
        static AudioClip _hit;
        static AudioClip _jump;

        public static AudioClip ShotgunFire => _shotgunFire ??= BuildNoiseBurst(
            "sfx_shotgun_fire", 0.18f, 0.55f, decay: 18f, bandLow: 80f, bandHigh: 1800f);

        public static AudioClip GrenadeFire => _grenadeFire ??= BuildToneBurst(
            "sfx_grenade_fire", 0.16f, 0.45f, startHz: 180f, endHz: 70f, decay: 14f);

        public static AudioClip Reload => _reload ??= BuildToneBurst(
            "sfx_reload", 0.08f, 0.25f, startHz: 420f, endHz: 280f, decay: 22f);

        public static AudioClip Hit => _hit ??= BuildNoiseBurst(
            "sfx_hit", 0.05f, 0.35f, decay: 40f, bandLow: 400f, bandHigh: 4000f);

        public static AudioClip Jump => _jump ??= BuildToneBurst(
            "sfx_jump", 0.07f, 0.2f, startHz: 160f, endHz: 260f, decay: 28f);

        public static void Play(AudioSource source, AudioClip clip, float volume = 1f, float pitch = 1f)
        {
            if (source == null || clip == null)
            {
                return;
            }

            source.pitch = pitch;
            source.PlayOneShot(clip, volume);
        }

        static AudioClip BuildNoiseBurst(string name, float duration, float amplitude, float decay,
            float bandLow, float bandHigh)
        {
            const int sampleRate = 22050;
            var count = Mathf.CeilToInt(duration * sampleRate);
            var samples = new float[count];
            var rng = new System.Random(name.GetHashCode());
            var prev = 0f;
            for (var i = 0; i < count; i++)
            {
                var t = i / (float)sampleRate;
                var env = Mathf.Exp(-decay * t) * amplitude;
                var white = (float)(rng.NextDouble() * 2.0 - 1.0);
                var cutoff = Mathf.Clamp01((bandHigh - bandLow) / 8000f);
                prev = Mathf.Lerp(prev, white, 0.15f + cutoff * 0.5f);
                var tone = Mathf.Sin(2f * Mathf.PI * Mathf.Lerp(bandLow, bandHigh * 0.25f, t / duration) * t);
                samples[i] = (prev * 0.85f + tone * 0.15f) * env;
            }

            var clip = AudioClip.Create(name, count, 1, sampleRate, false);
            clip.SetData(samples, 0);
            return clip;
        }

        static AudioClip BuildToneBurst(string name, float duration, float amplitude, float startHz,
            float endHz, float decay)
        {
            const int sampleRate = 22050;
            var count = Mathf.CeilToInt(duration * sampleRate);
            var samples = new float[count];
            var phase = 0.0;
            for (var i = 0; i < count; i++)
            {
                var t = i / (float)sampleRate;
                var u = duration > 0f ? t / duration : 0f;
                var hz = Mathf.Lerp(startHz, endHz, u);
                phase += 2.0 * System.Math.PI * hz / sampleRate;
                var env = Mathf.Exp(-decay * t) * amplitude;
                var click = i < 40 ? (1f - i / 40f) * 0.2f * amplitude : 0f;
                samples[i] = (float)System.Math.Sin(phase) * env + click * ((i & 1) == 0 ? 1f : -1f);
            }

            var clip = AudioClip.Create(name, count, 1, sampleRate, false);
            clip.SetData(samples, 0);
            return clip;
        }
    }
}
