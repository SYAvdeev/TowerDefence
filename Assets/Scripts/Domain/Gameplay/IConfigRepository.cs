using Domain.Entity.Config;

namespace Domain.Gameplay
{
    public interface IConfigRepository
    {
        public GameplayConfig GameplayConfig { get; }
        public TowerConfig TowerConfig { get; }
        public EnemyConfig EnemyConfig { get; }
        public DistantEnemyConfig DistantEnemyConfig { get; }
    }
}