using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DRoomPreset
{
    private int m_nID;
    private bool m_bFormatted = false;
    private DRoom d_RoomInfo;
    public DSizeN d_Size;
    public DCell[] d_Data;

    public DRoomPreset(int width, int height)
    {
        d_Size = new DSizeN(width, height);
        d_Data = new DCell[width * height];
    }
    public DRoomPreset(){}
    
    public DRoom GetRoomInfo()
    {
        if (!m_bFormatted)
            Format();
        return new DRoom(d_RoomInfo);
    }

    public void setID(int val)
    {
        m_nID = val;
    }

    private void Format()
    {
        d_RoomInfo = new DRoom();
        m_bFormatted = true;
        d_RoomInfo.d_Size = d_Size;
        d_RoomInfo.d_ID = m_nID;

        //0,0 locates in the left bottom corner.
        for(int i=1; i<d_Size.d_nWidth-1; i++) {
            int _tmp = i;
            if (d_Data[_tmp].d_bIsDoor) {
                d_RoomInfo.d_DoorList.AddLast(new DDoor(i, 0, DDoor.BOTTOM));
            }
            _tmp = i + (d_Size.d_nHeight - 1) * d_Size.d_nWidth;
            if (d_Data[_tmp].d_bIsDoor) {
                d_RoomInfo.d_DoorList.AddLast(new DDoor(i, d_Size.d_nHeight - 1, DDoor.TOP));
            }
        }
        for(int i=1; i<d_Size.d_nHeight-1; i++) {
            int _tmp = i * d_Size.d_nWidth;
            if (d_Data[_tmp].d_bIsDoor) {
                d_RoomInfo.d_DoorList.AddLast(new DDoor(0, i, DDoor.LEFT));
            }
            _tmp = i * d_Size.d_nWidth + d_Size.d_nWidth - 1;
            if (d_Data[_tmp].d_bIsDoor) {
                d_RoomInfo.d_DoorList.AddLast(new DDoor(d_Size.d_nWidth - 1, i, DDoor.RIGHT));
            }
        }
    }
}

public class RoomPresetsManager {
    private Dictionary<int, DRoomPreset> m_Presets;
    private int m_nNextID = 0;

    static RoomPresetsManager _inst;
    public static RoomPresetsManager GetInstance()
    {
        if (_inst == null)
            _inst = new RoomPresetsManager();
        return _inst;
    }

    private RoomPresetsManager()
    {
        m_Presets = new Dictionary<int, DRoomPreset>();
    }

    public void AddPreset(DRoomPreset preset)
    {
        m_Presets.Add(m_nNextID, preset);
        preset.setID(m_nNextID++);
    }

    public DRoomPreset GetPreset(int ID)
    {
        if (m_Presets.ContainsKey(ID))
            return m_Presets[ID];
        else
            return null;
    }

    public DRoomPreset RandomPicking() {
        int _tmp = Random.Range(0, m_Presets.Count);
        return m_Presets[_tmp];
    }

    public void LoadPreset(string file)
    {

    }

}
