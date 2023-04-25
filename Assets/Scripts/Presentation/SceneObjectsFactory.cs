using System;
using System.Collections.Generic;
using Data.Config;
using Domain.Entity;
using Presentation.LevelObjects;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Presentation
{
    public class SceneObjectsFactory : MonoBehaviour
    {
        [SerializeField] private Transform _poolParent;
        [SerializeField] private MainConfig mainConfig;
        [SerializeField] private AssetReference _bulletPresenterPrefab;

        private readonly Stack<EnemyPresenter> _meleeEnemiesPool = new();
        private readonly Stack<EnemyPresenter> _distantEnemiesPool = new();
        private readonly Stack<BulletPresenter> _bulletsPool = new();

        public void SpawnEnemy(Enemy enemy, Transform parent, float positionScale)
        {
            Stack<EnemyPresenter> pool = enemy.IsDistant ? _distantEnemiesPool : _meleeEnemiesPool;
            AssetReference enemyPrefab = enemy.IsDistant ? mainConfig.DistantEnemyPrefab : mainConfig.EnemyPrefab;
            
            GetFromStack(pool, enemyPrefab, parent, InitializePresenter);
            
            void InitializePresenter(EnemyPresenter presenter)
            {
                presenter.Initialize(positionScale);
                presenter.SetObject(enemy);
            }
        }
        
        public void SpawnBullet(Transform parent, Vector2 startPosition, Vector2 endPosition, float bulletCooldown, float positionScale)
        {
            GetFromStack(_bulletsPool, _bulletPresenterPrefab, parent, InitializePresenter);

            void InitializePresenter(BulletPresenter bulletPresenter)
            {
                bulletPresenter.Initialize(positionScale);
                bulletPresenter.StartSimulation(bulletCooldown, startPosition, endPosition);
            }
        }

        private void GetFromStack<T>(Stack<T> pool, AssetReference prefab, Transform parent, Action<T> onLoaded) where T : MonoBehaviour, IPoolObject
        {
            if (!pool.TryPop(out T presenter))
            {
                AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(prefab, parent);
                handle.Completed += OnCompleted;

                void OnCompleted(AsyncOperationHandle<GameObject> h)
                {
                    if (h.Status == AsyncOperationStatus.Succeeded)
                    {
                        presenter = h.Result.GetComponent<T>();
                        presenter.AddToPool += AddToPool;
                        presenter.gameObject.SetActive(true);
                        onLoaded.Invoke(presenter);
                    }
                }
            }
            else
            {
                presenter.transform.SetParent(parent);
                presenter.gameObject.SetActive(true);
                onLoaded.Invoke(presenter);
            }
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