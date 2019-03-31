using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class ObjectMoving : MonoBehaviour
{
    private Rigidbody           m_Rigidbody;
    private bool                m_ToMove;
    private float               m_InitialVelocity;
    private Vector3             m_VelocityOnZ;
    private float               m_Time;

    private UnityAction<string> m_PlayerOutAction;
    private UnityAction<string> m_GameRestartAction;
    private UnityAction<string> m_MoveAction;

    // Start is called before the first frame update
    private void Start()
    {
        Init();
        ResetData();
        InitCubePosition();
        RegisterEventHandlers();
    }

    private void FixedUpdate()
    {
        if(m_ToMove)
        {
            m_Time += Time.deltaTime;
            m_VelocityOnZ.z = CalculateVelocity();
            if (m_VelocityOnZ.z > 0)
            {
                m_Rigidbody.velocity = m_VelocityOnZ;
            }
            else
            {
                EventManager.TriggerEvent(EventType.GAME_OVER, CalculateDistance().ToString());

                m_ToMove            = false;
                m_InitialVelocity = 0f;
                m_Time =            0f;
            }
        }
    }

    private void OnDestroy()
    {
        UnregisterEventHandlers();
    }

    private float CalculateVelocity()
    {
        return (m_InitialVelocity - CommonDefinition.Acceleration * m_Time);
    }

    private float CalculateDistance()
    {
        return (m_InitialVelocity * m_Time - Mathf.Pow(CommonDefinition.Acceleration, 2) / 2);
    }

    private void Init()
    {
        m_Rigidbody = GetComponent<Rigidbody>();

        m_PlayerOutAction = new UnityAction<string>(HandlePlayerOut);
        m_GameRestartAction = new UnityAction<string>(HandleGameRestart);
        m_MoveAction = new UnityAction<string>(HandleMoveObject);
    }

    private void RegisterEventHandlers()
    {
        EventManager.StartListening(EventType.GAME_PLAYER_OUT, m_PlayerOutAction);
        EventManager.StartListening(EventType.GAME_RESTART, m_GameRestartAction);
        EventManager.StartListening(EventType.GAME_START_MOVE, m_MoveAction);
    }

    private void UnregisterEventHandlers()
    {
        EventManager.StopListening(EventType.GAME_PLAYER_OUT, m_PlayerOutAction);
        EventManager.StopListening(EventType.GAME_RESTART, m_GameRestartAction);
        EventManager.StopListening(EventType.GAME_START_MOVE, m_MoveAction);
    }

    private void InitCubePosition()
    {
        this.transform.position = new Vector3(0, 0.5f, 0);
    }

    private void HandleGameRestart(string message)
    {
        ResetData();
        InitCubePosition();
    }

    private void HandlePlayerOut(string message)
    {
        m_ToMove = false;
        m_Rigidbody.velocity = Vector3.zero;
    }

    private void HandleMoveObject(string message)
    {
        float force = MessageParserUtil.ParseMessage<float>(message);
        DoMove(force);
    }

    private void DoMove(float force)
    {
        Debug.LogFormat("Do Move force = {0}", force);
        if (force > 0)
        {
            m_InitialVelocity = force / 500;
            m_ToMove = true;
        }
    }

    private void ResetData()
    {
        m_ToMove            = false;
        m_InitialVelocity = 0f;
        m_VelocityOnZ       = Vector3.zero;
        m_Time              = 0f;
    }
}
