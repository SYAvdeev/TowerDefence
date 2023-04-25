using System;

namespace Domain.Entity.Config
{
    [Serializable]
    public class EnemyConfig
    {
        public float MoveSpeed;
        public float AttackDelay;
        public float AttackY;
        public float[] LevelDamage;
        public float MaxHealth;
    }
}