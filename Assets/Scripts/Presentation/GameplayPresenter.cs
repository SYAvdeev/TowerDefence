using System;
using System.Collections;
using Domain.Entity;
using Domain.Gameplay;
using Presentation.LevelObjects;
using UnityEngine;

namespace Presentation
{
    public class GameplayPresenter : MonoBehaviour, IGameplayPresenter
    {
        public event Action OnLose;
        public event Action OnPause;
        [SerializeField] private TowerPresenter _towerPresenter;
        [SerializeField] private SceneObjectsFactory _sceneObjectsFactory;
        [SerializeField] private Transform _levelObjectsParent;

        private Gameplay _gameplay;
        private Coroutine _updateRoutine;

        private float _sceneScale;
        private bool _pause;

        private bool Pause
        {
            get => _pause;
            set
            {
                if (value && _updateRoutine != null)
                {
                    StopCoroutine(_updateRoutine);
                    _updateRoutine = null;
                    OnPause?.Invoke();
                }
                else
                {
                    _updateRoutine = StartCoroutine(UpdateRoutine());
                }

                _pause = value;
            }
        }

        public void Initialize(Gameplay gameplayUseCase, float sceneScale)
        {
            _gameplay = gameplayUseCase;
            _sceneScale = sceneScale;
        }

        public void StartGame()
        {
            _gameplay.StartGame();
        }

        public void ContinueGame()
        {
            Pause = false;
        }

        public void OnGameStarted(Tower tower)
        {
            _towerPresenter.SetObject(tower);
            
            if (_updateRoutine != null)
            {
                StopCoroutine(_updateRoutine);
                _updateRoutine = null;
            }
            
            Pause = false;
        }

        public void Lose()
        {
            Pause = true;
            OnLose?.Invoke();
        }

        public void OnEnemySpawned(Enemy enemy)
        {
            _sceneObjectsFactory.SpawnEnemy(enemy, _levelObjectsParent, _sceneScale);
        }

        private IEnumerator UpdateRoutine()
        {
            while (true)
            {
                _gameplay.Update(Time.deltaTime);
                yield return null;
            }
        }

        private void OnDestroy()
        {
            if (_updateRoutine != null)
            {
                StopCoroutine(_updateRoutine);
            }
        }
    }
}