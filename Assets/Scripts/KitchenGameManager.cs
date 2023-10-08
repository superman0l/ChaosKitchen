using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KitchenGameManager : MonoBehaviour
{
    public static KitchenGameManager Instance { get; private set; }

    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }

    private State state;
    private float waitingToStartTimer = 1f;
    private float countdownToStartTimer = 3f;
    private float gamePlayingTimer = 30f;
    private float countingTimer = 0f;
    private bool isGamePaused = false;

    public event EventHandler OnGameStateChanged;//状态变化时直接调用事件 因为该单例已经开放状态接口所以不需要传入状态
    public event EventHandler<GamePauseEventArgs> GamePause;
    public class GamePauseEventArgs : EventArgs
    {
        public bool isPaused;
    }

    private void Awake()
    {
        Instance = this;
        state = State.WaitingToStart;
    }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    private void Update()
    {
        countingTimer += Time.deltaTime;
        switch (state)
        {
            case State.WaitingToStart:
                if (countingTimer > waitingToStartTimer)
                {
                    countingTimer = 0f;
                    state = State.CountdownToStart;
                    OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.CountdownToStart:
                if (countingTimer > countdownToStartTimer)
                {
                    countingTimer = 0f;
                    state = State.GamePlaying;
                    OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                if (countingTimer > gamePlayingTimer)
                {
                    countingTimer = 0f;
                    state = State.GameOver;
                    OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
        }
    }

    public bool isGamePlaying()
    {
        return state == State.GamePlaying;
    }

    public bool isCountdownToStart()
    {
        return state == State.CountdownToStart;
    }

    public string getCountdownTime()//用于UI获取倒数秒数 返回字符串便于调用
    {
        if (isCountdownToStart())
        {
            return ((int)countdownToStartTimer - Math.Floor(countingTimer)).ToString();
        }
        else return "";
    }

    public bool isGameOver()
    {
        return state == State.GameOver;
    }

    public float getGamePlayClock()
    {
        if (isGamePlaying())
        {
            return countingTimer / gamePlayingTimer;
        }
        else return 0;
    }

    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            Time.timeScale = 0f;
            GamePause?.Invoke(this, new GamePauseEventArgs
            {
                isPaused = true
            });
        }
        else
        {
            Time.timeScale = 1f;
            GamePause?.Invoke(this, new GamePauseEventArgs
            {
                isPaused = false
            });
        }
    }
}
