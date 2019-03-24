using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMoving : MonoBehaviour, IMoving
{
    private Rigidbody m_Rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();

        if(null == m_Rigidbody)
        {
            m_Rigidbody = this.gameObject.AddComponent<Rigidbody>();
        }
    }

    private void AddForceToObject(float force)
    {
        m_Rigidbody.AddForceAtPosition(new Vector3(0, 0, force), transform.position, ForceMode.Force);
    }

    public void DoMove(float force)
    {
        Debug.LogFormat("Do Move force = {0}", force);
        AddForceToObject(force/5000);
    }
}
