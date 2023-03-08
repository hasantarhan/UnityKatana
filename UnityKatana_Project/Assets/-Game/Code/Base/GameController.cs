using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Code;
using _Game.Code.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public enum GameState
{
    Boot,
    Ready,
    Game,
    Win,
    Fail
}

public class GameController : Singleton<GameController>
{
    public GameState gameState;
    public Action onBootGame;
    public Action onBootGameComplete;
    public Action onStartGame;
    public Action<bool> onFinishGame;
    private bool isEndGame;
    [SerializeField] private bool cheatingMode = true;

    public void Start()
    {
        DataManager.Init();
        BootGame();
    }

    public void BootGame()
    {
        gameState = GameState.Boot;
        onBootGame?.Invoke();
    }

    public void BootGameComplete()
    {
        onBootGameComplete?.Invoke();
        Debug.Log("BootGame completed " + Time.frameCount);
        gameState = GameState.Ready;
    }

    public void StartGame()
    {
        if (gameState == GameState.Ready)
        {
            onStartGame?.Invoke();
            gameState = GameState.Game;
        }
    }

    public void FinishGame(bool state)
    {
        if (isEndGame)
            return;

        if (gameState == GameState.Game)
        {
            gameState = state ? GameState.Win : GameState.Fail;
            isEndGame = true;
            onFinishGame?.Invoke(state);
        }
    }

    public void NextLevel()
    {
        DataManager.Player.Level++;
        DataManager.Player.Save();
        SceneManager.LoadScene(0);
    }

    public void Retry()
    {
        SceneManager.LoadScene(0);
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (!cheatingMode)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            FinishGame(true);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            FinishGame(false);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            NextLevel();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Retry();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Retry();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
#endif
    }
}