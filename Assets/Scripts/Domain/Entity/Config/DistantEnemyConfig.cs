using System;

namespace Domain.Entity.Config
{
    [Serializable]
    public class DistantEnemyConfig : EnemyConfig
    {
        public float AttackRange;
        public float BulletSpeed;
    }
}