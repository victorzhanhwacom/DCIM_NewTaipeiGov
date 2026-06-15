namespace VzDev.DCIM.Deployment
{
    /// <summary>
    /// 資產資料 (機櫃/設備資產類)
    /// </summary>
    public class AssetInfo
    {
        /// <summary>
        /// 資產名稱
        /// </summary>
        public string assetName;
        /// <summary>
        /// 資產編號
        /// </summary>
        public string assetNo;
        /// <summary>
        /// 資產類別
        /// </summary>
        public string category;
    }


    public struct COBieInfo
    {
        ///未定欄位數量
    }


    /// <summary>
    /// 機櫃專屬資料
    /// </summary>
    public struct RackInfo
    {
        public int power_watt_Max;
        public float weight_kg_Max;
        public int u_height_Max;
    }

    /// <summary>
    /// 資產設備專屬資料
    /// </summary>
    public struct EquipmentInfo
    {
        public int power_watt;
        public float weight_kg;
        public int u_height;
    }
}