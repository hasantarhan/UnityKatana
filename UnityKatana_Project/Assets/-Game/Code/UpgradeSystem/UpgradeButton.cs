using System;
using _Game.Code.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Code.UI
{
    [RequireComponent(typeof(Button))]
    public class UpgradeButton : MonoBehaviour
    {
        private Button button;
        public UpgradeConfig upgradeConfig;
        public TextMeshProUGUI priceText;
        public TextMeshProUGUI levelText;
    
        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(Click);
        }

        private void OnEnable()
        {
            upgradeConfig.onValuesUpdated += ValueUpdate;
            GameController.Instance.onBootGameComplete += ConfigInit;
        }

        private void ConfigInit()
        {
            upgradeConfig.Init();
        }

        private void ValueUpdate()
        {
            
            if (upgradeConfig.level>=upgradeConfig.maxLevel-1)
            {
                priceText.text = "MAX";
                levelText.text = "MAX";
                return;
            }
            priceText.text = upgradeConfig.GetPrice().ToString();
            levelText.text = "LVL " + (upgradeConfig.level + 1);
        }

        private void Click()
        {
            if (upgradeConfig.level <= upgradeConfig.maxLevel - 1)
            {
                if (CoinManager.Instance.Pay(upgradeConfig.GetPrice()))
                {
                    upgradeConfig.Upgrade();
                }
            }
        }
    }
}