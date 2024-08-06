using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QeustDataSO", menuName = "Papa/QeustDataSO")]
public class QuestDataSO : ScriptableObject
{
    [field: SerializeField] public RequesterType RequesterType { get; private set; }
    [field: SerializeField] public int QuestID {  get; private set; }
    [field: SerializeField] public int DayCondition { get; private set; }
    [field: SerializeField] public int TrustCondition { get; private set; }
    [field: SerializeField] public DoorPaperObject DoorPaperObject { get; private set; }
    [field: SerializeField] public LatterObject LatterObject { get; private set; }
    [field: SerializeField] public ActorElement QuestTarget {  get; private set; }
    [field : SerializeField] public MapDataSo QuestMap { get; private set; }

    

    public bool CheckCanStartQuest()
    {
        return SaveSystem.Save.CurSave.DayCount == DayCondition &&
            SaveSystem.Save.CurSave.RequesterPoints[(int)RequesterType] >= TrustCondition;
    }
}
