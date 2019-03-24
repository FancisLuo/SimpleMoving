using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GameObject m_Target;

    private Vector3     m_StartPoint;
    private Vector3     m_EndPoint;
    private float       m_StartTime;
    private float       m_EndTime;

    private float       m_DistanceOnY;
    private float       m_TimeDuration;
    private float       m_Speed;

    // Start is called before the first frame update
    void Start()
    {
        ResetData();
    }

    // Update is called once per frame
    void Update()
    {
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
            m_StartPoint = Input.mousePosition;
            m_StartTime = Time.deltaTime;

            Debug.LogFormat("start point = {0}, start time = {1}", m_StartPoint, m_StartTime);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            m_EndPoint = Input.mousePosition;
            m_EndTime = Time.deltaTime;

            Debug.LogFormat("end point = {0}, end time = {1}", m_EndPoint, m_EndTime);

            HandleTouchEnd();
        }
#endif
    }

    private void HandleTouchEnd()
    {
        m_DistanceOnY = m_EndPoint.y - m_StartPoint.y;
        m_TimeDuration = m_EndTime - m_StartTime;

        if(m_TimeDuration > 0)
        {
            m_Speed = m_DistanceOnY / m_TimeDuration;

            ExecuteEvents.Execute<IMoving>(m_Target, null, (x, y) => x.DoMove(m_Speed));
        }
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
    }
}
