using System;
using System.Collections.Generic;
using System.Numerics;

namespace Domain.Entity.State
{
    public class WaitForEnemiesState : IUpdatable
    {
        public event Action<Enemy> OnEnemySpotted;
        private readonly Vector2 _attackPosition;
        private readonly float _sqrAttackRange;
        private readonly List<Enemy> _enemies;
        
        public Vector2 AttackPosition => _attackPosition;
        
        public WaitForEnemiesState(Vector2 attackPosition, float attackRange, List<Enemy> enemies)
        {
            _attackPosition = attackPosition;
            _sqrAttackRange = attackRange * attackRange;
            _enemies = enemies;
        }

        public void Update(float deltaTime)
        {
            for (int i = 0; i < _enemies.Count; i++)
            {
                if ((_attackPosition - _enemies[i].Position).LengthSquared() <= _sqrAttackRange)
                {
                    OnEnemySpotted?.Invoke(_enemies[i]);
                    break;
                }
            }
        }
    }
}