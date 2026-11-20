using System;
using Ironstrait.Config;
using UnityEngine;

namespace Ironstrait.Combat
{
    /// <summary>
    /// Sandbox score/level tracker using design-doc assistant thresholds.
    /// Full upgrade effects are mostly stubs; level is tracked and displayed.
    /// </summary>
    public class ScoreProgression : MonoBehaviour
    {
        public event Action<float, int> ScoreChanged;

        float _score;
        int _level = 1;

        public float Score => _score;
        public int Level => _level;

        public float ScoreToNext
        {
            get
            {
                var thresholds = GameConfig.Mercenary.score.levelThresholds;
                if (_level >= thresholds.Length)
                {
                    return 0f;
                }

                return Mathf.Max(0f, thresholds[_level] - _score);
            }
        }

        public void ResetProgress()
        {
            _score = 0f;
            _level = 1;
            ScoreChanged?.Invoke(_score, _level);
        }

        public void AddDamageScore(float actualHealthRemoved)
        {
            if (actualHealthRemoved <= 0f)
            {
                return;
            }

            var per = Mathf.Max(1f, GameConfig.Mercenary.score.damagePerPoint);
            AddScore(actualHealthRemoved / per);
        }

        public void AddScore(float amount)
        {
            if (amount <= 0f)
            {
                return;
            }

            _score = Mathf.Max(0f, _score + amount);
            RecalculateLevel();
            ScoreChanged?.Invoke(_score, _level);
        }

        public void SetLevel(int level)
        {
            var thresholds = GameConfig.Mercenary.score.levelThresholds;
            _level = Mathf.Clamp(level, 1, thresholds.Length);
            _score = thresholds[_level - 1];
            ScoreChanged?.Invoke(_score, _level);
        }

        void RecalculateLevel()
        {
            var thresholds = GameConfig.Mercenary.score.levelThresholds;
            var newLevel = 1;
            for (var i = thresholds.Length - 1; i >= 0; i--)
            {
                if (_score >= thresholds[i])
                {
                    newLevel = i + 1;
                    break;
                }
            }

            // Cap display level at 10 (index 9 => level 10). Surplus discarded for progression.
            _level = Mathf.Min(newLevel, 10);
            if (_score > thresholds[thresholds.Length - 1])
            {
                _score = thresholds[thresholds.Length - 1];
            }
        }

        // --- Upgrade stubs (wired later; values readable for HUD/debug) ---

        public float GrenadeSelfDamageMultiplier
        {
            get
            {
                var m = 1f;
                if (_level >= 3) m *= 0.85f;
                if (_level >= 10) m *= 0.5f;
                return m;
            }
        }

        public float ReloadTimeMultiplier => _level >= 5 ? 0.85f : 1f;

        public float GrenadeDirectDamageMultiplier => _level >= 7 ? 1.2f : 1f;

        public float ShotgunPelletDamageMultiplier => _level >= 8 ? 1.1f : 1f;

        public float GrenadeSplashRadiusMultiplier => _level >= 10 ? 2f : 1f;

        public int ShotgunPelletCount
        {
            get
            {
                var baseCount = GameConfig.Mercenary.shotgun.pelletCount;
                return _level >= 10 ? 8 : baseCount;
            }
        }
    }
}
