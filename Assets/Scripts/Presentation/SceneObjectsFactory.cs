using System.Collections.Generic;
using Data.Config;
using Domain.Entity;
using Presentation.LevelObjects;
using UnityEngine;

namespace Presentation
{
    public class SceneObjectsFactory : MonoBehaviour
    {
        [SerializeField] private Transform _poolParent;
        [SerializeField] private MainConfig mainConfig;
        [SerializeField] private GameObject _bulletPresenterPrefab;

        private readonly Stack<EnemyPresenter> _meleeEnemiesPool = new();
        private readonly Stack<EnemyPresenter> _distantEnemiesPool = new();
        private readonly Stack<BulletPresenter> _bulletsPool = new();

        public EnemyPresenter SpawnEnemy(Enemy enemy, Transform parent, float positionScale)
        {
            Stack<EnemyPresenter> pool = enemy.IsDistant ? _distantEnemiesPool : _meleeEnemiesPool;
            GameObject enemyPrefab = enemy.IsDistant ? mainConfig.DistantEnemyPrefab : mainConfig.EnemyPrefab;
            
            EnemyPresenter enemyPresenter = GetFromStack(pool, enemyPrefab, parent);
            enemyPresenter.Initialize(positionScale);
            enemyPresenter.SetObject(enemy);

            return enemyPresenter;
        }
        
        public BulletPresenter SpawnBullet(Transform parent, Vector2 startPosition, Vector2 endPosition, float bulletCooldown, float positionScale)
        {
            BulletPresenter bulletPresenter = GetFromStack(_bulletsPool, _bulletPresenterPrefab, parent);  
            bulletPresenter.Initialize(positionScale);
            bulletPresenter.StartSimulation(bulletCooldown, startPosition, endPosition);
            return bulletPresenter;
        }

        private T GetFromStack<T>(Stack<T> pool, GameObject prefab, Transform parent) where T : MonoBehaviour, IPoolObject
        {
            if (!pool.TryPop(out T presenter))
            {
                presenter = Instantiate(prefab, parent).GetComponent<T>();
                presenter.AddToPool += AddToPool;
            }
            else
            {
                presenter.transform.SetParent(parent);
            }
            
            presenter.gameObject.SetActive(true);
            return presenter;
        }

        private void AddToPool(EnemyPresenter enemyPresenter)
        {
            enemyPresenter.transform.parent = _poolParent;
            if (enemyPresenter.Enemy.IsDistant)
            {
                _distantEnemiesPool.Push(enemyPresenter);
            }
            else
            {
                _meleeEnemiesPool.Push(enemyPresenter);
            }
        }
        
        private void AddToPool(BulletPresenter bulletPresenter)
        {
            bulletPresenter.transform.parent = _poolParent;
            _bulletsPool.Push(bulletPresenter);
        }

        private void AddToPool(IPoolObject poolObject)
        {
            switch (poolObject)
            {
                case EnemyPresenter enemyPresenter:
                    AddToPool(enemyPresenter);
                    break;
                case BulletPresenter bulletPresenter:
                    AddToPool(bulletPresenter);
                    break;
            }
        }
    }
}