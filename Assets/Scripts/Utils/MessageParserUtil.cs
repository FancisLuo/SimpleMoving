using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MessageParserUtil
{
   public static T ParseMessage<T>(string message, bool isJson = false)
    {
        T result = default(T);

        if(!isJson)
        {
            result = (T)Convert.ChangeType(message, typeof(T));
        }
        else
        {
            result = JsonUtility.FromJson<T>(message);
        }

        return result;
    }
}
