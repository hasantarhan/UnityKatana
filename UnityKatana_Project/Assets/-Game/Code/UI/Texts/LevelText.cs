using System;
using TMPro;
using UnityEngine;

namespace _Game.Code.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LevelText : MonoBehaviour
    {
        private TextMeshProUGUI text;
        private string header = "LEVEL ";
        private void Start()
        {
            text = GetComponent<TextMeshProUGUI>();
            text.text = header + (DataManager.Player.Level + 1);
        }
    }
}