using System;
using Domain.Entity;
using UnityEngine;

namespace Presentation.LevelObjects
{
    public abstract class DamageablePresenter : MonoBehaviour, IPoolObject
    {
        public event Action<IPoolObject> AddToPool;
        [SerializeField] private Transform _health;
        protected IDamageable _damageable;

        public virtual void SetObject(IDamageable damageable)
        {
            _damageable = damageable;
            damageable.OnGetDamage += OnHealthChanged;
            damageable.OnDie += OnDie;
            OnHealthChanged();
        }

        private void OnHealthChanged()
        {
            _health.localScale = new Vector3(Mathf.Clamp01(_damageable.CurrentHealth / _damageable.MaxHealth), _health.localScale.y);
        }

        protected virtual void OnDie(IDamageable damageable)
        {
            AddToPool?.Invoke(this);
        }
    }
}