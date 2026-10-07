using System.Collections.Generic;

public static class RackDoorMap
{
    public static Dictionary<string, RackDoorDeviceCodeInfo> DeviceCodeMap => dictionary;

    /// <summary>
    /// 機櫃門禁裝置對應的 機櫃DeviceCode 資訊
    /// </summary>
    private static Dictionary<string, RackDoorDeviceCodeInfo> dictionary = new Dictionary<string, RackDoorDeviceCodeInfo>
    {
        #region 機櫃A排
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+28", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-A1-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-A1-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+29", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-A2-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-A2-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+30", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-A3-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-A3-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+33", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-A4-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-A4-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+34", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-A5-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-A5-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+36", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-A6-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-A6-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+37", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-A7-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-A7-F") },
        #endregion

        #region 機櫃B排
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+3",  new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-B1-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-B1-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+4",  new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-B2-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-B2-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+5",  new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-B3-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-B3-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+8",  new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-B4-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-B4-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+9",  new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-B5-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-B5-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+11", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-B6-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-B6-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+12", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-B7-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-B7-F") },
        #endregion

        #region 機櫃C排
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+40", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-C1-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-C1-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+41", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-C2-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-C2-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+42", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-C3-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-C3-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+45", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-C4-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-C4-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+46", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-C5-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-C5-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+48", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-C6-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-C6-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+49", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-C7-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-C7-F") },
        #endregion

        #region 機櫃D排
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+15", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-D1-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-D1-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+16", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-D2-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-D2-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+17", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-D3-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-D3-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+20", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-D4-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-D4-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+21", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-D5-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-D5-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+23", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-D6-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-D6-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+24", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-D7-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-D7-F") },
        #endregion

        #region 機櫃E排
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+27", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-E1-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-E1-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+31", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-E2-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-E2-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+32", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-E3-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-E3-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+50", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-E4-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-E4-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+38", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-E5-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-E5-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+43", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-E6-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-E6-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+44", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-E7-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-E7-F") },
        #endregion

        #region 機櫃F排
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+2",  new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-F1-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-F1-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+6",  new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-F2-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-F2-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+7",  new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-F3-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-F3-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+25", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-F4-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-F4-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+13", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-F5-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-F5-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+18", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-F6-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-F6-F") },
        #endregion

        #region 機櫃G排
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1100: 資訊機櫃-W800xH42Ux1100+94", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-G1-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-G1-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1100: 資訊機櫃-W800xH42Ux1100+14", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-G2-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-G2-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1100: 資訊機櫃-W800xH42Ux1100+47", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-G3-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-G3-F") },
        #endregion

        #region 機櫃H排
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1100: 資訊機櫃-W800xH42Ux1100+95", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-H1-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-H1-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1100: 資訊機櫃-W800xH42Ux1100+39", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-H2-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-H2-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1100: 資訊機櫃-W800xH42Ux1100+22", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-H3-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-H3-F") },
        #endregion

        #region 機櫃I排
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+96", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-I1-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-I1-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+1",  new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-I2-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-I2-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+35", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-I3-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-I3-F") },
        #endregion

        #region 機櫃J排
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+97", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-J1-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-J1-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+26", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-J2-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-J2-F") },
        { "TGL+TPE+IDC+08F+1+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+10", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-J3-B", "TGL+TPE+IDC+08F+1+WE+EL+Elock+EL-J3-F") },
        #endregion

        #region 機櫃K排
        { "TGL+TPE+IDC+08F+2+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+74", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+2+WE+EL+Elock+EL-K1-B", "TGL+TPE+IDC+08F+2+WE+EL+Elock+EL-K1-F") },
        { "TGL+TPE+IDC+08F+2+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+73", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+2+WE+EL+Elock+EL-K2-B", "TGL+TPE+IDC+08F+2+WE+EL+Elock+EL-K2-F") },
        { "TGL+TPE+IDC+08F+2+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+72", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+2+WE+EL+Elock+EL-K3-B", "TGL+TPE+IDC+08F+2+WE+EL+Elock+EL-K3-F") },
        { "TGL+TPE+IDC+08F+2+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+71", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+2+WE+EL+Elock+EL-K4-B", "TGL+TPE+IDC+08F+2+WE+EL+Elock+EL-K4-F") },
        { "TGL+TPE+IDC+08F+2+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+70", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+2+WE+EL+Elock+EL-K5-B", "TGL+TPE+IDC+08F+2+WE+EL+Elock+EL-K5-F") },
        #endregion

        #region 機櫃L排
        { "TGL+TPE+IDC+08F+2+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+65", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+2+WE+EL+Elock+EL-L1-B", "TGL+TPE+IDC+08F+2+WE+EL+Elock+EL-L1-F") },
        { "TGL+TPE+IDC+08F+2+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+66", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+2+WE+EL+Elock+EL-L2-B", "TGL+TPE+IDC+08F+2+WE+EL+Elock+EL-L2-F") },
        { "TGL+TPE+IDC+08F+2+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+67", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+2+WE+EL+Elock+EL-L3-B", "TGL+TPE+IDC+08F+2+WE+EL+Elock+EL-L3-F") },
        { "TGL+TPE+IDC+08F+2+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+68", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+2+WE+EL+Elock+EL-L4-B", "TGL+TPE+IDC+08F+2+WE+EL+Elock+EL-L4-F") },
        { "TGL+TPE+IDC+08F+2+DCR+Rack+資訊機櫃-W800xH42Ux1200: 資訊機櫃-W800xH42Ux1200+69", new RackDoorDeviceCodeInfo("TGL+TPE+IDC+08F+2+WE+EL+Elock+EL-L5-B", "TGL+TPE+IDC+08F+2+WE+EL+Elock+EL-L5-F") }
        #endregion
    };

    [System.Serializable]
    public class RackDoorDeviceCodeInfo
    {
        public string deviceCode_BackDoor, deviceCode_FrontDoor;
        public RackDoorDeviceCodeInfo(string backDoorDeviceCode, string frontDoorDeviceCode)
        {
            deviceCode_BackDoor = backDoorDeviceCode;
            deviceCode_FrontDoor = frontDoorDeviceCode;
        }
    }
}