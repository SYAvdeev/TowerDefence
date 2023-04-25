using System;
using System.Collections;
using UnityEngine;

namespace Presentation.LevelObjects
{
    public class BulletPresenter : MonoBehaviour, IPoolObject
    {
        public event Action<IPoolObject> AddToPool;
        private float _positionScale;

        private float _currentLifetime;
        private float _lifetime;
        private Vector2 _startPosition;
        private Vector2 _finalPosition;

        private Coroutine _simulationRoutine;
        
        public void Initialize(float positionScale)
        {
            _positionScale = positionScale;
        }

        public void StartSimulation(float lifetime, Vector2 startPosition, Vector2 finalPosition)
        {
            _lifetime = lifetime;
            _currentLifetime = lifetime;
            _startPosition = startPosition;
            _finalPosition = finalPosition;

            if (_simulationRoutine != null)
            {
                StopCoroutine(_simulationRoutine);
            }
            _simulationRoutine = StartCoroutine(SimulationRoutine());
        }

        private IEnumerator SimulationRoutine()
        {
            while (_currentLifetime > 0f)
            {
                transform.localPosition = _positionScale * Vector3.Lerp(_startPosition, _finalPosition, 1f - _currentLifetime / _lifetime);
                _currentLifetime -= Time.deltaTime;
                yield return null;
            }

            gameObject.SetActive(false);
            AddToPool?.Invoke(this);
        }
    }
}