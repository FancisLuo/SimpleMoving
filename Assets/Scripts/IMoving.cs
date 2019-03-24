using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IMoving : IEventSystemHandler
{
    void DoMove(float force);
}
