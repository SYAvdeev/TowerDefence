using System;

namespace Domain.Entity.Config
{
    [Serializable]
    public class TowerConfig
    {
        public float MaxHealth;
        public float AttackDelay;
        public float AttackRange;
        public float BulletSpeed;
        public UpgradableValueConfig[] LevelArmor;
        public UpgradableValueConfig[] LevelDamage;
        public float ShootPositionX;
        public float ShootPositionY;
    }
}