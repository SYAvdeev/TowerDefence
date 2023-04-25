using System;
using System.Numerics;
using Domain.Entity.Config;

namespace Domain.Entity.State
{
    public class MoveState : IUpdatable
    {
        public event Action<float, float> OnPositionChanged;
        
        private readonly Enemy _enemy;
        private float _speed;
        private float _targetYCoord;
        
        public MoveState(Enemy enemy)
        {
            _enemy = enemy;
        }
        
        public void Initialize(EnemyConfig enemyConfig)
        {
            _speed = enemyConfig.MoveSpeed;
            _targetYCoord = enemyConfig.AttackY;
            OnPositionChanged = null;
        }

        public void Update(float deltaTime)
        {
            _enemy.Position += new Vector2(0f, _speed * deltaTime);

            if (_enemy.Position.Y >= _targetYCoord)
            {
                _enemy.Position.Y = _targetYCoord;
                _enemy.ToAttackState();
            }

            OnPositionChanged?.Invoke(_enemy.Position.X, _enemy.Position.Y);
        }
    }
}