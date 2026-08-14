using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar_RoomHandlerModData : BaseRoomHandlerModData
{
    #region EDITOR EXPOSED FIELDS
    [Header("RoomItem Locators")]
    public Basic_RoomItemModData[] m_BarSeatsSelectable = null;
    #endregion

    public override string RoomHandlerType => RoomHandlerType_GameIDs.Bar.ToString();
}
