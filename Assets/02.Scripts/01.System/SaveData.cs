using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSaveData
{
    public int LastSaveIndex;
    public int CurSaveIndex = -1;
    public SaveData[] SaveDatas;
    public SaveData CurSave => CurSaveIndex == -1 ? null : SaveDatas[CurSaveIndex];
    public GlobalSaveData()
    {
        SaveDatas = new SaveData[3];
    }
    public void Save()
    {
        CurSave.Save();
        LastSaveIndex = CurSaveIndex;
    }

    public void SetCurSaveIndex(int index)
    {
        if (SaveDatas[index] == null)
        {
            SaveDatas[index] = new SaveData();
        }

        CurSaveIndex = index;
    }
}

public class SaveData
{
    public DateTime LastTime;
    public int DayCount;
    public Dictionary<RequesterType, int> RequesterPoints;
    public int HasCoin;

    public SaveData()
    {
        LastTime = DateTime.Now;
        DayCount = 0;
        HasCoin = 50;
        RequesterPoints = new Dictionary<RequesterType, int>();
        foreach (RequesterType type in Enum.GetValues(typeof(RequesterType)))
        {
            RequesterPoints.Add(type, 0);
        }
    }

    public void Save()
    {
        LastTime = DateTime.Now;
    }
}
