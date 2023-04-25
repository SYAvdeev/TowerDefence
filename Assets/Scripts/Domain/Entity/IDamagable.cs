using System;

namespace Domain.Entity
{
    public interface IDamageable
    {
        void GetDamage(float damage);
        event Action OnGetDamage;
        event Action<IDamageable> OnDie;
        float CurrentHealth { get; }
        float MaxHealth  { get; }
    }
}