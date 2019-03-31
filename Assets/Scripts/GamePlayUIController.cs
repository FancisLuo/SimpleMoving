using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

/// <summary>
/// 游戏主界面管理
/// </summary>
public class GamePlayUIController : MonoBehaviour
{
    [SerializeField] private Text   m_ScoreText;
    [SerializeField] private Text   m_GameResultText;
    [SerializeField] private Button m_GameStartButton;
    [SerializeField] private Button m_GameRestartButton;

    private UnityAction<string>     m_PlayerOutAction;
    private UnityAction<string>     m_GameOverAction;

    // Start is called before the first frame update
    void Start()
    {
        ClearDisplay();

        ShowStartOrRestart(true);

        m_PlayerOutAction = new UnityAction<string>(HandlePlayerOut);
        m_GameOverAction = new UnityAction<string>(HandleGameOver);
        m_GameStartButton.onClick.AddListener(HandleGameStart);
        m_GameRestartButton.onClick.AddListener(HandleGameRestart);

        EventManager.StartListening(EventType.GAME_PLAYER_OUT, m_PlayerOutAction);
        EventManager.StartListening(EventType.GAME_OVER, m_GameOverAction);
    }

    // 当 MonoBehaviour 将被销毁时调用此函数
    private void OnDestroy()
    {
        EventManager.StopListening(EventType.GAME_PLAYER_OUT, m_PlayerOutAction);
        EventManager.StopListening(EventType.GAME_OVER, m_GameOverAction);

        m_GameStartButton.onClick.RemoveAllListeners();
        m_GameRestartButton.onClick.RemoveAllListeners();
    }

    private void HandlePlayerOut(string message)
    {
        m_GameResultText.text = "滚出边界！";
    }

    private void HandleGameOver(string message)
    {
        float result = MessageParserUtil.ParseMessage<float>(message);
        m_ScoreText.text = string.Format("{0:F2}m", result);
    }

    private void HandleGameStart()
    {
        EventManager.TriggerEvent(EventType.GAME_START);
        ClearDisplay();
        ShowStartOrRestart(false);
    }

    private void HandleGameRestart()
    {
        EventManager.TriggerEvent(EventType.GAME_RESTART);
        ClearDisplay();
        ShowStartOrRestart(false);
    }

    private void ClearDisplay()
    {
        m_ScoreText.text        = "";
        m_GameResultText.text   = "";
    }

    private void ShowStartOrRestart(bool isStart)
    {
        m_GameStartButton.gameObject.SetActive(isStart);
        m_GameRestartButton.gameObject.SetActive(!isStart);
    }
}
