using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RequesterData", menuName = "Papa/RequesterData")]
public class RequesterData : ScriptableObject
{
    [field: Header("Data")]
    [field: SerializeField] public string RequsterName { get; private set; }
    [field: SerializeField] public Sprite RequsterMarker { get; private set; }
    [field: SerializeField] public Color RequsterColor { get; private set; }

    [field: Header("CalculateData")]
    // 초기 신뢰도
    [field: SerializeField] public int RequestInitTrust { get; private set; }
    // 최대 신뢰도
    [field: SerializeField] public int RequestMaxTrust { get; private set; }
    // 신뢰도 변화량 (시진이 알맞으면 상승, 맞지 않으면 하락)
    [field: SerializeField] public int RequestDaltaTrust { get; private set; }
    // 보수
    [field: SerializeField] public int RequestReward { get; private set; }
    // 실패시 벌금
    [field: SerializeField] public int RequestPenalty { get; private set; }

    [field: Header("Event")]
    [field: SerializeField] public List<RequesterEventData> EventDatas { get; private set; }

    public RequesterEventData GetEventData()
    {
        RequesterEventData result = null;
        foreach (var data in EventDatas)
        {
            // 해당 날자가 이벤트 발생 시점 보다 전 이라면 종료
            if (DaySystem.Instance.DayCount < data.EventStartDay) break;

            // 해당 날자가 이벤트 발생 시점 이거나 이후라면 데이터를 리턴
            result = data;
        }
        return result;
    }

}

[System.Serializable]
public class RequesterEventData
{
    [field: SerializeField] public int EventStartDay { get; private set; }
    [field: SerializeField] public DoorPaperObject DoorPaperObject { get; private set; }
    [field: SerializeField] public LatterObject LatterObject { get; private set; }
    [field: SerializeField] public ActorElement EventTarget { get; private set; }
}