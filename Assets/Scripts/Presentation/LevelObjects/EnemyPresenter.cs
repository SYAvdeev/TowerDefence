using System.Collections;
using Domain.Entity;
using UnityEngine;

namespace Presentation.LevelObjects
{
    public class EnemyPresenter : DamageablePresenter
    {
        private const float DieDelay = 0.5f;
        
        private static int _enemyRunAnimatorHash = Animator.StringToHash("EnemyRun");
        private static int _enemyDieAnimatorHash = Animator.StringToHash("EnemyDie");
        private static int _enemyAttackAnimatorHash = Animator.StringToHash("EnemyAttack");
        
        [SerializeField] private Animator _animator;
        private float _positionScale;
        
        public Enemy Enemy => _damageable as Enemy;

        public void Initialize(float positionScale)
        {
            _positionScale = positionScale;
        }

        public override void SetObject(IDamageable damageable)
        {
            base.SetObject(damageable);
            Enemy.MoveState.OnPositionChanged += OnPositionUpdate;
            _animator.Play(_enemyRunAnimatorHash);
            OnPositionUpdate(Enemy.Position.X, Enemy.Position.Y);
            Enemy.AttackState.OnAttack += OnAttack;
            Enemy.OnDie += OnDie;
        }

        private void OnPositionUpdate(float x, float y)
        {
            transform.localPosition = _positionScale * new Vector3(x, y);
        }

        private void OnAttack()
        {
            _animator.Play(_enemyAttackAnimatorHash);
        }

        protected override void OnDie(IDamageable damageable)
        {
            _animator.Play(_enemyDieAnimatorHash);
            StartCoroutine(DieRoutine());
        }

        private IEnumerator DieRoutine()
        {
            float time = DieDelay;
            while (time > 0f)
            {
                time -= Time.deltaTime;
                yield return null;
            }
            gameObject.SetActive(false);
            base.OnDie(_damageable);
        }
    }
}