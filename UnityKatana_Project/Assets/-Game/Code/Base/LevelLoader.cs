using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using UnityEngine;

namespace _Game.Code.Base
{
    public class LevelLoader : MonoBehaviour
    {
        public string[] levels;
        public bool autoLevelLoad;
        public int loopLevel;
        public Level level;

        private void OnEnable()
        {
            GameController.Instance.onBootGame += Init;
        }

        public void Init()
        {
            var level = DataManager.Player.Level;
            LoadLevel(level);
        }

        public void LoadLevel(int levelNo)
        {
            if (autoLevelLoad)
            {
                var lvl = levelNo;
                var maxLevel = levels.Length - 1;
                if (levelNo > maxLevel)
                {
                    lvl = (levelNo) % (maxLevel - loopLevel + 1) + loopLevel;
                }

                var levelName = levels[lvl];
                Debug.Log(levelName);
                StartCoroutine(AsyncLoadLevel(Path.Combine("LevelPrefabs", levelName)));
            }
            else
            {
                GameController.Instance.BootGameComplete();
            }
        }

        private IEnumerator AsyncLoadLevel(string path)
        {
            var request = Resources.LoadAsync(path);
            yield return request;
            level = Instantiate((GameObject) request.asset).GetComponent<Level>();
            level.gameObject.SetActive(true);
            Resources.UnloadUnusedAssets();
            GameController.Instance.BootGameComplete();
        }
    }
}