using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    private Dictionary<EventType, CustomEvent> m_EventMap;

    private static EventManager m_Instance;

    public static EventManager Instance
    {
        get
        {
            if (null == m_Instance)
            {
                GameObject go = GameObject.Find(CommonDefinition.EventManagerObjectName);
                if(null != go)
                {
                    m_Instance = go.GetComponent<EventManager>();
                    if (null == m_Instance)
                    {
                        m_Instance = go.AddComponent<EventManager>();
                    }
                    DontDestroyOnLoad(go);
                }

                if (null != m_Instance)
                {
                    m_Instance.Init();
                }
                else
                {
                    Debug.LogError("cannot finde Event Manager");
                }
            }

            return m_Instance;
        }
    }

    private void Init()
    {
        if (null == m_EventMap)
        {
            m_EventMap = new Dictionary<EventType, CustomEvent>();
        }
    }

    public static void StartListening(EventType eventType, UnityAction<string> listener)
    {
        CustomEvent thisEvent = null;
        if (Instance.m_EventMap.TryGetValue(eventType, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new CustomEvent();
            thisEvent.AddListener(listener);
            Instance.m_EventMap.Add(eventType, thisEvent);
        }
    }

    public static void StopListening(EventType eventType, UnityAction<string> listener)
    {
        if (m_Instance == null)
        {
            return;
        }

        CustomEvent thisEvent = null;
        if (Instance.m_EventMap.TryGetValue(eventType, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(EventType eventType, string message = null)
    {
        CustomEvent thisEvent = null;
        if (Instance.m_EventMap.TryGetValue(eventType, out thisEvent))
        {
            thisEvent.Invoke(message);
        }
    }
}
