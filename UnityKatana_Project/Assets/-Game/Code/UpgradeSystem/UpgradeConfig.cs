using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Game.Code.Base
{
    public enum UpgradeType
    {
        Earn,
        Speed
    }

    [CreateAssetMenu(fileName = "Upgrade Config", menuName = "Katana/Upgrade Config", order = 0)]
    public class UpgradeConfig : ScriptableObject
    {
        public UpgradeType upgradeType;
        public AnimationCurve valueCurve;
        public AnimationCurve costCurve;
        public List<UpgradeData> values;
        public int level;
        public int maxLevel;
        public Action onValuesUpdated;

        public void Init()
        {
            switch (upgradeType)
            {
                case UpgradeType.Earn:
                    level = DataManager.Player.EarningLevel;
                    break;
                case UpgradeType.Speed:
                    level = DataManager.Player.SpeedLevel;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            maxLevel = (int)valueCurve.keys.Last().time;

            UpgradeHelper.Fill(this);
            onValuesUpdated?.Invoke();
        }

        public void Upgrade()
        {
            switch (upgradeType)
            {
                case UpgradeType.Earn:
                    DataManager.Player.EarningLevel++;
                    break;
                case UpgradeType.Speed:
                    DataManager.Player.SpeedLevel++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            DataManager.Player.Save();
            Init();
        }

        public int GetPrice()
        {
            return values[level].price;
        }

        public float GetValue()
        {
            return values[level].value;
        }
    }

    [Serializable]
    public struct UpgradeData
    {
        public int price;
        public float value;
    }
}