using System;
using Domain.Entity.Config;

namespace Domain.Entity.State
{
    public class AttackState : IUpdatable
    {
        public event Action OnAttack;

        protected IDamageable _damageable;
        private float _attackDelay;
        protected float _damage;

        protected float _currentAttackDelay;

        protected void Initialize(IDamageable damageable, float attackDelay, float damage)
        {
            _damageable = damageable;
            _attackDelay = attackDelay;
            _currentAttackDelay = attackDelay;
            _damage = damage;
            OnAttack = null;
        }
        
        public virtual void Initialize(IDamageable damageable, EnemyConfig enemyConfig, int level)
        {
            Initialize(damageable, enemyConfig.AttackDelay, enemyConfig.LevelDamage[level]);
        }

        public virtual void SetDamageable(IDamageable damageable)
        {
            _damageable = damageable;
        }

        public void SetDamage(float damage)
        {
            _damage = damage;
        }

        public virtual void Update(float deltaTime)
        {
            _currentAttackDelay -= deltaTime;

            if (_currentAttackDelay <= 0f)
            {
                _damageable.GetDamage(_damage);
                AttackInternal();
            }
        }

        protected void AttackInternal()
        {
            _currentAttackDelay = _attackDelay;
            OnAttack?.Invoke();
        }
    }
}