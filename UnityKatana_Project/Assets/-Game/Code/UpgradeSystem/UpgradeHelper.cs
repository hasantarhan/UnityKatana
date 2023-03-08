using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Game.Code.Base
{
    public static class UpgradeHelper
    {
        public static void Fill(UpgradeConfig config)
        {
            config.values = new List<UpgradeData>();
            var maxTime = config.valueCurve.keys.Last().time;
            for (int i = 0; i < maxTime; i++)
            {
                var value = config.valueCurve.Evaluate(i);
                var price = (int)config.costCurve.Evaluate(i);
                price = price.RoundOff();
                var upgradeData = new UpgradeData
                {
                    value = value,
                    price = price
                };
                config.values.Add(upgradeData);
            }
        }
    }
}