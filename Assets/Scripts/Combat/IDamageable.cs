using UnityEngine;

namespace Ironstrait.Combat
{
    public interface IDamageable
    {
        bool IsAlive { get; }
        float CurrentHealth { get; }
        float MaxHealth { get; }
        float ApplyDamage(float amount, GameObject source, Vector3 hitPoint, bool isSplash);
    }
}
