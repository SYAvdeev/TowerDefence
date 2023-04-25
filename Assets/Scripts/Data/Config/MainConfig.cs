using Domain.Entity.Config;
using Domain.Gameplay;
using UnityEngine;

namespace Data.Config
{
    [CreateAssetMenu(fileName ="Config", menuName = "Assets/Config/Main Config", order = 0)]
    public class MainConfig : ScriptableObject, IConfigRepository
    {
        [SerializeField] private GameplayConfig _gameplayConfig;
        [SerializeField] private TowerConfig _towerConfig;
        [SerializeField] private EnemyConfig _enemyConfig;
        [SerializeField] private DistantEnemyConfig _distantEnemyConfig;
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private GameObject _distantEnemyPrefab;

        public GameplayConfig GameplayConfig => _gameplayConfig;
        public TowerConfig TowerConfig => _towerConfig;
        public EnemyConfig EnemyConfig => _enemyConfig;
        public DistantEnemyConfig DistantEnemyConfig => _distantEnemyConfig;
        public GameObject EnemyPrefab => _enemyPrefab;
        public GameObject DistantEnemyPrefab => _distantEnemyPrefab;
    }
}