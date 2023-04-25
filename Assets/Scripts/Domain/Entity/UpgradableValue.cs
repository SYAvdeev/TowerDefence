using System;
using Domain.Entity.Config;

namespace Domain.Entity
{
    public class UpgradableValue
    {
        public event Action<float> OnUpgrade;
        private readonly UpgradableValueConfig[] _configs;
        private int _currentLevel;
        private UpgradableValueConfig _current;
        public UpgradableValueConfig Current => _current;
        public bool CanUpgrade => _configs.Length - 1 > _currentLevel;

        public UpgradableValue(UpgradableValueConfig[] configs)
        {
            _configs = configs;
            _currentLevel = 0;
            _current = configs[0];
        }
        
        public void Upgrade()
        {
            if (CanUpgrade)
            {
                ++_currentLevel;
                _current = _configs[_currentLevel];
                OnUpgrade?.Invoke(_current.Value);
            }
        }

        public void ToDefault()
        {
            _currentLevel = 0;
            _current = _configs[_currentLevel];
            OnUpgrade?.Invoke(_current.Value);
        }
    }
}