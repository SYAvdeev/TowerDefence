using System;
using System.Collections.Generic;
using Domain.Entity.Config;

namespace Domain.Entity.State
{
    public class DistantAttackState : AttackState
    {
        public event Action<float> OnDistantAttack;
        private float _bulletCooldown;
        private readonly List<float> _currentShots = new (4);

        public override void Initialize(IDamageable damageable, EnemyConfig enemyConfig, int level)
        {
            base.Initialize(damageable, enemyConfig, level);
            DistantEnemyConfig distantEnemyConfig = (DistantEnemyConfig)enemyConfig;
            _bulletCooldown = distantEnemyConfig.AttackRange / distantEnemyConfig.BulletSpeed;
            Clear();
            OnDistantAttack = null;
        }

        public void Initialize(IDamageable damageable, float attackDelay, float attackRange, float bulletSpeed, float damage)
        {
            base.Initialize(damageable, attackDelay, damage);
            _bulletCooldown = attackRange / bulletSpeed;
            Clear();
        }

        public override void Update(float deltaTime)
        {
            _currentAttackDelay -= deltaTime;

            if (_currentAttackDelay <= 0f)
            {
                _currentShots.Add(_bulletCooldown);
                AttackInternal();
                OnDistantAttack?.Invoke(_bulletCooldown);
            }

            for (int i = _currentShots.Count - 1; i >= 0; i--)
            {
                _currentShots[i] -= deltaTime;
                if (_currentShots[i] <= 0f)
                {
                    _currentShots.RemoveAt(i);
                    _damageable.GetDamage(_damage);
                }
            }
        }

        public override void SetDamageable(IDamageable damageable)
        {
            base.SetDamageable(damageable);
            Clear();
        }

        public void Clear()
        {
            _currentShots.Clear();
        }
    }
}