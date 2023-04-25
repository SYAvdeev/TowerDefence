using System.Globalization;
using Domain.Entity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UpgradeMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _moneyAmount;
        [SerializeField] private TextMeshProUGUI _armorAmount;
        [SerializeField] private TextMeshProUGUI _damageAmount;
        [SerializeField] private TextMeshProUGUI _upgradeArmorPrice;
        [SerializeField] private TextMeshProUGUI  _upgradeDamagePrice;
        [SerializeField] private Button _upgradeArmorButton;
        [SerializeField] private Button _upgradeDamageButton;

        private Tower _tower;

        public void Initialize(Tower tower)
        {
            _tower = tower;
            _tower.Armor.OnUpgrade += OnUpgradeArmor;
            _tower.Damage.OnUpgrade += OnUpgradeDamage;
            _tower.OnMoneyChanged += OnMoneyChanged;
            
            _armorAmount.text = "Armor: " + _tower.Armor.Current.Value.ToString(CultureInfo.InvariantCulture);
            _upgradeArmorPrice.text = "Upgrade: " + _tower.Armor.Current.Price;
            _damageAmount.text = "Damage: " + _tower.Damage.Current.Value.ToString(CultureInfo.InvariantCulture);
            _upgradeDamagePrice.text = "Upgrade: " + _tower.Damage.Current.Price;
        }
        
        private void OnMoneyChanged(int value)
        {
            _moneyAmount.text = "Money: " + value;
        }

        private void OnUpgradeArmor(float value)
        {
            _armorAmount.text = "Armor: " + value.ToString(CultureInfo.InvariantCulture);

            if (_tower.Armor.CanUpgrade)
            {
                _upgradeArmorButton.interactable = true;
                _upgradeArmorPrice.text = "Upgrade: " + _tower.Armor.Current.Price;
            }
            else
            {
                _upgradeArmorButton.interactable = false;
                _upgradeArmorPrice.text = "Upgrade";
            }
        }
        
        private void OnUpgradeDamage(float value)
        {
            _damageAmount.text = "Damage: " + value.ToString(CultureInfo.InvariantCulture);
            
            if (_tower.Damage.CanUpgrade)
            {
                _upgradeDamageButton.interactable = true;
                _upgradeDamagePrice.text = "Upgrade: " + _tower.Damage.Current.Price;
            }
            else
            {
                _upgradeDamageButton.interactable = false;
                _upgradeDamagePrice.text = "Upgrade";
            }
        }

        public void UpgradeArmorClick()
        {
            if (_tower.CanUpgradeArmor)
            {
                _tower.UpgradeArmor();
            }
        }
        
        public void UpgradeDamageClick()
        {
            if (_tower.CanUpgradeDamage)
            {
                _tower.UpgradeDamage();
            }
        }
    }
}
