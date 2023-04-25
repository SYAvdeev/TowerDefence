using Domain.Entity;
using UnityEngine;

namespace Presentation.LevelObjects
{
    public class TowerPresenter : DamageablePresenter
    {
        [SerializeField] private SceneObjectsFactory sceneObjectsFactory;
        private float _sceneScale;
        private Tower Tower => (Tower)_damageable;
        
        public void Initialize(float sceneScale)
        {
            _sceneScale = sceneScale;
        }
        
        public override void SetObject(IDamageable damageable)
        {
            base.SetObject(damageable);
            Tower.DistantAttackState.OnDistantAttack += OnAttack;
        }

        private void OnAttack(float bulletCooldown)
        {
            Vector2 startPosition = new Vector2(Tower.ShootPosition.X, Tower.ShootPosition.Y);
            Vector2 endPosition = new Vector2(Tower.CurrentEnemy.Position.X, Tower.CurrentEnemy.Position.Y);
            sceneObjectsFactory.SpawnBullet(transform.parent, startPosition, endPosition, bulletCooldown, _sceneScale);
        }
    }
}