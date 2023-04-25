using Data.Config;
using Domain.Gameplay;
using Presentation.LevelObjects;
using UI;
using UnityEngine;

namespace Presentation
{
    public class Installer : MonoBehaviour
    {
        [SerializeField] private GameplayPresenter _gameplayPresenter;
        [SerializeField] private TowerPresenter _towerPresenter;
        [SerializeField] private MainConfig _mainConfig;
        [SerializeField] private GameplayScaler _gameplayScaler;
        [SerializeField] private UpgradeMenu _upgradeMenu;
    
        private void Start()
        {
            Gameplay gameplay = new Gameplay(_gameplayPresenter, _mainConfig);
            float gameplayScale = _gameplayScaler.CalculateScale();
            _towerPresenter.Initialize(gameplayScale);
            _gameplayPresenter.Initialize(gameplay, gameplayScale);
            _upgradeMenu.Initialize(gameplay.Tower);
        }
    }
}