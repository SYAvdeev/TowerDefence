using System.Collections.Generic;
using Domain.Entity;

namespace Domain.Gameplay
{
    public class Gameplay
    {
        private readonly IGameplayPresenter _gameplayPresenter;
        private readonly IConfigRepository _configRepository;
        
        private readonly EnemyFactory _enemyFactory;
        private readonly EnemiesSpawner _enemiesSpawner;
        private readonly Tower _tower;
        
        private readonly List<Enemy> _enemies;

        private int _currentWave;
        private int _waveEnemiesCount;

        public Tower Tower => _tower;

        public Gameplay(IGameplayPresenter gameplayPresenter, IConfigRepository configRepository)
        {
            _gameplayPresenter = gameplayPresenter;
            _configRepository = configRepository;

            _enemies = new();
            _tower = new Tower(_configRepository.TowerConfig, _enemies);
            _tower.OnDie += Lose;
            _enemyFactory = new EnemyFactory();
            _enemiesSpawner = new EnemiesSpawner(_enemyFactory, configRepository, _tower);
            _enemiesSpawner.OnEnemySpawned += OnEnemySpawned;
        }

        public void StartGame()
        {
            _currentWave = 0;
            _waveEnemiesCount = _configRepository.GameplayConfig.EnemiesMultiplierPerWave * (_currentWave + 1);
            _enemiesSpawner.StartSpawn(_currentWave);
            
            _gameplayPresenter.OnGameStarted(_tower);
        }

        private void NextWave()
        {
            ++_currentWave;
            _waveEnemiesCount = _configRepository.GameplayConfig.EnemiesMultiplierPerWave * (_currentWave + 1);
            _enemiesSpawner.StartSpawn(_currentWave);
        }

        public void Update(float deltaTime)
        {
            _enemiesSpawner.Update(deltaTime);
            _tower.CurrentState.Update(deltaTime);
            for (int i = 0; i < _enemies.Count; i++)
            {
                _enemies[i].CurrentState.Update(deltaTime);
            }
        }

        private void Lose(IDamageable damageable)
        {
            _gameplayPresenter.Lose();
            _tower.ToDefault();
            
            for (int i = _enemies.Count - 1; i >= 0; --i)
            {
                _enemies[i].Die();
            }

            _enemies.Clear();
        }

        private void OnEnemyDestroyed(IDamageable damageable)
        {
            Enemy enemy = (Enemy)damageable;
            _enemies.Remove(enemy);
            _enemyFactory.PushToPool(enemy);
            --_waveEnemiesCount;

            if (_waveEnemiesCount == 0)
            {
                NextWave();
            }
        }

        private void OnEnemySpawned(Enemy enemy)
        {
            _enemies.Add(enemy);
            _gameplayPresenter.OnEnemySpawned(enemy);
            enemy.OnDie += OnEnemyDestroyed;
        }
    }
}