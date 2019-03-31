using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GameObject m_Target;

    private Vector3                     m_StartPoint;
    private Vector3                     m_EndPoint;
    private float                       m_StartTime;
    private float                       m_EndTime;

    private float                       m_DistanceOnY;
    private float                       m_TimeDuration;
    private float                       m_Speed;

    private bool                        m_IsGameStart;
    private bool                        m_InputValid;

    private UnityAction<string>         m_GameStartAction;
    private UnityAction<string>         m_GameRestartAction;
    private UnityAction<string>         m_GameOverAction;

    // Start is called before the first frame update
    void Start()
    {
        InitActions();
        ResetData();
        RegisterHandlers();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_IsGameStart)
        {
            return;
        }


#if UNITY_ANDROID && !UNITY_EDITOR
        if(Input.touchCount > 0)
        {
            var input = Input.GetTouch(0);
            if(TouchPhase.Began != input.phase)
            {
                m_StartPoint    = Input.mousePosition;
                m_StartTime     = Time.deltaTime;

                Debug.LogFormat("start point = {0}, start time = {1}", m_StartPoint, m_StartTime);
            }
            else if(TouchPhase.Ended == input.phase)
            {
                m_EndPoint  = Input.mousePosition;
                m_EndTime   = Time.deltaTime;
                Debug.LogFormat("end point = {0}, end time = {1}", m_EndPoint, m_EndTime);
                HandleTouchEnd();
            }
        }
#elif UNITY_EDITOR

        if (Input.GetMouseButtonDown(0))
        {
            m_InputValid = CheckOnCube();
            // 起始点必须是在cube上
            if (!m_InputValid)
            {
                return;
            }

            m_StartPoint    = Input.mousePosition;
            m_StartTime     = Time.time;

            Debug.LogFormat("start point = {0}, start time = {1}", m_StartPoint, m_StartTime);
        }
        else if (Input.GetMouseButtonUp(0) && m_InputValid)
        {
            m_EndPoint  = Input.mousePosition;
            m_EndTime   = Time.time;

            Debug.LogFormat("end point = {0}, end time = {1}", m_EndPoint, m_EndTime);

            HandleTouchEnd();
            m_InputValid = false;
        }
#endif
    }

    // 当 MonoBehaviour 将被销毁时调用此函数
    private void OnDestroy()
    {
        UnRegisterHandlers();
    }

    private bool CheckOnCube()
    {
        bool checkResult = false;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100))
        {
            if(null != hit.collider.gameObject && hit.collider.gameObject.CompareTag(CommonDefinition.PlayerTag))
            {
                checkResult = true;
            }
        }

        return checkResult;
    }

    private void HandleTouchEnd()
    {
        m_DistanceOnY   = m_EndPoint.y - m_StartPoint.y;
        m_TimeDuration  = m_EndTime - m_StartTime;

        if(m_TimeDuration > 0)
        {
            m_Speed = m_DistanceOnY / m_TimeDuration;
            Debug.LogFormat("m_Speed = {0}", m_Speed);

            //ExecuteEvents.Execute<IMoving>(m_Target, null, (x, y) => x.DoMove(m_Speed));
            EventManager.TriggerEvent(EventType.GAME_START_MOVE, m_Speed.ToString());
        }
        else
        {
            Debug.Log("m_TimeDuration < 0");
        }
    }

    private void HandleGameStart(string message)
    {
        m_IsGameStart = true;
    }

    private void HandleGameRestart(string message)
    {
        m_IsGameStart = true;
    }

    private void HandleGameOver(string message)
    {
        ResetData();
    }

    private void InitActions()
    {
        m_GameStartAction = new UnityAction<string>(HandleGameStart);
        m_GameRestartAction = new UnityAction<string>(HandleGameRestart);
        m_GameOverAction = new UnityAction<string>(HandleGameOver);
    }

    private void RegisterHandlers()
    {
        EventManager.StartListening(EventType.GAME_START, m_GameStartAction);
        EventManager.StartListening(EventType.GAME_RESTART, m_GameRestartAction);
        EventManager.StartListening(EventType.GAME_OVER, m_GameOverAction);
    }

    private void UnRegisterHandlers()
    {
        EventManager.StopListening(EventType.GAME_START, m_GameStartAction);
        EventManager.StopListening(EventType.GAME_RESTART, m_GameRestartAction);
        EventManager.StopListening(EventType.GAME_OVER, m_GameOverAction);
    }

    private void ResetData()
    {
        m_StartPoint    = Vector3.zero;
        m_EndPoint      = Vector3.zero;
        m_StartTime     = 0f;
        m_EndTime       = 0f;

        m_DistanceOnY   = 0f;
        m_TimeDuration  = 0f;

        m_Speed         = 0f;

        m_IsGameStart   = false;
        m_InputValid    = false;
    }
}
