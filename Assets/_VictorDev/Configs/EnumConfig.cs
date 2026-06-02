/// 各類Enum的設定
namespace VzDev.Configs
{
    #region XYZ座標
    /// X軸 - 靠左、置中、靠右
    public enum EnumAlignmentPivotX
    {
        Left,
        Center,
        Right
    }
    
    /// Y軸 - 靠上、置中、靠下
    public enum EnumAlignmentPivotY
    {
        Top,
        Center,
        Bottom
    }

    /// Z軸 - 靠前、置中、靠後
    public enum EnumAlignmentPivotZ
    {
        Front,
        Center,
        Back
    }
    #endregion
    
    #region 時間
    public enum EnumTimeFormat
    {
        時分秒_12小時制, 時分秒_24小時制, 西元年月日, 星期, 星期_縮寫, 完整年月日時分秒_12小時制, 完整年月日時分秒_24小時制
    }
    
    public enum EnumTime
    {
        時, 分, 秒
    }
    #endregion

    /// 搜尋類型
    public enum EnumSearchType
    {
        Include, Exclude
    }
    /// 設備資訊狀態 (Good / Warning / Overload / Missing)
    public enum EnumIndicatorStatus
    {
        Normal , Warning , Overload , MissingData
    }
}