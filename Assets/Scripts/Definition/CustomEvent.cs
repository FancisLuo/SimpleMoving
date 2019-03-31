using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 自定义事件
/// 以string类型作为参数，在使用时解析对应的需要的参数
/// 暂时以<seealso cref="MessageParserUtil.ParseMessage{T}(string, bool)"/>方法来解析
/// </summary>
public class CustomEvent : UnityEvent<string>
{
    public CustomEvent(): base() { }
}
