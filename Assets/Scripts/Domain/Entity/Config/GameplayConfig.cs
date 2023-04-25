using System;

namespace Domain.Entity.Config
{
    [Serializable]
    public class GameplayConfig
    {
        public int EnemiesMultiplierPerWave;
        public float SpawnDelay;
        public float SpawnY;
        public float SpawnXMin;
        public float SpawnXMax;
        public float MapYSize;
    }
}