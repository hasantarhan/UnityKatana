using System;
using _Game.Code.Base;
using TMPro;
using UnityEngine;

namespace _Game.Code.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class CoinText : MonoBehaviour
    {
        private TextMeshProUGUI text;
        private void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            CoinManager.Instance.userCoinUpdate += CoinUpdate;
        }

        private void CoinUpdate(int coin)
        {
            text.text = coin.ToString();
        }
    }
}