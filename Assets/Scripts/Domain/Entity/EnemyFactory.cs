using System.Collections.Generic;
using System.Numerics;
using Domain.Entity.Config;
using Domain.Entity.State;

namespace Domain.Entity
{
    public class EnemyFactory
    {
        private readonly Stack<Enemy> _meleeEnemiesPool = new ();
        private readonly Stack<Enemy> _distantEnemiesPool = new ();
        public Enemy CreateEnemy(EnemyConfig enemyConfig, IDamageable damageable, Vector2 position, int level)
        {
            bool isDistantEnemy = enemyConfig is DistantEnemyConfig;
            
            Stack<Enemy> pool = isDistantEnemy ? _distantEnemiesPool : _meleeEnemiesPool;

            if (!pool.TryPop(out Enemy enemy))
            {
                enemy = new Enemy();
                enemy.MoveState = new MoveState(enemy);
                enemy.AttackState = isDistantEnemy ? new DistantAttackState() : new AttackState();
            }
            enemy.Initialize(enemyConfig.MaxHealth);
            enemy.MoveState.Initialize(enemyConfig);
            enemy.AttackState.Initialize(damageable, enemyConfig, level);
            enemy.Position = position;
            
            return enemy;
        }

        public void PushToPool(Enemy enemy)
        {
            if (enemy.IsDistant)
            {
                _distantEnemiesPool.Push(enemy);
            }
            else
            {
                _meleeEnemiesPool.Push(enemy);
            }
        }
    }
}