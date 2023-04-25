using System;
using System.Numerics;
using Domain.Entity.State;

namespace Domain.Entity
{
    public class Enemy : IDamageable
    {
        public event Action OnGetDamage;
        public event Action<IDamageable> OnDie;
        public Vector2 Position;

        private float _currentHealth;
        private float _maxHealth;

        public AttackState AttackState;
        public MoveState MoveState;
        
        private IUpdatable _currentState;

        public float CurrentHealth => _currentHealth;
        public float MaxHealth => _maxHealth;
        public IUpdatable CurrentState => _currentState;
        public bool IsDistant => AttackState is DistantAttackState;

        public void Initialize(float maxHealth)
        {
            _maxHealth = maxHealth;
            _currentHealth = maxHealth;
            _currentState = MoveState;
        }

        public void GetDamage(float damage)
        {
            _currentHealth -= damage;
            OnGetDamage?.Invoke();
            
            if (_currentHealth <= 0f)
            {
                Die();
            }
        }

        public void Die()
        {
            OnDie?.Invoke(this);
            OnDie = null;
        }

        public void ToAttackState()
        {
            _currentState = AttackState;
        }
    }
}