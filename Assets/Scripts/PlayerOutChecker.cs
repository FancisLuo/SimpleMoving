using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOutChecker : MonoBehaviour
{

    // 如果另一个碰撞器进入了触发器，则调用 OnTriggerEnter
    private void OnTriggerEnter(Collider other)
    {
        Debug.LogFormat("collider name & tag = {0} - {1}", other.name, other.gameObject.tag);
        if(other.gameObject.CompareTag("Player"))
        {
            EventManager.TriggerEvent(EventType.GAME_PLAYER_OUT, string.Empty);
            EventManager.TriggerEvent(EventType.GAME_OVER, 0.ToString());
        }
    }
}
