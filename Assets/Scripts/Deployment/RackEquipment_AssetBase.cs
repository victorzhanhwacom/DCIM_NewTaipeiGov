namespace VzDev.DCIM.Deployment
{
    /// <summary>
    /// 設備資產資料 - 基底，供機櫃內所有設備類型繼承
    /// </summary>
    public abstract class RackEquipment_AssetBase
    {
        public AssetInfo assetInfo;
        public COBieInfo cobieInfo;
        public EquipmentInfo equipmentInfo;
        public ModelInfo modelInfo;
    }
}

