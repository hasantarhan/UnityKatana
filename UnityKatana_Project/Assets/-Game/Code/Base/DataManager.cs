using System;
using UnityEngine;

namespace _Game.Code
{
    public static class DataManager
    {
        public static PlayerData Player;
        public static void Init()
        {
            Player = new PlayerData();
        }
    }

    [Serializable]
    public class PlayerData
    {
        public int EarningLevel
        {
            get
            {
                return PlayerPrefs.GetInt("EarningLevel", 0);
            }
            set
            {
                PlayerPrefs.SetInt("EarningLevel",value);
            }
        }
        public int SpeedLevel
        {
            get
            {
                return PlayerPrefs.GetInt("SpeedLevel", 0);
            }
            set
            {
                PlayerPrefs.SetInt("SpeedLevel",value);
            }
        }

        public int Level
        {
            get
            {
               return PlayerPrefs.GetInt("Level", 0);
            }
            set
            {
                 PlayerPrefs.SetInt("Level", value);
            }
        }
        public int Coin
        {
            get
            {
                return PlayerPrefs.GetInt("Coin", 500);
            }
            set
            {
                PlayerPrefs.SetInt("Coin", value);
            }
        }
        public void Save()
        {
            PlayerPrefs.Save();
        }
    }
}