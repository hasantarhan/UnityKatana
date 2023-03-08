using System;
using _Game.Code.Utils;
using UnityEngine;

namespace _Game.Code.Base
{
    public class CoinManager : Singleton<CoinManager>
    {
        public int userCoin;
        public int sessionCoin;
        public Action<int> userCoinUpdate;
        public Action<int> sessionCoinUpdate;
        private void OnEnable()
        {
            GameController.Instance.onBootGameComplete += Init;
        }

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            userCoin = DataManager.Player.Coin;
            sessionCoin = 0;
            userCoinUpdate?.Invoke(userCoin);
            sessionCoinUpdate?.Invoke(sessionCoin);
        }

        public bool Pay(int coin)
        {
            var compare = Compare(coin);
            if (compare)
            {
                userCoin -= coin;
                userCoinUpdate?.Invoke(userCoin);
                Save();
            }

            return compare;
        }

        public bool Compare(int coin)
        {
            return userCoin >= coin;
        }

        public void AddCoin(int coin)
        {
            sessionCoin += coin;
            sessionCoinUpdate?.Invoke(sessionCoin);
        }

        public void AddToUserCoin()
        {
            userCoin += sessionCoin;
            userCoinUpdate?.Invoke(sessionCoin);
            Save();
        }

        private void Save()
        {
            DataManager.Player.Coin = userCoin;
            DataManager.Player.Save();
        }
    }
}