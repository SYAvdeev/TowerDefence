using System;
using System.Collections.Generic;
using System.Numerics;
using Domain.Entity.Config;
using Domain.Entity.State;

namespace Domain.Entity
{
    public class Tower : IDamageable
    {
        public event Action OnGetDamage;
        public event Action<IDamageable> OnDie;
        public event Action<int> OnMoneyChanged;

        public readonly DistantAttackState DistantAttackState;
        private readonly WaitForEnemiesState _waitForEnemiesState;

        private readonly UpgradableValue _damage;
        private readonly UpgradableValue _armor;

        private IUpdatable _currentState;

        private float _currentHealth;
        private int _money;
        private Enemy _currentEnemy;
        public Enemy CurrentEnemy => _currentEnemy;

        public Vector2 ShootPosition => _waitForEnemiesState.AttackPosition;
            
        public IUpdatable CurrentState => _currentState;
        public float CurrentHealth => _currentHealth;
        public float MaxHealth { get; }

        public UpgradableValue Armor => _armor;
        public UpgradableValue Damage => _damage;

        public Tower(TowerConfig config, List<Enemy> enemies)
        {
            _money = 0;
            MaxHealth = config.MaxHealth;
            _currentHealth = config.MaxHealth;

            _damage = new UpgradableValue(config.LevelDamage);
            _armor = new UpgradableValue(config.LevelArmor);
            
            DistantAttackState = new DistantAttackState();
            DistantAttackState.Initialize(null, config.AttackDelay, config.AttackRange, config.BulletSpeed, _damage.Current.Value);
            _damage.OnUpgrade += OnUpgradeDamage;
            _waitForEnemiesState = new WaitForEnemiesState(new Vector2(config.ShootPositionX, config.ShootPositionY), config.AttackRange, enemies);
            _waitForEnemiesState.OnEnemySpotted += ToAttackState;
            _currentState = _waitForEnemiesState;
        }

        public void ToDefault()
        {
            _currentHealth = MaxHealth;
            _damage.ToDefault();
            _armor.ToDefault();
            _money = 0;
            OnMoneyChanged?.Invoke(_money);
            DistantAttackState.Clear();
            _currentState = _waitForEnemiesState;
        }

        public void GetDamage(float damage)
        {
            _currentHealth -= _armor.Current.Value * damage;
            OnGetDamage?.Invoke();
            
            if (_currentHealth <= 0f)
            {
                OnDie?.Invoke(this);
                OnDie = null;
                OnGetDamage = null;
            }
        }

        private void ToAttackState(Enemy enemy)
        {
            _currentState = DistantAttackState;
            _currentEnemy = enemy;
            DistantAttackState.SetDamageable(enemy);
            enemy.OnDie += ToWaitForEnemiesState;
        }
        
        private void ToWaitForEnemiesState(IDamageable damageable)
        {
            damageable.OnDie -= ToWaitForEnemiesState;
            ++_money;
            OnMoneyChanged?.Invoke(_money);
            DistantAttackState.Clear();
            _currentState = _waitForEnemiesState;
        }

        private void OnUpgradeDamage(float value)
        {
            DistantAttackState.SetDamage(value);
        }

        public bool CanUpgradeDamage => CanUpgrade(_damage.CanUpgrade, _damage.Current.Price);
        public bool CanUpgradeArmor => CanUpgrade(_armor.CanUpgrade, _armor.Current.Price);
        private bool CanUpgrade(bool canUpgrade, int price)
        {
            return canUpgrade && price <= _money;
        }

        public void UpgradeDamage() => Upgrade(_damage);
        public void UpgradeArmor() => Upgrade(_armor);
        private void Upgrade(UpgradableValue upgradableValue)
        {
            _money -= upgradableValue.Current.Price;
            OnMoneyChanged?.Invoke(_money);
            upgradableValue.Upgrade();
        }
    }
}