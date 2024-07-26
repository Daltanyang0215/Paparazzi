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
    // �ʱ� �ŷڵ�
    [field: SerializeField] public int RequestInitTrust { get; private set; }
    // �ִ� �ŷڵ�
    [field: SerializeField] public int RequestMaxTrust { get; private set; }
    // �ŷڵ� ��ȭ�� (������ �˸����� ���, ���� ������ �϶�)
    [field: SerializeField] public int RequestDaltaTrust { get; private set; }
    // ����
    [field: SerializeField] public int RequestReward { get; private set; }
    // ���н� ����
    [field: SerializeField] public int RequestPenalty { get; private set; }

    [field: Header("Event")]
    [field: SerializeField] public List<RequesterEventData> EventDatas { get; private set; }

    public RequesterEventData GetEventData()
    {
        RequesterEventData result = null;
        foreach (var data in EventDatas)
        {
            // �ش� ���ڰ� �̺�Ʈ �߻� ���� ���� �� �̶�� ����
            if (DaySystem.Instance.DayCount < data.EventStartDay) break;

            // �ش� ���ڰ� �̺�Ʈ �߻� ���� �̰ų� ���Ķ�� �����͸� ����
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