using System;
using System.Numerics;
using Domain.Entity;
using Domain.Entity.Config;
using Domain.Entity.State;

namespace Domain.Gameplay
{
    public class EnemiesSpawner : IUpdatable
    {
        public event Action<Enemy> OnEnemySpawned; 

        private readonly EnemyFactory _enemyFactory;
        private readonly IConfigRepository _configRepository;
        private readonly Tower _tower;

        private int _currentSpawnCount;
        private float _time;
        private int _waveNumber;

        private readonly Random _random = new();
        
        public EnemiesSpawner(EnemyFactory enemyFactory, IConfigRepository configRepository, Tower tower)
        {
            _enemyFactory = enemyFactory;
            _configRepository = configRepository;
            _tower = tower;
        }

        public void StartSpawn(int waveNumber)
        {
            _waveNumber = waveNumber;
            _currentSpawnCount = (waveNumber + 1) * _configRepository.GameplayConfig.EnemiesMultiplierPerWave;
            _time = _configRepository.GameplayConfig.SpawnDelay;
        }
        
        public void Update(float deltaTime)
        {
            if (_currentSpawnCount > 0)
            {
                _time -= deltaTime;
                if (_time <= 0f)
                {
                    --_currentSpawnCount;

                    EnemyConfig enemyConfig = _random.NextDouble() >= 0.5 ? _configRepository.EnemyConfig : _configRepository.DistantEnemyConfig;

                    GameplayConfig gameplayConfig = _configRepository.GameplayConfig;

                    float xCoord = (float)_random.NextDouble() * (gameplayConfig.SpawnXMax - gameplayConfig.SpawnXMin) +
                                     gameplayConfig.SpawnXMin;

                    Vector2 position = new Vector2(xCoord, gameplayConfig.SpawnY);
                    Enemy enemy = _enemyFactory.CreateEnemy(enemyConfig, _tower, position, _waveNumber);

                    _time = _configRepository.GameplayConfig.SpawnDelay;
                    
                    OnEnemySpawned?.Invoke(enemy);
                }
            }
        }
    }
}