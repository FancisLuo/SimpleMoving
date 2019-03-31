/// <summary>
/// 定义当前使用的事件类型
/// </summary>
public enum EventType : uint
{
    /// <summary>
    /// 无
    /// </summary>
    NONE                    = 0,

    /// <summary>
    /// 游戏开始事件
    /// </summary>
    GAME_START              = 1,

    /// <summary>
    /// 游戏结束后重新开始
    /// </summary>
    GAME_RESTART            = 2,

    /// <summary>
    /// 物体滚动开始事件
    /// </summary>
    GAME_START_MOVE         = 3,

    /// <summary>
    /// 游戏结束
    /// </summary>
    GAME_OVER               = 4,

    /// <summary>
    /// 物体滚出边界
    /// </summary>
    GAME_PLAYER_OUT         = 5,
}
